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

        //Add cookies
        [HttpGet]
        [Route("test/{id}")]
        public async Task<IActionResult> retrieveFavouriteWords(int id)
        {
            IQueryable<LanguageItem> _languageItem = from l in _context.LanguageItem
                                                     where l.userId == id
                                                     select l;
            
            var response = await _languageItem.ToListAsync();
            if(response[0] != null)
            {
                return NoContent();
            }
            //var _languageItem = await _context.LanguageItem.FindAsync(languageItem.userId);
            return Ok(response[0]);
        }

        [HttpPost]
        public async Task<IActionResult> addFavouriteWord([FromBody]LanguageItem languageItem)
        {
            if (!ValidData.isValidDatabaseEntry(languageItem))
            {
                return BadRequest();
            }
            _context.LanguageItem.Add(languageItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLanguageItem", languageItem);

        }

        [HttpPut]
        public async Task<IActionResult> changeRanking([FromBody]LanguageItem languageItem)
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
            return Ok();
        }
        

        private bool LanguageItemExists(int id)
        {
            return _context.LanguageItem.Any(e => e.Id == id);
        }
    }
}