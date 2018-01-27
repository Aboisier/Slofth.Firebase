using Firebase.Net.Utils;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace Firebase.Net.Tests
{
    [TestFixture]
    class HandledHttpClientTests
    {
        HandledHttpClient HandledClient { get; set; }
        Mock<HttpClient> ClientMock { get; set; }
        Mock<HttpResponseMessage> ResponseMock { get; set; }
        Mock<HttpContent> ContentMock { get; set; }
        Error Error { get; set; }

        [SetUp]
        public void SetUp()
        {
            ClientMock = new Mock<HttpClient>();
            ResponseMock = new Mock<HttpResponseMessage>();
            ContentMock = new Mock<HttpContent>();

            // TODO : We'll probably need to wrap the HttpClient in order to mock the ReadAsAsync method.
            ContentMock.Setup(x => x.ReadAsAsync<Error>()).Returns(() => Task.FromResult(Error)); 

            ResponseMock.Setup(x => x.IsSuccessStatusCode).Returns(() => false);
            ResponseMock.Setup(x => x.Content).Returns(() => ContentMock.Object);

            ClientMock.Setup(x => x.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<object>())).Returns(() => Task.FromResult(ResponseMock.Object));

            HandledClient = new HandledHttpClient(ClientMock.Object);
        }

        [Test]
        public async Task Post_InvalidRefreshTokenError_ShouldThrowInvalidRefreshTokenException()
        {
            await HandledClient.PostAsJsonAsync("http://www.example.com", "my data");
        }
    }
}
