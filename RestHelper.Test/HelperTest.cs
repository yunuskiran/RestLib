using System;
using System.ComponentModel;
using RestHelper.Test.Models;
using Xunit;

namespace RestHelper.Test
{
    public class HelperTest
    {
        private const string UrlArgumentNullExceptionMessage = "Value cannot be null.\r\nParameter name: url";
        private const string UriFormatException = "Invalid Uri";
        private const string GithubUrl = "http://github.com";
        private const string GithubUrlWithRequest = "http://github.com?Id=1&Name=test";
        private const string GithubUrlWithNullParameterRequest = "http://github.com?Id=&Name=";

        [Fact]
        [Category("Get")]
        public void Get_With_Null_Url_Throws_ArgumentNullException()
        {
            var helper = new Helper<Request, Response>();
            var result = Record.Exception(() => helper.Get(string.Empty, new Request()));
            Assert.NotNull(result);
            Assert.Equal(UrlArgumentNullExceptionMessage, result.Message);
        }

        [Fact]
        [Category("Get")]
        public void GetAsync_With_Null_Url_Throws_ArgumentNullException()
        {
            var helper = new Helper<Request, Response>();
            var result = Record.ExceptionAsync(() => helper.GetAsync(string.Empty, new Request()));
            Assert.NotNull(result);
            Assert.Equal(UrlArgumentNullExceptionMessage, result.Result.Message);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_UnValidateUrl_Return_UriFormatException()
        {
            var result = Record.Exception(() => UrlGenerator.CreateUrl(string.Empty));
            Assert.NotNull(result);
            Assert.Equal(UriFormatException, result.Message);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_ValidateUrl_Return_Url()
        {
            var result = UrlGenerator.CreateUrl("http://github.com");
            Assert.NotNull(result);
            Assert.Equal(GithubUrl, result);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Request()
        {
            var result = UrlGenerator.CreateUrl(GithubUrl, new Request
            {
                Id = 1,
                Name = "test"
            });

            Assert.NotNull(result);
            Assert.Equal(result, GithubUrlWithRequest);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Null_Request()
        {
            var result = UrlGenerator.CreateUrl(GithubUrl, new Request(), true);

            Assert.NotNull(result);
            Assert.Equal(result, GithubUrl);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Null_Request_And_NotIgnoreNullProperties()
        {
            var result = UrlGenerator.CreateUrl(GithubUrl, new Request(), false);

            Assert.NotNull(result);
            Assert.Equal(result, GithubUrlWithNullParameterRequest);
        }
    }
}