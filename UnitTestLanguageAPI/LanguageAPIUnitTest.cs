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
    public class LanguageAPIUnitTest
    {
        public static readonly DbContextOptions<LanguageAPIContext> options =
            new DbContextOptionsBuilder<LanguageAPIContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase")
            .Options;

        public static readonly IList<string> favouriteWords = new List<string> { "球", "帽子" };
        public static readonly IList<LanguageItem> languageItemList = new List<LanguageItem> {
            new LanguageItem()
            {
                userId = 1,
                rank = 1,
                languageName = "Chinese (Simplified)",
                languageCode = "zh-CN",
                word = "球"
            },
            new LanguageItem()
            {
                userId = 1,
                rank = 2,
                languageName = "Chinese (Simplified)",
                languageCode = "zh-CN",
                word = "帽子"
            }
        };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new LanguageAPIContext(options))
            {
                LanguageItem languageItem1 = new LanguageItem()
                {
                    userId = 1,
                    rank = 1,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "球"
                };
                LanguageItem languageItem2 = new LanguageItem()
                {
                    userId = 1,
                    rank = 2,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "帽子"
                };
                context.LanguageItem.Add(languageItem1);
                context.LanguageItem.Add(languageItem2);
                context.SaveChanges();
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

        /*============================================================================================
                                   _____   ____   _____ _______ 
                                  |  __ \ / __ \ / ____|__   __|
                                  | |__) | |  | | (___    | |   
                                  |  ___/| |  | |\___ \   | |   
                                  | |    | |__| |____) |  | |   
                                  |_|     \____/|_____/   |_|   

        * ===========================================================================================*/

        [TestMethod]
        public async Task TestAddFavouriteWord()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                LanguageItem languageItem = new LanguageItem()
                {
                    userId = 1,
                    rank = 3,
                    languageName = "English",
                    languageCode = "en",
                    word = "Amazing"

                };
                //When
                LanguageItemsController languageItemsController = new LanguageItemsController(context);
                IActionResult result = await languageItemsController.AddFavouriteWord(languageItem) as IActionResult;

                //Then
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));

                var exists = await context.LanguageItem.Where(l =>
                    l.userId == languageItem.userId &&
                    l.rank == languageItem.rank &&
                    l.languageName == languageItem.languageName &&
                    l.languageCode == languageItem.languageCode &&
                    l.word == languageItem.word
                ).AnyAsync();
                Assert.IsNotNull(exists);
                Assert.IsTrue(exists);

            }
        }

        //Check for duplicate
        [TestMethod]
        public async Task TestAddExistingWord()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                LanguageItem languageItem1 = new LanguageItem()
                {
                    userId = 1,
                    rank = 3,
                    languageName = "English",
                    languageCode = "en",
                    word = "Amazing"

                };
                LanguageItem languageItem2 = new LanguageItem()
                {
                    userId = 1,
                    rank = 4,
                    languageName = "English",
                    languageCode = "en",
                    word = "Amazing"

                };
                //When
                LanguageItemsController languageItemsController = new LanguageItemsController(context);
                IActionResult result = await languageItemsController.AddFavouriteWord(languageItem1) as IActionResult;
                IActionResult result2 = await languageItemsController.AddFavouriteWord(languageItem2) as IActionResult;

                //Then
                Assert.IsNotNull(result);
                Assert.IsNotNull(result2);
                Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
                Assert.IsInstanceOfType(result2, typeof(ConflictResult));

                var exists = await context.LanguageItem.Where(l =>
                    l.userId == languageItem1.userId &&
                    l.rank == languageItem1.rank &&
                    l.languageName == languageItem1.languageName &&
                    l.languageCode == languageItem1.languageCode &&
                    l.word == languageItem1.word
                ).AnyAsync();

                Assert.IsNotNull(exists);
                Assert.IsTrue(exists);

            }
        }

        [TestMethod]
        public async Task TestRetrieveWords()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                LanguageItem languageItem1 = new LanguageItem()
                {
                    userId = 1,
                    rank = 1,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "球"
                };
                LanguageItem languageItem2 = new LanguageItem()
                {
                    userId = 1,
                    rank = 2,
                    languageName = "Chinese (Simplified)",
                    languageCode = "zh-CN",
                    word = "帽子"
                };

                //When
                LanguageItemsController languageItemsController = new LanguageItemsController(context);
                IActionResult result = await languageItemsController.RetrieveFavouriteWords(languageItem1.userId);
                
                //Then
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                Assert.IsNotNull(result);

                var checkResponse = result as OkObjectResult;
                Assert.IsNotNull(checkResponse);

                var languageItems = checkResponse.Value as List<LanguageItem>;
                for(var i = 0; i < languageItems.Count; i++)
                {
                    Assert.AreEqual(languageItems[i].languageName,languageItemList[i].languageName);
                    Assert.AreEqual(languageItems[i].languageCode, languageItemList[i].languageCode);
                    Assert.AreEqual(languageItems[i].rank, languageItemList[i].rank);
                    Assert.AreEqual(languageItems[i].word, languageItemList[i].word);
                }
                
            }
        }


    }
}
