using DAL;
using DAL.Models;
using DAL.Repository;
using IngTransactions.Util;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;

namespace TestProject
{
    [TestFixture]
    public class Tests
    {
        private SqliteDbContext sqliteDbContext;
        private AccountRepository accountRepository;
        IConfiguration config;


        [OneTimeSetUp]
        public void Setup()
        {
            var appConfig = new AppConfiguration();
            var appconfig = appConfig.GetApplicationConfiguration();
            config = appConfig.configuration;
            var str = appconfig.GetPath();
            sqliteDbContext = new SqliteDbContext();
            accountRepository = new AccountRepository(sqliteDbContext);
            sqliteDbContext.Account.Add(new Account() { Currency = "Ron", Iban = "192929121", Name = "Test", Product = "test" });
            sqliteDbContext.SaveChanges();
        }

        [Test]
        public void CheckAddAcount()
        {
            var sut = accountRepository.GetAccounts();
            Assert.That(sut.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void CheckCerts()
        {
            var cert = new X509Certificate2(
                config.GetValue<string>("Certificates:signing:Path"),
                config.GetValue<string>("Certificates:signing:Password"));
        }
    }
}
