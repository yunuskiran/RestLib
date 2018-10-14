using System;
using System.Collections.Generic;
using System.ComponentModel;
using RestHelper.Test.Models;
using Xunit;
using static RestHelper.UrlGenerator;
using static Xunit.Assert;
using static Xunit.Record;

namespace RestHelper.Test
{
    public class HelperTest
    {
        private const string UrlArgumentNullExceptionMessage = "Value cannot be null.\r\nParameter name: url";
        private const string UriFormatException = "Invalid Uri";
        private const string GithubUrl = "http://github.com";
        private const string GithubUrlWithRequest = "http://github.com?Id=1&Name=test";
        private const string GithubUrlWithNullParameterRequest = "http://github.com?Id=&Name=";
        private const string TodosUrl = "https://jsonplaceholder.typicode.com/todos";

        [Fact]
        [Category("Url")]
        public void Create_Url_With_UnValidateUrl_Return_UriFormatException()
        {
            var result = Exception(() => CreateUrl(string.Empty));
            NotNull(result);
            Equal(UriFormatException, result.Message);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_ValidateUrl_Return_Url()
        {
            var result = CreateUrl("http://github.com");
            NotNull(result);
            Equal(GithubUrl, result);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Request()
        {
            var result = CreateUrl(GithubUrl, new Request
            {
                Id = 1,
                Name = "test"
            });

            NotNull(result);
            Equal(result, GithubUrlWithRequest);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Null_Request()
        {
            var result = CreateUrl(GithubUrl, new Request(), true);

            NotNull(result);
            Equal(result, GithubUrl);
        }

        [Fact]
        [Category("Url")]
        public void Create_Url_With_Null_Request_And_NotIgnoreNullProperties()
        {
            var result = CreateUrl(GithubUrl, new Request(), false);

            NotNull(result);
            Equal(result, GithubUrlWithNullParameterRequest);
        }

        [Fact]
        [Category("Get")]
        public void Get_With_Null_Url_Throws_ArgumentNullException()
        {
            var helper = new Helper<Request, Response>();
            var result = Exception(() => helper.Get(string.Empty, new Request()));
            NotNull(result);
            Equal(UrlArgumentNullExceptionMessage, result.Message);
        }

        [Fact]
        [Category("Get")]
        public void GetAsync_With_Null_Url_Throws_ArgumentNullException()
        {
            var helper = new Helper<Request, Response>();
            var result = ExceptionAsync(() => helper.GetAsync(string.Empty, new Request()));
            NotNull(result);
            Equal(UrlArgumentNullExceptionMessage, result.Result.Message);
        }

        [Fact]
        [Category("Get")]
        public void Get_With_Valid_Parameters_ReturnResult()
        {
            var helper = new Helper<dynamic, List<Todo>>();
            var result = helper.Get(TodosUrl, null);
            NotNull(result);
        }


        [Fact]
        [Category("Get")]
        public void GetAsync_With_Valid_Parameters_ReturnResult()
        {
            var helper = new Helper<dynamic, List<Todo>>();
            var result = helper.GetAsync(TodosUrl, null).Result;
            NotNull(result);
        }

        [Fact]
        [Category("Post")]
        public void Post_With_Valid_Parameters_ReturnResult()
        {
            var request = new Todo
            {
                userId = 1,
                title = "new todo item",
                completed = false
            };

            var helper = new Helper<Todo, Todo>();
            var result = helper.Post(TodosUrl, request);
            NotNull(result);
            True(result.id > decimal.Zero);
        }

        [Fact]
        [Category("Post")]
        public void PostAsync_With_Valid_Parameters_ReturnResult()
        {
            var request = new Todo
            {
                userId = 1,
                title = "new todo item",
                completed = false
            };

            var helper = new Helper<Todo, Todo>();
            var result = helper.PostAsync(TodosUrl, request).Result;
            NotNull(result);
            True(result.id > decimal.Zero);
        }
    }
}