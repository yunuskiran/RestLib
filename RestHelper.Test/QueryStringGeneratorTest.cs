using RestHelper.Test.Models;
using Xunit;

namespace RestHelper.Test
{
    public class QueryStringGeneratorTest
    {
        [Fact]
        public void With_NullObject_ReturnEmptyString()
        {
            var queryString = QueryStringGenerator.Generate(null);
            Assert.Empty(queryString);
        }

        [Fact]
        public void With_NullObject_ReturnObjectNamesQueryString()
        {
            Request request = new Request
            {
                Id = null
            };

            var queryString = QueryStringGenerator.Generate(request);
            Assert.Equal("Id=&Name=", queryString);
        }

        [Fact]
        public void WithIgnoreNullProperties_NullObject_ReturnEmptyString()
        {
            Request request = new Request
            {
                Id = null
            };

            var queryString = QueryStringGenerator.Generate(request, true);
            Assert.Empty(queryString);
        }

        [Fact]
        public void With_FullObject_ReturnFullQueryString()
        {
            Request request = new Request
            {
                Id = 1,
                Name = "test"
            };

            var queryString = QueryStringGenerator.Generate(request);
            Assert.Equal("Id=1&Name=test", queryString);
        }

        [Fact]
        public void With_FillSomeProperties_ReturnsOnlyFilledPropertiesQueryString() 
        {
            Request request = new Request
            {
                Id = 2
            };

            var queryString = QueryStringGenerator.Generate(request, true);
            Assert.Equal("Id=2", queryString);
        }

        [Fact]
        public void With_FillSomeProperties_ReturnsWithNullabledPropertiesQueryString()
        {
            Request request = new Request
            {
                Id = 2
            };

            var queryString = QueryStringGenerator.Generate(request);
            Assert.Equal("Id=2&Name=", queryString);
        }
    }
}
