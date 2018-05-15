using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static System.Text.Encoding;

namespace RestHelper
{
    public class Helper<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly TimeSpan _timeOut;
        private readonly Encoding _encoding;
        private readonly string _mediaType;
        private const string EmptyContent = "[]";
        private const string Gzip = "gzip";
        private const string BaseExceptionString = "Error Occured!";
        private const string ContentNullExceptionString = "Content is null";

        public Helper()
            : this(TimeSpan.FromSeconds(60))
        {
        }

        public Helper(TimeSpan timeOut)
            : this(timeOut, UTF8)
        {
        }

        public Helper(Encoding encoding)
            : this(TimeSpan.FromSeconds(60), encoding)
        {
        }

        public Helper(string mediaType)
            : this(TimeSpan.FromSeconds(60), mediaType)
        {
        }

        public void GetAsync()
        {
            throw new NotImplementedException();
        }

        public Helper(TimeSpan timeOut, Encoding encoding)
            : this(timeOut, encoding, "application/json")
        {
        }

        public Helper(TimeSpan timeOut, string mediaType)
            : this(timeOut, UTF8, mediaType)
        {
        }

        public Helper(Encoding encoding, string mediaType)
            : this(TimeSpan.FromSeconds(60), encoding, mediaType)
        {
        }

        public Helper(TimeSpan timeOut, Encoding encoding, string mediaType)
        {
            _timeOut = timeOut;
            _encoding = encoding;
            _mediaType = mediaType;
        }

        public TResponse Post(string url, TRequest request)
        {
            TResponse result = null;
            try
            {
                result = PostAsync(url, request).Result;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null) throw exception.InnerException;
            }

            return result;
        }

        public TResponse Get(string url, TRequest request)
        {
            TResponse result = null;
            try
            {
                result = GetAsync(url, request).Result;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null) throw exception.InnerException;
            }

            return result;
        }

        public async Task<TResponse> PostAsync(string url, TRequest request)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            var result = Activator.CreateInstance<TResponse>();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(60);
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.PostAsync(url,
                    new StringContent(JsonConvert.SerializeObject(request), _encoding, _mediaType));
                if (response.Content.Headers.ContentEncoding.Contains(Gzip))
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    if (!response.IsSuccessStatusCode) throw new Exception(BaseExceptionString);

                    if (content == null) throw new Exception(ContentNullExceptionString);

                    using (var inputStream = new MemoryStream(content))
                    using (var gzip = new GZipStream(inputStream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(gzip))
                    {
                        var readedContent = await reader.ReadToEndAsync();
                        result = JsonConvert.DeserializeObject<TResponse>(readedContent);
                    }
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode) throw new Exception(content);

                    if (string.IsNullOrWhiteSpace(content) && content != EmptyContent)
                        result = JsonConvert.DeserializeObject<TResponse>(content);
                }
            }

            return result;
        }

        public async Task<TResponse> GetAsync(string url, TRequest request)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            var result = Activator.CreateInstance<TResponse>();
            using (var client = new HttpClient())
            {
                client.Timeout = _timeOut;
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response =
                    await client.GetAsync(UrlGenerator.CreateUrl(url, request, true));
                if (response.Content.Headers.ContentEncoding.Contains(Gzip))
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    if (!response.IsSuccessStatusCode) throw new Exception(BaseExceptionString);

                    if (content == null) throw new Exception(ContentNullExceptionString);

                    using (var inputStream = new MemoryStream(content))
                    using (var gzip = new GZipStream(inputStream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(gzip))
                    {
                        result = JsonConvert.DeserializeObject<TResponse>(await reader.ReadToEndAsync());
                    }
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode) throw new Exception(content);

                    if (string.IsNullOrWhiteSpace(content) && content != EmptyContent)
                        result = JsonConvert.DeserializeObject<TResponse>(content);
                }
            }

            return result;
        }
    }
}