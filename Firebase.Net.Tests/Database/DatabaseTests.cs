using Firebase.Net.Database;
using NUnit.Framework;
using System.Collections.Generic;
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
        public async Task SetUp()
        {
            FirebaseHelper = new FirebaseHelper();
            Database = new Database(DatabaseUrl, () => DatabaseSecret);
            await FirebaseHelper.UpdateRules("AllowEverything.json");
            await FirebaseHelper.ImportDatabase("People.json");
        }

        [TearDown]
        [OneTimeTearDown]
        public async Task TearDown()
        {
            await FirebaseHelper.CleanUp();
        }

        [Test]
        public async Task GetPeople_DataExists_ShouldRetreiveDictionary()
        {
            // Arrange
            var expectedPerson = new Person("Jennifer Cavalleri", 24, "Love means never having to say you're sorry");

            // Act
            var actual = await Database.Ref("People")
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.AreEqual(8, actual.Keys.Count);
            Assert.AreEqual(actual["jcavalleri"].ToJson(), expectedPerson.ToJson());
        }

        [Test]
        public async Task GetPersonOnce_ExistingPerson_ShouldRetreivePerson()
        {
            // Arrange
            var expected = new Person("Jennifer Cavalleri", 24, "Love means never having to say you're sorry");

            // Act
            var actual = await Database.Ref("People")
                                       .Child("jcavalleri")
                                       .Once<Person>();

            // Assert
            Assert.AreEqual(expected.ToJson(), actual.ToJson());
        }

        [Test]
        public async Task DeletePerson_ExistingPerson_ShouldDeletePerson()
        {
            // Arrange
            var expected = new Person("Jennifer Cavalleri", 24, "Love means never having to say you're sorry");

            // Act
            await Database.Ref("People")
                          .Child("jcavalleri")
                          .Remove();

            // Assert
            var actual = await Database.Ref("People")
                                     .Child("jcavalleri")
                                     .Once<Person>();

            Assert.IsNull(actual);
        }

        [Test]
        public async Task DeleteAge_ExistingPerson_ShouldDeletePerson()
        {
            // Arrange
            var expected = new Person("Jennifer Cavalleri", 24, "Love means never having to say you're sorry");

            // Act
            await Database.Ref("People")
                          .Child("jcavalleri")
                          .Remove();

            // Assert
            var actual = await Database.Ref("People")
                                     .Child("jcavalleri")
                                     .Child("Age")
                                     .Once<int>();

            Assert.AreEqual(default(int), actual);
        }

        [Test]
        public async Task SetAge_ExistingPerson_ShouldChangeAge()
        {
            // Arrange
            var expected = new Person("Jennifer Cavalleri", 24, "Love means never having to say you're sorry");

            // Act
            await Database.Ref("People")
                          .Child("jcavalleri")
                          .Child("Age")
                          .Set(123);

            // Assert
            var actual = await Database.Ref("People")
                                     .Child("jcavalleri")
                                     .Child("Age")
                                     .Once<int>();

            Assert.AreEqual(123, actual);
        }

        [Test]
        public async Task SetPerson_WithString_ShouldReplaceWholePath()
        {
            // Arrange
            string expected = "Some string.";

            // Act
            await Database.Ref("People")
                          .Child("jcavalleri")
                          .Set(expected);

            // Assert
            var actual = await Database.Ref("People")
                                     .Child("jcavalleri")
                                     .Once<string>();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Push_Nothing_ShouldCreatKeyInDatabase()
        {
            // Arrange
            var expected = new Person("Joey Tribbiani", 31, "How you doin'?");

            // Act
            var newRef = await Database.Ref("People")
                                       .Push();

            // Assert
            await newRef.Set(expected);

            var actual = await Database.Ref("People")
                                       .Child(newRef.Key)
                                       .Once<Person>();

            Assert.AreEqual(expected.ToJson(), actual.ToJson());
        }

        [Test]
        public async Task OrderByAgeLimitToFirst_People_ShouldReturnFirstTwoPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderBy("Age")
                                       .LimitToFirst(2)
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("jbond"));
            Assert.IsTrue(result.ContainsKey("wwhitman"));
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task OrderByAgeLimitToLast_People_ShouldReturnLastTwoPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderBy("Age")
                                       .LimitToLast(2)
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("dvader"));
            Assert.IsTrue(result.ContainsKey("et"));
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task OrderByAgeStartAt_People_ShouldReturnExpctedPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderBy("Age")
                                       .StartAt(100)
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("dvader"));
            Assert.IsTrue(result.ContainsKey("et"));
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task OrderByAgeEndAt_People_ShouldReturnExpectedPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderBy("Age")
                                       .EndAt(25)
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("jbond"));
            Assert.IsTrue(result.ContainsKey("wwhitman"));
            Assert.IsTrue(result.ContainsKey("jcavalleri"));
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task OrderByKeyEndAtJbon_People_ShouldReturnExpectedPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderByKey()
                                       .EndAt("jbond")
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("dvader"));
            Assert.IsTrue(result.ContainsKey("et"));
            Assert.IsTrue(result.ContainsKey("jbond"));
        }

        [Test]
        public async Task OrderByEqualTo_People_ShouldReturnExpectedPeople()
        {
            // Act
            var result = await Database.Ref("People")
                                       .OrderByKey()
                                       .EqualTo("jbond")
                                       .Once<Dictionary<string, Person>>();

            // Assert
            Assert.IsTrue(result.ContainsKey("jbond"));
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task Once_InvalidTokenType_ShouldThrowCouldNotParseAuthTokenException()
        {
            // Arrange
            Database UnauthenticatedDatabase = new Database(DatabaseUrl, () => "dummy_token");
            await FirebaseHelper.UpdateRules("DenyEverything.json");

            // Act & Assert
            Assert.That(async () => await UnauthenticatedDatabase.Ref("People").Once<Dictionary<string, Person>>(), Throws.Exception.InstanceOf<CouldNotParseAuthTokenException>());
        }

        [Test]
        public async Task Once_InvalidToken_ShouldThrowCouldNotParseAuthTokenException()
        {
            // Arrange
            Database UnauthenticatedDatabase = new Database(DatabaseUrl, () => "2K7qdbC2X9nQNRzlqQC14XmsGPR6Y5phUfxC2B5Z");
            await FirebaseHelper.UpdateRules("DenyEverything.json");

            // Act & Assert
            Assert.That(async () => await UnauthenticatedDatabase.Ref("People").Once<Dictionary<string, Person>>(), Throws.Exception.InstanceOf<CouldNotParseAuthTokenException>());
        }
    }
}
