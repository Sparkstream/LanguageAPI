using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LanguageAPI.Models;

namespace LanguageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly LanguageAPIContext _context;

        public UserInfoController(LanguageAPIContext context)
        {
            _context = context;
        }

        // GET: api/LanguageItems
        [HttpGet]
        public IEnumerable<UserInfo> GetUsers()
        {
            return _context.UserInfo;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody]UserInfo userInfo)
        {
            //Check if user exists
            var user = await _context.UserInfo.Where(u =>
                u.username == userInfo.username
            ).FirstOrDefaultAsync();
            //Doesnt then bad request
            if(user == null)
            {
                return BadRequest();
            }
            //Wrong password then return unauthorized
            if((user.username != userInfo.username) || (user.password != userInfo.password)) 
            {
                return Unauthorized();
            }
            
            return Ok(user);
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserInfo _userInfo)
        {
            if(string.IsNullOrEmpty(_userInfo.password) || string.IsNullOrEmpty(_userInfo.username))
            {
                return UnprocessableEntity();
            }
            var exists = await _context.UserInfo.Where(u =>
                u.username == _userInfo.username
            ).AnyAsync();

            if (exists)
            {
                return Conflict();
            }
            UserInfo userInfo = new UserInfo()
            {
                username = _userInfo.username,
                password = _userInfo.password
            };
            
            _context.UserInfo.Add(userInfo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserInfo userInfo)
        {
            var user = _context.UserInfo.Find(userInfo.Id);
            user.password = userInfo.password;
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool UserInfoExists(int id)
        {
            return _context.UserInfo.Any(e => e.Id == id);
        }
    }
}