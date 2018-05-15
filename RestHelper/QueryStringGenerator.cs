using System.Text;

namespace RestHelper
{
    public static class QueryStringGenerator
    {
        public static string Generate(object obj, bool ignoreNullProperties = false)
        {
            var stringBuilder = new StringBuilder();
            if (obj == null) return string.Empty;

            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
                if (obj.GetType().GetProperty(property.Name)?.GetValue(obj) != null)
                    stringBuilder.Append($"{property.Name}={obj.GetType().GetProperty(property.Name)?.GetValue(obj)}&");
                else
                    stringBuilder.Append(ignoreNullProperties ? string.Empty : $"{property.Name}=&");

            var queryString = stringBuilder.Length > default(int)
                ? stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString()
                : string.Empty;
            return queryString;
        }
    }
}