using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationService.Models;

namespace PresentationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresentationsController : ControllerBase
    {
        private readonly PresentationContext _context;

        public PresentationsController(PresentationContext context)
        {
            _context = context;
        }

        // GET: api/Presentations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Presentation>>> GetPresentations()
        {
            return await _context.Presentation.ToListAsync();
        }

        // GET: api/Presentations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Presentation>> GetPresentation(int id)
        {
            var presentation = await _context.Presentation.FindAsync(id);

            if (presentation == null)
            {
                return NotFound();
            }

            return presentation;
        }

        // PUT: api/Presentations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPresentation(int id, Presentation presentation)
        {
            if (id != presentation.Id)
            {
                return BadRequest();
            }

            _context.Entry(presentation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresentationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Presentations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Presentation>> PostPresentation(Presentation presentation)
        {
            _context.Presentation.Add(presentation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPresentation", new { id = presentation.Id }, presentation);
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

            _context.Presentation.Remove(presentation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PresentationExists(int id)
        {
            return _context.Presentation.Any(e => e.Id == id);
        }
    }
}
