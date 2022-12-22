using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telephones.API.Data;
using Telephones.API.Data.Models;
using Telephones.API.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Telephones.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelephonesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TelephonesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<TelephonesController>
        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(_mapper.Map<IEnumerable<ShortRecordViewModel>>(_context.Records));
        }

        // GET api/<TelephonesController>/5
        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            if (_context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RecordViewModel>(record));
        }

        // POST api/<TelephonesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateRecordViewModel vmodel)
        {
            Record model = _mapper.Map<CreateRecordViewModel, Record>(vmodel);
            await _context.Records.AddAsync(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // PUT api/<TelephonesController>/5
        [HttpPut("[action]/{id:guid}")]
        public async Task<IActionResult> Put([FromBody] UpdateRecordViewModel viewModel)
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

        // DELETE api/<TelephonesController>/5
        [HttpDelete("[action]/{id:guid}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context is not null)
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
