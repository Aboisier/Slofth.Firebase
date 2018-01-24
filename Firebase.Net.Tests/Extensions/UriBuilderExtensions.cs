using NUnit.Framework;
using System;
using Firebase.Net.Extensions;

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
            var builder = new UriBuilder(baseUrl);

            // Act
            builder.AddParam("my_param", "val");

            // Assert
            Assert.AreEqual("?my_param=val", builder.Query);
        }

        [Test]
        public void AddParam_IntParam_ShouldBuildExpectedUri()
        {
            // Arrange
            var baseUrl = "https://www.example.com";
            var builder = new UriBuilder(baseUrl);

            // Act
            builder.AddParam("my_param", 55);

            // Assert
            Assert.AreEqual("?my_param=55", builder.Query);
        }


        [Test]
        public void AddParam_ParamWith_ShouldEscapeKeyAndValue()
        {
            // Arrange
            var baseUrl = "https://www.example.com";
            var builder = new UriBuilder(baseUrl);

            // Act
            builder.AddParam("#myparam", "value$");

            // Assert
            Assert.AreEqual("?%23myparam=value%24", builder.Query);
        }
    }
}
