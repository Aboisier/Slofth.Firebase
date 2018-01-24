using Firebase.Net.Utils;
using NUnit.Framework;
using System;

namespace Firebase.Net.Tests.Extensions
{
    [TestFixture]
    class UriBuilderExtensions
    {
        [Test]
        public void AddParam_StringParam_ShouldBuildExpectedUri()
        {
            // Arrange
            var baseUrl = "https://www.example.com";

            // Act
            var builder = UrlBuilder.Create(baseUrl).AddParam("my_param", "val");

            // Assert
            Assert.AreEqual("?my_param=val", builder.Query);
        }

        [Test]
        public void AddParam_IntParam_ShouldBuildExpectedUri()
        {
            // Arrange
            var baseUrl = "https://www.example.com";

            // Act
            var builder = UrlBuilder.Create(baseUrl).AddParam("my_param", 55);

            // Assert
            Assert.AreEqual("?my_param=55", builder.Query);
        }


        [Test]
        public void AddParam_ParamWithInvalidCharacters_ShouldEscapeKeyAndValue()
        {
            // Arrange
            var baseUrl = "https://www.example.com";

            // Act
            var builder = UrlBuilder.Create(baseUrl).AddParam("#myparam", "value$");

            // Assert
            Assert.AreEqual("?%23myparam=value%24", builder.Query);
        }

        [Test]
        public void AppendToPath_PathWithErraticSlashes_PathShouldBeSanitized()
        {
            // Arrange
            var baseUrl = "https://www.example.com";

            // Act
            var builder = UrlBuilder.Create(baseUrl)
                                    .AppendToPath("foo/")
                                    .AppendToPath("/bar///")
                                    .AppendToPath("lel");


            // Assert
            Assert.AreEqual("/foo/bar/lel", builder.Path);
        }
    }
}
