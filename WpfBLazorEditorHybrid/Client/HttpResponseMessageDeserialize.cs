using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Client
{

    public class HttpResponseMessageDeserialize<T> 
    {
        HttpRequestSender sender;
        public HttpResponseMessageDeserialize(HttpRequestSender _sender)
        {
            sender = _sender;
        }
        public async Task<T?> DeserealizeResultToContentAsync(HttpResponseMessage result)
        {
            if (result == null)
            {
                var userinfo = sender.GetUserInfo();
                throw new ScopedExeption(userinfo, $"DeserealizeResultToContentAsync<{typeof(T).Name}>", new ArgumentNullException(nameof(result)));
            }

            if (result.IsSuccessStatusCode == false)
            {
                var userinfo = sender.GetUserInfo();
                var err = result.GetErrorMessage();
                throw new ScopedExeption(userinfo, $"DeserealizeResultToContentAsync<{typeof(T).Name}>", new Exception(err));
            }

            if (result.Content == null) return default;
            await using var stream = await result.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<T>(stream, options);
        }

        public T DeserializeResponce(HttpResponseMessage response)
        {
            var task = DeserealizeResultToContentAsync(response);
            task.Wait();
            return task.Result!;
        }

    }
}
