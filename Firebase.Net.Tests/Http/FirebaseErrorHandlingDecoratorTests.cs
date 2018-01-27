using Firebase.Net.Http;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Tests.Http
{
    [TestFixture]
    class FirebaseErrorHandlingDecoratorTests
    {
        Mock<IHttpClientFacade> BaseClientMock { get; set; }
        Mock<HttpResponseMessage> ResponseMock { get; set; }
        Mock<HttpContent> ContentMock { get; set; }
        IHttpClientFacade ClientDecorator { get; set; }

        Error Error { get; set; }
        bool IsSuccessStatusCode { get; set; }

        [SetUp]
        public void SetUp()
        {
            BaseClientMock = new Mock<IHttpClientFacade>();
            ResponseMock = new Mock<HttpResponseMessage>();
            ContentMock = new Mock<HttpContent>();
            ClientDecorator = new FirebaseErrorHandlingDecorator(BaseClientMock.Object);

            ContentMock.Setup(x => x.ReadAsAsync<Error>()).Returns(() => Task.FromResult(Error));

            ResponseMock.Setup(x => x.IsSuccessStatusCode).Returns(() => IsSuccessStatusCode);
            ResponseMock.Setup(x => x.Content).Returns(() => ContentMock.Object);

            BaseClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(() => Task.FromResult(ResponseMock.Object));
            BaseClientMock.Setup(x => x.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(() => Task.FromResult(ResponseMock.Object));
            BaseClientMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(() => Task.FromResult(ResponseMock.Object));
            BaseClientMock.Setup(x => x.DeleteAsync(It.IsAny<string>())).Returns(() => Task.FromResult(ResponseMock.Object));
        }

        [Test]
        public async Task Test()
        {
            // Arrange


            // Act
            await ClientDecorator.PostAsJsonAsync(Constants.Url, "my data");

            // Assert
        }

        class Constants
        {
            public static readonly string Url = "http://www.example.com/test";
        }
    }
}
