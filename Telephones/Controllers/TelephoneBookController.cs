using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telephones.Data;
using Telephones.Data.Models;
using Telephones.ViewModels;

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
        /// Действие для отображения конкретной записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns>Страница конкретной записи. Страница ошибки NotFound, если запись не найдена</returns>
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
        /// Действие для отображения страницы обноления данных записи
        /// </summary>
        /// <param name="id">Идентификатор записи для поиска</param>
        /// <returns>Страница обнвления записи. Страница ошибки NotFound, если запись не найдена</returns>
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

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRecordViewModel viewModel)
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
        /// Действие для добавления новой записи
        /// </summary>
        /// <returns>Страница добавления записи. Страница ошибки NotFound, если запись не найдена</returns>
        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            return View(new CreateRecordViewModel());
        }

        /// <summary>
        /// Добавляет новую запись
        /// </summary>
        /// <param name="vmodel">Данные для добавления</param>
        /// <returns>Возвращает на стартовую старицу сайта</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecordViewModel vmodel)
        {
            Record model = _mapper.Map<CreateRecordViewModel, Record>(vmodel);
            await _context.Records.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "TelephoneBook");
        }

        [HttpPost]
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
