namespace TTB.Support
{
    public static class GetPath
    {

        public static string GetWwwRootImgPath()
        {
            // Ищем вверх от текущего каталога выполнение приложения wwwroot папку
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                var wwwroot = Path.Combine(dir.FullName, "wwwroot");
                if (Directory.Exists(wwwroot))
                {
                    var img = Path.Combine(wwwroot, "img");
                    if (!Directory.Exists(img))
                    {
                        try
                        {
                            Directory.CreateDirectory(img);
                        }
                        catch
                        {
                            // игнорируем ошибки создания, дальше будет fallback
                        }
                    }
                    return img + Path.DirectorySeparatorChar;
                }
                dir = dir.Parent;
            }

            // Fallback — создаём папку внутри каталога приложения
            var fallback = Path.Combine(AppContext.BaseDirectory, "wwwroot", "img");
            try
            {
                if (!Directory.Exists(fallback))
                    Directory.CreateDirectory(fallback);
            }
            catch
            {
                // ignored
            }
            return fallback + Path.DirectorySeparatorChar;
        }
    }
}
