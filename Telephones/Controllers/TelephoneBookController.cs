using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telephones.Data;
using Telephones.Data.Models;
using Telephones.ViewModels;
using System.Linq;

namespace Telephones.Controllers
{
    /// <summary>
    /// Контроллер для телефонной книги
    /// </summary>
    public class TelephoneBookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TelephoneBookController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Начальная страница отображает список записей в телефонной книге
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<ShortRecordViewModel> result = _mapper.Map<IEnumerable<ShortRecordViewModel>>(await _context.Records.ToListAsync());
            var r = Url.ActionLink();
            return View(result);
        }

        /// <summary>
        /// Страница для поиска отдельной записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<RecordViewModel>(record));
        }

        /// <summary>
        /// Действие страницы для обновления записи
        /// </summary>
        /// <param name="id">Идентификатор записи для обновления</param>
        /// <returns>Страница обновления записи. Ошибка NotFound в случае неудачи</returns>
        [HttpGet]
        public async Task<IActionResult> Update(int? id) 
        {
            if (id == null || _context.Records == null) 
            {
                return NotFound();
            }

            Record? model = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null) 
            {
                return NotFound();
            }

            return View(_mapper.Map<Record, UpdateRecordViewModel>(model));
        }

        /// <summary>
        /// Действие для обновления данных записи
        /// </summary>
        /// <param name="viewModel">Данные для обновления</param>
        /// <returns>Результат выполения запроса</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateRecordViewModel viewModel)
        {
            Record model = _mapper.Map<UpdateRecordViewModel, Record>(viewModel);

            if (_context.Records.Any(m => m.Id == model.Id) && ModelState.IsValid)
            {
                _context.Records.Update(model);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return new NotFoundResult();
        }

        /// <summary>
        /// Действия для станицы добавления записи
        /// </summary>
        /// <returns>Старица для доавбеления записи</returns>
        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            return View(new CreateRecordViewModel());
        }

        /// <summary>
        /// Действие для добавления новой записи
        /// </summary>
        /// <param name="vmodel">Запись для добавления</param>
        /// <returns>Редирект на стартовую страницу сайта</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecordViewModel vmodel)
        {
            Record model = _mapper.Map<CreateRecordViewModel, Record>(vmodel);
            await _context.Records.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "TelephoneBook");
        }

        /// <summary>
        /// Дейтсвие для удаления записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Результат выполнения действия</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id is not null && _context is not null) 
            {
                Record? model = _context.Records.FirstOrDefault(m => m.Id == id);

                if (model is not null) 
                {
                    _context.Records.Remove(model);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }

            return NotFound();
        }
    }
}
