using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telephones.API.Data;
using Telephones.API.Data.Models;
using Telephones.API.Infrastructure;
using Telephones.API.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Telephones.API.Controllers
{
    /// <summary>
    /// Контроллет для API данных телефонной книги
    /// </summary>
    [Route("api/[controller]")]
    public class TelephonesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TelephonesController> _logger;

        public TelephonesController(AppDbContext context, IMapper mapper, ILogger<TelephonesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Предоставляет записи из телефонной в уменьшенном формате. Метод GET
        /// </summary>
        // GET: api/<TelephonesController>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            WrapperResult<IEnumerable<ShortRecordDTO>> result = WrapperResult.Build<IEnumerable<ShortRecordDTO>>();

            try
            {
                result.Result =  _mapper.Map<IEnumerable<ShortRecordDTO>>(_context.Records);
                return Ok(result);
            }
            catch (Exception e) 
            {
                string errorMessage = $"Unknow Error {e.Message}";
                result.ExceptionObject = e;
                result.Message = errorMessage;
                _logger.LogError(errorMessage);
                return Ok(result);
            }
        }

        /// <summary>
        /// Предоставляет полные данные о конкретной записи. Метод GET
        /// </summary>
        /// <param name="id">Идентификатор записи</param>
        // GET api/<TelephonesController>/5
        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            WrapperResult<RecordDTO> result = WrapperResult.Build<RecordDTO>();

            try
            {
                var @record = await _context.Records
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (@record == null)
                {
                    string errorMessage = "Record not found";
                    result.ExceptionObject = new KeyNotFoundException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }

                result.Result = _mapper.Map<RecordDTO>(record);
                return Ok(result);
            }
            catch (Exception e) 
            {
                string errorMessage = $"Unknow Error {e.Message}";
                result.ExceptionObject = e;
                result.Message = errorMessage;
                _logger.LogError(errorMessage);
                return Ok(result);
            }
            
        }

        /// <summary>
        /// Создает новую запись в источнике данных. Метод POST
        /// </summary>
        /// <param name="vmodel">Данные для создания записи</param>
        /// <returns>Результат операции</returns>
        // POST api/<TelephonesController>
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody]CreateRecordDTO vmodel)
        {
            WrapperResult result = WrapperResult.Build<int>();
            try
            {
                if (vmodel is null)
                {
                    string errorMessage = "Record is NULL!!!";
                    result.ExceptionObject = new NullReferenceException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }
                if (!ModelState.IsValid)
                {
                    string errorMessage = "Record is invalid";
                    result.ExceptionObject = new InvalidDataException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }
                if (_context.Records.Any(m => m.PhoneNumber == vmodel.PhoneNumber))
                {
                    string errorMessage = "Record with this phone number already exist";
                    result.ExceptionObject = new ArgumentException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }
                Record model = _mapper.Map<CreateRecordDTO, Record>(vmodel);
                await _context.Records.AddAsync(model);
                await _context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception e) 
            {
                string errorMessage = $"Unknow Error {e.Message}";
                result.ExceptionObject = e;
                result.Message = errorMessage;
                _logger.LogError(errorMessage);
                return Ok(result);
            }
            
        }

        /// <summary>
        /// Обновляет данные о записи в телефонной книге.
        /// Метод PUT.
        /// </summary>
        /// <param name="viewModel">Обновленные данные записи для обновления</param>
        /// <returns>Результат операции</returns>
        // PUT api/<TelephonesController>/5
        [HttpPut("[action]/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateRecordDTO viewModel)
        {
            WrapperResult result = WrapperResult.Build<int>();

            try 
            {
                if (viewModel is null)
                {
                    string errorMessage = "Record is NULL!!!";
                    result.ExceptionObject = new NullReferenceException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }

                if (!_context.Records.Any(m => m.Id == viewModel.Id))
                {
                    string errorMessage = "Record with this ID number not exist";
                    result.ExceptionObject = new ArgumentException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }

                if (!ModelState.IsValid)
                {
                    string errorMessage = "Record is invalid";
                    result.ExceptionObject = new InvalidDataException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }

                Record model = _mapper.Map<UpdateRecordDTO, Record>(viewModel);

                if (_context.Records.Any(m => m.Id == model.Id) && ModelState.IsValid)
                {
                    _context.Records.Update(model);
                    await _context.SaveChangesAsync();
                    return Ok(result);
                }
                
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                string errorMessage = $"Unknow Error {e.Message}";
                result.ExceptionObject = e;
                result.Message = errorMessage;
                _logger.LogError(errorMessage);
                return Ok(result);
            }
            
        }

        /// <summary>
        /// Удаляет данные о записи в источнике данных
        /// Метод DELETE
        /// </summary>
        /// <param name="id">Идентификатор записи для удаления</param>
        /// <returns>Результат операции</returns>
        // DELETE api/<TelephonesController>/5
        [HttpDelete("[action]/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            WrapperResult result = WrapperResult.Build<int>();
            try
            {
                if (id == null) 
                {
                    string errorMessage = "ID is null";
                    result.ExceptionObject = new ArgumentException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);
                }
                if (!_context.Records.Any(m => m.Id == id))
                {
                    string errorMessage = "Record with this ID number not exist";
                    result.ExceptionObject = new ArgumentException();
                    result.Message = errorMessage;
                    _logger.LogError(errorMessage);
                    return Ok(result);

                }
                Record model = _context.Records.FirstOrDefault(m => m.Id == id)!;
                _context.Records.Remove(model);
                await _context.SaveChangesAsync();
                return Ok(result);
            }
            catch (Exception e) 
            {
                string errorMessage = $"Unknow Error {e.Message}";
                result.ExceptionObject = e;
                result.Message = errorMessage;
                _logger.LogError(errorMessage);
                return Ok(result);
            }
        }
    }
}
