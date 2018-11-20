using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageItemsController : ControllerBase
    {
        private readonly LanguageAPIContext _context;

        public LanguageItemsController(LanguageAPIContext context)
        {
            _context = context;
        }

        // GET: api/LanguageItems
        [HttpGet]
        public IEnumerable<LanguageItem> GetLanguageItem()
        {
            return _context.LanguageItem;
        }

        // GET: api/LanguageItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLanguageItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var languageItem = await _context.LanguageItem.FindAsync(id);

            if (languageItem == null)
            {
                return NotFound();
            }

            return Ok(languageItem);
        }

        // PUT: api/LanguageItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguageItem([FromRoute] int id, [FromBody] LanguageItem languageItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != languageItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(languageItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageItemExists(id))
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

        // POST: api/LanguageItems
        [HttpPost]
        public async Task<IActionResult> PostLanguageItem([FromBody] LanguageItem languageItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LanguageItem.Add(languageItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguageItem", new { id = languageItem.Id }, languageItem);
        }

        // DELETE: api/LanguageItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguageItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var languageItem = await _context.LanguageItem.FindAsync(id);
            if (languageItem == null)
            {
                return NotFound();
            }

            _context.LanguageItem.Remove(languageItem);
            await _context.SaveChangesAsync();

            return Ok(languageItem);
        }

        private bool LanguageItemExists(int id)
        {
            return _context.LanguageItem.Any(e => e.Id == id);
        }
    }
}