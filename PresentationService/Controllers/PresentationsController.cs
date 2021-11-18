using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationService.Models;
using PresentationService.Services.Implementations;
using PresentationService.Services.Interfaces;

namespace PresentationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationsController : ControllerBase
    {
        private readonly PresentationContext _context;
        private IWorkerWithDB _presentationWorker;

        public PresentationsController(PresentationContext context, IWorkerWithDB workerWithDB)
        {
            _context = context;
            _presentationWorker = workerWithDB;

            _presentationWorker.Context = _context;
        }

        [HttpGet]
        public async Task<List<Presentation>> GetPresentations(DateTime dateFrom, DateTime dateTo)
        {
            return _presentationWorker.GetAllFields(dateFrom: dateFrom, dateTo: dateTo);
        }

        [HttpGet("{id}")]
        public Task<Presentation> GetPresentation(int id)
        {
            return  _presentationWorker.GetField(id);
        }

        [HttpGet("GetDefinedValue/{qauntity}")]
        public async Task<List<Presentation>> GetDefinedValuePresentation(int qauntity)
        {
            return _context.Presentation.OrderBy(p => p.Id).Take(qauntity).ToList();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPresentation(int id, Presentation presentation)
        {
            var t =  await _presentationWorker.ChangeField();

            if(t != 0)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Presentation>> PostPresentation(Presentation presentation)
        {
            await _presentationWorker.AddingField(presentation);
            var t = _context.Presentation.Add(presentation);

            return CreatedAtAction("GetPresentation", t);
        }

        // DELETE: api/Presentations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePresentation(int id)
        {
            var presentation = await _context.Presentation.FindAsync(id);
            if (presentation == null)
            {
                return NotFound();
            }
            await _presentationWorker.DeleteField(id);

            return NoContent();
        }

        private bool PresentationExists(int id)
        {
            return _context.Presentation.Any(e => e.Id == id);
        }
    }
}
