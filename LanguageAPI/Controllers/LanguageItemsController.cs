using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageAPI.Helpers;
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
 

        public LanguageItemsController(LanguageAPIContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public async Task<IActionResult> RetrieveAllWords([FromBody] LanguageItem languageItem)
        {
            IQueryable<LanguageItem> _languageItem = from l in _context.LanguageItem
                                where l.userId == languageItem.userId
                                select l;
            var response =  await _languageItem.ToListAsync();

           //var _languageItem = await _context.LanguageItem.FindAsync(languageItem.userId);
            return Ok(response);
        }

        //Add cookies
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> RetrieveFavouriteWords(int id)
        {
            IQueryable<LanguageItem> _languageItem = from l in _context.LanguageItem
                                                     where l.userId == id
                                                     select l;
            
            var response = await _languageItem.ToListAsync();
            return Ok(response);
            
        }

        [HttpPost]
        public async Task<IActionResult> AddFavouriteWord([FromBody]LanguageItem languageItem)
        {
            if (!ValidData.isValidDatabaseEntry(languageItem))
            {
                return BadRequest();
            }
            var exists = await _context.LanguageItem.Where(l =>
                l.userId == languageItem.userId &&
                l.languageName == languageItem.languageName &&
                l.languageCode == languageItem.languageCode &&
                l.word == languageItem.word
            ).AnyAsync();
            if (exists)
            {
                return Conflict();
            }
            _context.LanguageItem.Add(languageItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLanguageItem", languageItem);

        }

        //Language name or code MUST BE SENT
        [HttpPut]
        public async Task<IActionResult> ChangeRanking([FromBody]LanguageItem languageItem)
        {
            //Find all words for that language for that user
            var rank = languageItem.rank;
            var languageItems = await _context.LanguageItem.Where(l => 
            l.userId == languageItem.userId 
            && l.languageName == languageItem.languageName).ToListAsync();
            languageItems.ForEach(l =>
            {
                if (l.Id == languageItem.Id)
                {
                    l.rank = rank;
                }
                else if(l.rank>=rank)
                {
                    l.rank++;
                }
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        //Give a id of an item to remove
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteWord(int id)
        {
            if (!LanguageItemExists(id))
            {
                return NotFound();
            }
            var languageItem = _context.LanguageItem.Find(id);

            var languageItems = await _context.LanguageItem.Where(l =>
            l.userId == languageItem.userId
            && l.languageName == languageItem.languageName).ToListAsync();

            _context.LanguageItem.Remove(languageItem);

            languageItems.ForEach(l =>
            {
                if (l.rank >= languageItem.rank)
                {
                    l.rank--;
                }
            });
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool LanguageItemExists(int id)
        {
            return _context.LanguageItem.Any(e => e.Id == id);
        }
    }
}