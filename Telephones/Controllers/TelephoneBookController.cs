using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telephones.Data;
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
        public async Task<IActionResult> Index()
        {
            IEnumerable<ShortRecordViewModel> result = _mapper.Map<IEnumerable<ShortRecordViewModel>>(await _context.Records.ToListAsync());
            return View(result);
        }

        /// <summary>
        /// Страница для поиска отдельной записи
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        /// <returns></returns>
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
    }
}
