using LoginExample.WebApp.Mapper;
using LoginExample.WebApp.Models;
using LoginExampleWebApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoginExample.Tests
{
    [TestClass]
    public class UnitTests
    {
        private WebApp.Store.UserStore _userStore;
        private string databaseConnectionString = "Data Source=blogging.db";

        public UnitTests()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            //dbContextOptionsBuilder.UseSqlite(databaseConnectionString);
            dbContextOptionsBuilder.UseInMemoryDatabase("testDb");

            //Should have the components registered in IOC
            _userStore = new WebApp.Store.UserStore(
                new DatabaseContext(dbContextOptionsBuilder.Options)
                , new UserMapper());
        }

        [TestMethod]
        public void TestRegisterAndLogin()
        {
            string username = "username";
            string password = "password";
            var userModel = new UserModel { Username = username, Password = password };
            _userStore.addUser(userModel);
            var user = _userStore.loginUser(username, password);
            _userStore.removeUser(user.Id);
            Assert.IsNotNull(user);
        }
    }
}
