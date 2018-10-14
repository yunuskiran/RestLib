using RestHelper.Test.Models;
using Xunit;
using static RestHelper.QueryStringGenerator;
using static Xunit.Assert;

namespace RestHelper.Test
{
    public class QueryStringGeneratorTest
    {
        [Fact]
        public void With_NullObject_ReturnEmptyString()
        {
            var queryString = Generate(null);
            Empty(queryString);
        }

        [Fact]
        public void With_NullObject_ReturnObjectNamesQueryString()
        {
            var request = new Request
            {
                Id = null
            };

            var queryString = Generate(request);
            Equal("Id=&Name=", queryString);
        }

        [Fact]
        public void WithIgnoreNullProperties_NullObject_ReturnEmptyString()
        {
            var request = new Request
            {
                Id = null
            };

            var queryString = Generate(request, true);
            Empty(queryString);
        }

        [Fact]
        public void With_FullObject_ReturnFullQueryString()
        {
            var request = new Request
            {
                Id = 1,
                Name = "test"
            };

            var queryString = Generate(request);
            Equal("Id=1&Name=test", queryString);
        }

        [Fact]
        public void With_FillSomeProperties_ReturnsOnlyFilledPropertiesQueryString() 
        {
            var request = new Request
            {
                Id = 2
            };

            var queryString = Generate(request, true);
            Equal("Id=2", queryString);
        }

        [Fact]
        public void With_FillSomeProperties_ReturnsWithNullabledPropertiesQueryString()
        {
            var request = new Request
            {
                Id = 2
            };

            var queryString = Generate(request);
            Equal("Id=2&Name=", queryString);
        }
    }
}
