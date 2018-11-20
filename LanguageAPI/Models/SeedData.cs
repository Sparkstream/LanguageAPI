using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageAPI.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new LanguageAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<LanguageAPIContext>>()))
            {
                // Look for any movies.
                if (context.LanguageItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.LanguageItem.AddRange(
                    new LanguageItem
                    {
                        userId = 0,
                        rank = 1,
                        languageName = "Chinese (Simplified)",
                        languageCode = "zh-CN",
                        word = "你好"
                    },
                    new LanguageItem { 
                        userId = 0,
                        rank = 2,
                        languageName = "Chinese (Simplified)",
                        languageCode = "zh-CN",
                        word = "你在做什么？"
                          
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
