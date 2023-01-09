using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Telephones.Data;
using Telephones.ViewModels;
using Telephones.API.Client.Interfaces;
using Telephones.API.Client.DTO;

namespace Telephones.Controllers
{
    /// <summary>
    /// Контроллер для телефонной книги
    /// </summary>
    public class TelephoneBookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITelephoneBookClientAPI _clientAPI;

        public TelephoneBookController(AppDbContext context, IMapper mapper, ITelephoneBookClientAPI clientAPI)
        {
            _clientAPI = clientAPI;
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
            WrapperResultDTO<IEnumerable<ShortRecordDTO>> resultQuery = await _clientAPI.GetRecordsAsync();
            if (resultQuery.IsSuccess) 
            {
                IEnumerable<ShortRecordViewModel> result = _mapper.Map<IEnumerable<ShortRecordViewModel>>(resultQuery.Result);
                return View(result);
            }
            
            return View();
        }

        /// <summary>
        /// Страница для поиска отдельной записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
            {
                return View();
            }
            WrapperResultDTO<RecordDTO> resultQuery = await _clientAPI.GetRecordAsync(id);

            if (resultQuery.IsSuccess) 
            {
                return View(_mapper.Map<RecordViewModel>(resultQuery.Result));
            }

            return View();
        }

        /// <summary>
        /// Действие страницы для обновления записи
        /// </summary>
        /// <param name="id">Идентификатор записи для обновления</param>
        /// <returns>Страница обновления записи. Ошибка NotFound в случае неудачи</returns>
        [HttpGet]
        public async Task<IActionResult> Update(int? id) 
        {
            if (id == null)
            {
                return View();
            }
            WrapperResultDTO<RecordDTO> resultQuery = await _clientAPI.GetRecordAsync(id);

            if (resultQuery.IsSuccess)
            {
                return View(_mapper.Map<UpdateRecordViewModel>(resultQuery.Result));
            }

            return View();
        }

        /// <summary>
        /// Действие для обновления данных записи
        /// </summary>
        /// <param name="viewModel">Данные для обновления</param>
        /// <returns>Результат выполения запроса</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateRecordViewModel viewModel)
        {
            UpdateRecordDTO dto = _mapper.Map<UpdateRecordViewModel, UpdateRecordDTO>(viewModel);

            WrapperResultDTO<int> resultQuery = await _clientAPI.UpdateRecordAsync(dto);

            if (!resultQuery.IsSuccess)
            {
                return BadRequest();
            }

            return Ok(dto);
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
            CreateRecordDTO dto = _mapper.Map<CreateRecordViewModel, CreateRecordDTO>(vmodel);
            WrapperResultDTO<int> resultQuery = await _clientAPI.CreateRecordAsync(dto);

            if (!resultQuery.IsSuccess) 
            {
                return BadRequest();
            }

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
            WrapperResultDTO<int> resultQuery = await _clientAPI.DeleteRecordAsync(id);

            if (!resultQuery.IsSuccess) 
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
