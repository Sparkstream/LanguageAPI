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

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody]UserInfo userInfo)
        {
            var id = userInfo.Id;
            var user = await _context.UserInfo.FindAsync(id);
            if((user.username != userInfo.username) || (user.password != userInfo.password)) 
            {
                return Unauthorized();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/user")]
        public async Task<IActionResult> RegisterUser([FromForm] UserInfo userInfo)
        {
            _context.UserInfo.Add(userInfo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("/update")]
        public async Task<IActionResult> UpdateUser([FromForm] UserInfo userInfo)
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