using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LanguageAPI.Models;

namespace LanguageAPI.Models
{
    public class LanguageAPIContext : DbContext
    {
        public LanguageAPIContext (DbContextOptions<LanguageAPIContext> options)
            : base(options)
        {
        }

        public DbSet<LanguageAPI.Models.LanguageItem> LanguageItem { get; set; }

        public DbSet<LanguageAPI.Models.UserInfo> UserInfo { get; set; }
    }
}
