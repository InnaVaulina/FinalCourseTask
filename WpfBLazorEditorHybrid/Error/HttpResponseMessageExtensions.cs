using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.Error
{

    public static class HttpResponseMessageExtensions
    {
        public static async Task<string?> GetErrorMessageAsync(this HttpResponseMessage? response)
        {
            if (response == null)
                return "Нет ответа от сервера";

            if (response.IsSuccessStatusCode)
                return null;

            string body;
            try
            {
                body = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync();
            }
            catch
            {
                body = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(body))
                return $"{(int)response.StatusCode} {response.ReasonPhrase}";

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;

                if (root.TryGetProperty("message", out var m) || root.TryGetProperty("Message", out m))
                    return $"{(int)response.StatusCode} {response.ReasonPhrase}: {m.GetString()}";

                if (root.TryGetProperty("detail", out var d))
                    return $"{(int)response.StatusCode} {response.ReasonPhrase}: {d.GetString()}";

                if (root.TryGetProperty("title", out var t))
                    return $"{(int)response.StatusCode} {response.ReasonPhrase}: {t.GetString()}";

                if (root.TryGetProperty("errors", out var errors))
                {
                    if (errors.ValueKind == JsonValueKind.Object)
                    {
                        var sb = new StringBuilder();
                        foreach (var prop in errors.EnumerateObject())
                        {
                            if (prop.Value.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var item in prop.Value.EnumerateArray())
                                    sb.AppendLine(item.GetString());
                            }
                            else
                            {
                                sb.AppendLine(prop.Value.ToString());
                            }
                        }
                        var msg = sb.ToString().Trim();
                        if (!string.IsNullOrEmpty(msg))
                            return $"{(int)response.StatusCode} {response.ReasonPhrase}: {msg}";
                    }
                }

                // fallback: вернуть тело как есть
                return $"{(int)response.StatusCode} {response.ReasonPhrase}: {body}";
            }
            catch
            {
                // тело не JSON — вернуть как есть
                return $"{(int)response.StatusCode} {response.ReasonPhrase}: {body}";
            }
        }

        // Синхронная оболочка (в проекте уже используются sync-вызовы через Task.Run)
        public static string? GetErrorMessage(this HttpResponseMessage? response)
            => Task.Run(() => response.GetErrorMessageAsync()).GetAwaiter().GetResult();
    }
}
