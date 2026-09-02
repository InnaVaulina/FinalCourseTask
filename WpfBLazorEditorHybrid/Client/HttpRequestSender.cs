using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TTClassLibrary.IServices;
using WpfBLazorHybridClient.Client.Work;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Client
{
    public class HttpRequestSender: IHttpRequestSender
    {
        protected string baseAddress = ConfigurationManager.AppSettings["ApiUrl"];
        User user;
        HttpClient client;

        protected string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
        public HttpRequestSender(User _user)
        {
            user = _user;
            client = new HttpClient();
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                    var result = await client.SendAsync(request);

                    return result;
                }
                catch (Exception ex)
                {
                    var userinfo = GetUserInfo();
                    throw new ScopedExeption(userinfo, url, ex);
                }
            
        }

        public async Task<HttpResponseMessage> Get(Uri geturl)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, geturl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = await client.SendAsync(request);

                return result;
            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, geturl.AbsolutePath, ex);
            }
        }

        public async Task<HttpResponseMessage> Delete(string url)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);

                var result = await client.SendAsync(request);

                return result;
            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, url, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, JsonContent content)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = content;
                var result = await client.SendAsync(request);

                return result;

            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, url, ex);
            }
        }


        public async Task<HttpResponseMessage> Post(string url, MultipartFormDataContent formData)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = formData;
                var result = await client.SendAsync(request);

                return result;
            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, url, ex);
            }
        }

        public async Task<HttpResponseMessage> Put(string url, JsonContent content)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = content;
                var result = await client.SendAsync(request);

                return result;
            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, url, ex);
            }
        }

        public async Task<HttpResponseMessage> Put(string url, MultipartFormDataContent formData)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
                request.Content = formData;
                var result = await client.SendAsync(request);

                return result;
            }
            catch (Exception ex)
            {
                var userinfo = GetUserInfo();
                throw new ScopedExeption(userinfo, url, ex);
            }
        }

        public UserInfo GetUserInfo()
        {
            return new UserInfo() { UserName = user.UserName };
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
                    if(imageBytes == null || imageBytes.Length == 0)
                        throw new Exception("Файл изображения не загружен");
                    await File.WriteAllBytesAsync(savePath, imageBytes);
                    if (!fileInfo.Exists)
                        throw new Exception("Файл изображения не сохранен");
                }
                catch (Exception ex)
                {
                    var userinfo = GetUserInfo();
                    throw new ScopedExeption(userinfo, $"GetImage {fileName}", ex);
                }
            }
            return fileInfo;
        }

        public ByteArrayContent MakeImageContent(FileInfo? fileInfo)
        {
            if (fileInfo != null && File.Exists(fileInfo.FullName))
            {
                var fileBytes = File.ReadAllBytes(fileInfo.FullName);

                var ext = System.IO.Path.GetExtension(fileInfo.FullName).ToLowerInvariant();
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(fileInfo.FullName, out var mime))
                {
                    mime = "application/octet-stream";
                }
                var content = new ByteArrayContent(fileBytes, 0, fileBytes.Length);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mime);
                return content;
            }
            return null;
        }
    }
}
