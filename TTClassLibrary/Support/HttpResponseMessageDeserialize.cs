using System.Text.Json;

namespace TTClassLibrary.Support
{

    public class HttpResponseMessageDeserialize<T> 
    {
        public HttpResponseMessageDeserialize()
        {
        }
        public async Task<T?> DeserealizeResultToContentAsync(HttpResponseMessage result)
        {
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
