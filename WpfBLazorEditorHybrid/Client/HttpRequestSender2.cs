using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.IServices;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Client
{
    public class HttpRequestSender2 : IHttpRequestSender
    {

        protected string baseAddress = ConfigurationManager.AppSettings["ApiUrl"];
        protected string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
        User user;
        HttpClient client;


        public HttpRequestSender2(User _user)
        {
            user = _user;
            client = new HttpClient();
        }

        async Task ResultError(HttpResponseMessage result, string url)
        {
            Exception newEx = null;
            if (!result.IsSuccessStatusCode)
            {
                var details = await result.Content.ReadAsStringAsync();
                if (result.ReasonPhrase == "Unauthorized")
                {

                    newEx = new Exception($"Request failed with status code: {result.StatusCode}, reason: {details}. Token may be invalid or expired.");
                }
                else
                {
                    
                    newEx = new Exception($"Request failed with status code: {result.StatusCode}, reason: {details}");
                }
            }
            if (newEx != null)
            {
                throw newEx;
            }
        }


        public async Task<HttpResponseMessage> Get(string url)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = await client.SendAsync(request);
                await ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }

        }

        public async Task<HttpResponseMessage> Get(Uri geturl)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, geturl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = await client.SendAsync(request);
                await ResultError(result, geturl.ToString());
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, geturl.ToString(), ex);
            }
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = await client.SendAsync(request);
                await ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, JsonContent content)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = content;
                var result = await client.SendAsync(request);

                await ResultError(result, url);
                return result;

            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, MultipartFormDataContent formData)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = formData;
                var result = await client.SendAsync(request);

                await ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }
        }

        public async Task<HttpResponseMessage> Put(string url, JsonContent content)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = content;
                var result = await client.SendAsync(request);

                await ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }
        }

        public async Task<HttpResponseMessage> Put(string url, MultipartFormDataContent formData)
        {
            try
            {
                url = $"{baseAddress}/" + url;
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = formData;
                var result = await client.SendAsync(request);

                await ResultError(result, url);
                return result;
            }
            catch (Exception ex)
            {
                throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
            }
        }

        


        public async Task<FileInfo?> GetImage(string fileName)
        {
            var url = $"{baseAddress.TrimEnd('/')}/IMG/{fileName}";
            string savePath = @$"{imageSavePath}{fileName}";
            FileInfo fileInfo = new FileInfo(savePath);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            byte[] imageBytes;
            using (var client = new HttpClient())
            {
                try
                {
                    imageBytes = await client.GetByteArrayAsync(url);
                    if (imageBytes == null || imageBytes.Length == 0)
                        throw new Exception("Файл изображения не загружен");
                    await File.WriteAllBytesAsync(savePath, imageBytes);
                    if (!fileInfo.Exists)
                        throw new Exception("Файл изображения не сохранен");
                }
                catch (Exception ex)
                {
                    throw new ScopedExeption(new UserInfo { UserName = user.UserName, Id = user.Id }, url, ex);
                }
            }
            return fileInfo;
        }


    }
}
