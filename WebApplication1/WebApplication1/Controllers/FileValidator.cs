using System.Text;

namespace WebApplication1.Controllers
{
    public static class FileValidator
    {
        // Максимум 5 МБ
        private const long MaxFileSize = 5 * 1024 * 1024;

        // Разрешённые MIME и расширения (пример)
        private static readonly string[] AllowedContentTypes = { "image/png", "image/jpeg", "image/gif", "image/webp", "image/svg+xml" };
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg", ".gif", ".webp", ".svg" };

        public static bool IsValidImage(IFormFile file)
        {
            if (file == null) return false;
            if (file.Length == 0 || file.Length > MaxFileSize) return false;

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext)) return false;

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                // не прерываем — всё ещё проверим сигнатуру
            }

            using (var stream = file.OpenReadStream())
            {
                // читаем первые 12 байт — достаточно для большинства сигнатур
                byte[] header = new byte[12];
                int read = stream.Read(header, 0, header.Length);

                if (IsPng(header, read)) return true;
                if (IsJpeg(header, read)) return true;
                if (IsGif(header, read)) return true;
                if (IsWebp(header, read)) return true;
                if (IsBmp(header, read)) return true;
                if (IsTiff(header, read)) return true;

                // SVG — текстовый формат, проверим первые символы
                if (IsSvg(stream, header, read)) return true;
            }

            return false;
        }

        private static bool IsPng(byte[] h, int r) =>
            r >= 8 && h[0] == 0x89 && h[1] == 0x50 && h[2] == 0x4E && h[3] == 0x47 &&
            h[4] == 0x0D && h[5] == 0x0A && h[6] == 0x1A && h[7] == 0x0A;

        private static bool IsJpeg(byte[] h, int r) =>
            r >= 3 && h[0] == 0xFF && h[1] == 0xD8 && h[2] == 0xFF;

        private static bool IsGif(byte[] h, int r) =>
            r >= 6 && h[0] == 0x47 && h[1] == 0x49 && h[2] == 0x46 && h[3] == 0x38 &&
            (h[4] == 0x37 || h[4] == 0x39) && h[5] == 0x61;

        private static bool IsWebp(byte[] h, int r) =>
            r >= 12 &&
            h[0] == 0x52 && h[1] == 0x49 && h[2] == 0x46 && h[3] == 0x46 && // "RIFF"
            h[8] == 0x57 && h[9] == 0x45 && h[10] == 0x42 && h[11] == 0x50; // "WEBP"

        private static bool IsBmp(byte[] h, int r) =>
            r >= 2 && h[0] == 0x42 && h[1] == 0x4D; // "BM"

        private static bool IsTiff(byte[] h, int r) =>
            r >= 4 && ((h[0] == 0x49 && h[1] == 0x49 && h[2] == 0x2A && h[3] == 0x00) ||
                       (h[0] == 0x4D && h[1] == 0x4D && h[2] == 0x00 && h[3] == 0x2A));

        private static bool IsSvg(Stream stream, byte[] header, int read)
        {
            // проверим, содержит ли начало '<' и слово "svg"
            var prefix = new byte[read];
            System.Array.Copy(header, prefix, read);
            string s = Encoding.UTF8.GetString(prefix).TrimStart();
            if (s.StartsWith("<") && s.IndexOf("svg", System.StringComparison.OrdinalIgnoreCase) >= 0) return true;

            // если не уверены, попробуем прочитать немного больше (без большого чтения)
            return false;
        }
    }
}
