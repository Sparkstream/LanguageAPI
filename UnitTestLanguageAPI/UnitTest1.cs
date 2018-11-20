using LanguageAPI.Controllers;
using LanguageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace UnitTestLanguageAPI
{
    [TestClass]
    public class UnitTest1
    {
        public static readonly DbContextOptions<LanguageAPIContext> options =
            new DbContextOptionsBuilder<LanguageAPIContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase")
            .Options;

        public static IConfiguration configuration = null;
        public static readonly IList<string> favouriteWords = new List<string> { "球", "帽子" };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new LanguageAPIContext(options))
            {
                LanguageItem languageItem1 = new LanguageItem()
                {
                    userId = 0,
                    rank = 1,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "球"
                };
                LanguageItem languageItem2 = new LanguageItem()
                {
                    userId = 0,
                    rank = 2,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "帽子"
                };
                context.LanguageItem.Add(languageItem1);
                context.LanguageItem.Add(languageItem2);
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new LanguageAPIContext(options))
            {
                context.LanguageItem.RemoveRange(context.LanguageItem);
                context.SaveChanges();
            }
        }
        [TestMethod]

        /*[TestMethod]
        public async Task TestChangeWordRanking()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                string words = "球";
                LanguageItem languageItem1 = context.LanguageItem.Where(x => x.word == favouriteWords[0]).Single();
                languageItem1.rank = 2;

                //When
                LanguageItemsController languageItemsController = new LanguageItemsController(context, configuration);
                IActionResult result = await languageItemsController.PutLanguageItem(languageItem1.Id, languageItem);

            }
        }*/
    }
}
