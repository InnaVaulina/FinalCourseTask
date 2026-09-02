using System;
using System.IO;
using System.Text;

namespace WpfBLazorHybridClient.Error
{
    public static class Logger
    {
        static string LogsDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "WpfBLazorHybridClient", "Logs");

        public static void Log(string text)
        {
            try
            {
                Directory.CreateDirectory(LogsDirectory);
                var file = Path.Combine(LogsDirectory, $"{DateTime.Now:yyyy-MM-dd}.log");
                var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {text}{Environment.NewLine}";
                File.AppendAllText(file, entry, Encoding.UTF8);
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }
    }
}
