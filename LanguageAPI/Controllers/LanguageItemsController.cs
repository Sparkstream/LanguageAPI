using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LanguageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageItemsController : ControllerBase
    {
        private readonly LanguageAPIContext _context;
        private IConfiguration _configuration;

        public LanguageItemsController(LanguageAPIContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/LanguageItems
        [HttpGet]
        public IEnumerable<LanguageItem> GetLanguageItem()
        {
            return _context.LanguageItem;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> retrieveAllWords([FromBody] LanguageItem languageItem)
        {
            IQueryable<LanguageItem> _languageItem = from l in _context.LanguageItem
                                where l.userId == languageItem.userId
                                select l;
            var response =  await _languageItem.ToListAsync();

           //var _languageItem = await _context.LanguageItem.FindAsync(languageItem.userId);
            return Ok(response);
        }

        [HttpGet]
        [Route("test/{id}")]
        public async Task<IActionResult> retrieveWords(int id)
        {
            IQueryable<LanguageItem> _languageItem = from l in _context.LanguageItem
                                                     where l.userId == id
                                                     select l;
            var response = await _languageItem.ToListAsync();

            //var _languageItem = await _context.LanguageItem.FindAsync(languageItem.userId);
            return Ok(response[0]);
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