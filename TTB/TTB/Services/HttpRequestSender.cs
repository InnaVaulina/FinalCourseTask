using Microsoft.AspNetCore.StaticFiles;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Security.Claims;
using TTB.Support;
using TTClassLibrary.IServices;
using TTClassLibrary.Support;



namespace TTB.Services
{
    public delegate void ErrorHandler(ClaimsIdentity? identity, Exception ex, string? url);
    public class HttpRequestSender: IHttpRequestSender
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private const string ClientName = "ApiClient";
        private TokenProcessing tokenProcessing;
        private string? token;
        private ClaimsIdentity? identity;
        private string imageSavePath;

        public event ErrorHandler? OnError;

        private void OnTokenChanged(string? _token)
        {
            token = _token ?? string.Empty;
            if(!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(JwtHelpers.ParseClaimsFromJwt(token), "jwt");
            }
            else
            {
                identity = null;
            }
        }


        public HttpRequestSender(IHttpClientFactory httpClientFactory, TokenProcessing _tokenProcessing)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            tokenProcessing = _tokenProcessing ?? throw new ArgumentNullException(nameof(_tokenProcessing));
            tokenProcessing.TokenChanged += OnTokenChanged;
            token = null;
            identity = null;
            imageSavePath = GetPath.GetWwwRootImgPath();
        }

        void ResultError(HttpResponseMessage result, string url)
        {
            Exception newEx = null;
            if (!result.IsSuccessStatusCode)
            {
                if (result.ReasonPhrase == "Unauthorized")
                {
                    tokenProcessing.RemoveToken();
                    newEx = new Exception($"Request failed with status code: {result.StatusCode}, reason: {result.ReasonPhrase}. Token may be invalid or expired.");
                }
                else
                {
                    newEx = new Exception($"Request failed with status code: {result.StatusCode}, reason: {result.ReasonPhrase}");
                }
            }
            if (newEx != null)
            {
                OnError?.Invoke(identity, newEx, url);
                throw newEx;
            }
        }
        


        public async Task<HttpResponseMessage> Get(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }




        public async Task<HttpResponseMessage> Get(Uri geturl)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Get, geturl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.SendAsync(request);
                ResultError(result, geturl.ToString());
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, geturl.ToString());
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, JsonContent content)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;

            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, MultipartFormDataContent formData)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = formData;
                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<HttpResponseMessage> Put(string url, JsonContent content)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = content;
                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<HttpResponseMessage> Put(string url, MultipartFormDataContent formData)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                request.Content = formData;
                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(ClientName);

                using var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var result = await client.SendAsync(request);
                ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, url);
                throw new Exception(ex.Message, ex);
            }
        }


        

        public async Task<FileInfo?> GetImage(string fileName)
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            Uri requestUri;

            string savePath = Path.Combine(imageSavePath ?? string.Empty, fileName);
            var dir = Path.GetDirectoryName(savePath);
            if (string.IsNullOrEmpty(dir))
                dir = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(dir);

            FileInfo fileInfo = new FileInfo(savePath);
            byte[] imageBytes;

            try 
            {
                requestUri = new Uri(client.BaseAddress, $"IMG/{fileName}");
                imageBytes = await client.GetByteArrayAsync(requestUri);
                await File.WriteAllBytesAsync(savePath, imageBytes);
                return fileInfo;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(identity, ex, $"IMG/{fileName}");
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
