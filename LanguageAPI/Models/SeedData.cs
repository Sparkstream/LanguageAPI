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
                if (context.LanguageItem.Count() == 0)
                {
                    context.LanguageItem.AddRange(
                        new LanguageItem
                        {
                            userId = 1,
                            rank = 1,
                            languageName = "Chinese (Simplified)",
                            languageCode = "zh-CN",
                            word = "你好"
                        },
                        new LanguageItem
                        {
                            userId = 1,
                            rank = 2,
                            languageName = "Chinese (Simplified)",
                            languageCode = "zh-CN",
                            word = "你在做什么？"

                        },
                        new LanguageItem
                        {
                            userId = 1,
                            rank = 3,
                            languageName = "Chinese (Simplified)",
                            languageCode = "zh-CN",
                            word = "狗"
                        },
                        new LanguageItem
                        {
                            userId = 2,
                            rank = 1,
                            languageName = "English",
                            languageCode = "en",
                            word = "hi"

                        },
                        new LanguageItem
                        {
                            userId = 2,
                            rank = 1,
                            languageName = "English",
                            languageCode = "en",
                            word = "stuff"

                        }
                    );
                    context.SaveChanges();
                }
                
                if (context.UserInfo.Count() == 0)
                {
                    context.UserInfo.AddRange(
                       new UserInfo
                       {
                           username = "Sparkstream",
                           password = "abcd1234"
                       },
                       new UserInfo
                       {
                           username = "Sixthshadow42",
                           password = "abcd1234"
                       }
                    );  
                }

                context.SaveChanges();
              
            }
        }
    }
}
