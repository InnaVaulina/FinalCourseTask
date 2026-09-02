using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace TTB.Services
{
    public class Logger
    {
        static string LogsDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "TTB", "Logs");
        public Logger(HttpRequestSender _requestSender)
        {
            _requestSender.OnError += Log;
        }

        public void Log(ClaimsIdentity? identity, Exception ex, string? url) 
        {
            try
            {
                if (!Directory.Exists(LogsDirectory))
                    Directory.CreateDirectory(LogsDirectory);
                var file = Path.Combine(LogsDirectory, $"{identity.Name}_{DateTime.Now:yyyy-MM-dd}.log");
                var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine} {url}{Environment.NewLine} {ex.Message}";
                File.AppendAllText(file, entry, Encoding.UTF8);
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }

    }
}
