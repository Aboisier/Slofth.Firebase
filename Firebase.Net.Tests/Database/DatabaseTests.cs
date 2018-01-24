using Firebase.Net.Database;
using NUnit.Framework;
using System.Configuration;
using System.Threading.Tasks;

namespace PolyPaint.Tests.Services
{
    [TestFixture]
    class FirebaseDatabaseServiceTests
    {
        private FirebaseHelper FirebaseHelper { get; set; }
        private Database Database { get; set; }

        private string DatabaseUrl => ConfigurationManager.AppSettings.Get("DatabaseUrl");
        private string DatabaseSecret => ConfigurationManager.AppSettings.Get("DatabaseSecret");


        [SetUp]
        public void SetUp()
        {
            FirebaseHelper = new FirebaseHelper();
            Database = new Database(DatabaseUrl, () => DatabaseSecret);
        }

        [TearDown]
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await FirebaseHelper.CleanUp();
        }

        [Test]
        public async Task Testy()
        {
            // Arrange
            await FirebaseHelper.UpdateRules("AllowEverything.json");

            // Act
            //Database.Ref("lel").Once();
        }
    }

}
