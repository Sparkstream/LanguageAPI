using LanguageAPI.Controllers;
using LanguageAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestLanguageAPI
{
    [TestClass]
    public class UserInfoUnitTest
    {
        public static readonly DbContextOptions<LanguageAPIContext> options =
            new DbContextOptionsBuilder<LanguageAPIContext>()
            .UseInMemoryDatabase(databaseName: "testDatabase")
            .Options;

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new LanguageAPIContext(options))
            {
                UserInfo user1 = new UserInfo()
                {
                    username = "Sparkstream",
                    password = "abcd1234"
                };
                UserInfo user2 = new UserInfo()
                {
                    username = "Sixthshadow42",
                    password = "abcd1234"
                };
                context.UserInfo.Add(user1);
                context.UserInfo.Add(user2);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new LanguageAPIContext(options))
            {
                context.UserInfo.RemoveRange(context.UserInfo);
                context.SaveChanges();
            }
        }
        
        [TestMethod]
        public async Task testAddUser()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                UserInfo user = new UserInfo()
                {
                    username = "Bob",
                    password = "abcd1234"
                };
                //When
                UserInfoController userInfoController= new UserInfoController(context);
                IActionResult result = await userInfoController.RegisterUser(user);

                var userExists = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).AnyAsync();
                var dbUser = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).FirstOrDefaultAsync();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(OkResult));
                Assert.IsTrue(userExists);

                Assert.AreEqual(dbUser.username,user.username);
                Assert.AreEqual(dbUser.password, user.password);
                
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task testAddUserwithNoPassword()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                UserInfo user = new UserInfo()
                {
                    username = "Bob",
                    password = ""
                };
                //When
                UserInfoController userInfoController = new UserInfoController(context);
                IActionResult result = await userInfoController.RegisterUser(user);

                var userExists = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).AnyAsync();
                var dbUser = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).FirstOrDefaultAsync();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(UnprocessableEntityResult));
                Assert.IsTrue(!userExists);

                Assert.IsNull(dbUser);
                

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task testAuthenticateUser()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                UserInfo user = new UserInfo()
                {
                    Id = 1,
                    username = "Sparkstream",
                    password = "abcd1234"
                };
                //When
                UserInfoController userInfoController = new UserInfoController(context);
                IActionResult result = await userInfoController.AuthenticateUser(user);

                var userExists = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).AnyAsync();
                var dbUser = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).FirstOrDefaultAsync();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(OkResult));
                Assert.IsTrue(userExists);

                Assert.AreEqual(dbUser.username, user.username);
                Assert.AreEqual(dbUser.password, user.password);

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task testAuthenticateUserWrongPassword()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                UserInfo user = new UserInfo()
                {
                    username = "Sparkstream",
                    password = "1234"
                };
                //When
                UserInfoController userInfoController = new UserInfoController(context);
                IActionResult result = await userInfoController.AuthenticateUser(user);

                var userExists = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).AnyAsync();
                var dbUser = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).FirstOrDefaultAsync();

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
                Assert.IsTrue(userExists);

                Assert.AreEqual(dbUser.username, user.username);
                Assert.AreNotEqual(dbUser.password, user.password);

                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task testAuthenticateNonExistentUser()
        {
            using (var context = new LanguageAPIContext(options))
            {
                //Given
                UserInfo user = new UserInfo()
                {
                    username = "Maestro",
                    password = "abcd1234"
                };
                //When
                UserInfoController userInfoController = new UserInfoController(context);
                IActionResult result = await userInfoController.AuthenticateUser(user);

                var userExists = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).AnyAsync();
                var dbUser = await context.UserInfo.Where(u =>
                   u.username == user.username
                ).FirstOrDefaultAsync();

                Assert.IsNull(dbUser);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(BadRequestResult));
                Assert.IsTrue(!userExists);

                context.SaveChanges();
            }
        }
    }
}
