using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using TTClassLibrary.Functions.CollectionForm;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using System.Configuration;


namespace WpfBLazorHybridClient.Functions.Header.AVM
{
    public delegate void ChangeImageHandler(string url);
    public delegate Task ChangeModelHandler();

    public class EditCollectionFormVM : INotifyPropertyChanged
    {
        public event ChangeImageHandler ChangeImage;
        public event ChangeModelHandler ChangeModel;

        EditCollectionFormDM editCollectionFormDM;
        public EditCollectionFormDM EditCollectionFormDM { get { return editCollectionFormDM; } }

        public EditCollectionFormVM(EditCollectionFormDM _editCollectionFormDM)
        {
            editCollectionFormDM = _editCollectionFormDM;
            
            saveForm = new WCommand(async _ =>
            {
                try
                {
                    editCollectionFormDM.HtmlPattern = RemoveExcessive(editCollectionFormDM.HtmlPattern);
                    var result = await editCollectionFormDM.SaveCollectionFormContentAsync();
                    ChangeModel?.Invoke();
                    MessageBox.Show("Запрос выполнен успешно");
                }
                catch (ScopedExeption ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }

            });
        }

        string? objectUrl;

        public string? ImageUrl
        {
            get 
            {
                string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
                objectUrl = ConvertFilePathToUrl(@$"{imageSavePath}{editCollectionFormDM.ImagePath}");
                return objectUrl; 
            }
        }

        public void SaveImageContent(FileInfo fileInfo) 
        {
            ImageFileModel imageinfo = new ImageFileModel
            {
                FileName = fileInfo.Name,
                Content = File.ReadAllBytes(fileInfo.FullName),
                ContentType = System.IO.Path.GetExtension(fileInfo.FullName).ToLowerInvariant()
            };
            editCollectionFormDM.SaveImageContent(imageinfo);
            objectUrl = ConvertFilePathToUrl(Path.GetFullPath(fileInfo.FullName));
            ChangeImage?.Invoke(objectUrl);
        }

        

        /// <summary>
        /// Преобразует полный путь файла в URL, пригодный для браузера:
        /// - если файл в wwwroot => "/..."
        /// - если небольшой (< 3MB) => data:URI
        /// - иначе => file:/// URI (запасной вариант)
        /// </summary>
        private string ConvertFilePathToUrl(string full)
        {
            //var full = Path.GetFullPath(fileInfo.FullName);

            // Попытка найти wwwroot в пути и вернуть относительный URL
            var parts = full.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            var idx = Array.FindIndex(parts, p => p.Equals("wwwroot", StringComparison.OrdinalIgnoreCase));
            if (idx >= 0 && idx < parts.Length - 1)
            {
                var rel = string.Join("/", parts.Skip(idx + 1));
                return "/" + rel;
            }

            if (!File.Exists(full))
            {
                // если файла нет — вернуть исходный путь как запасной вариант
                return full;
            }

            try
            {
                var bytes = File.ReadAllBytes(full);

                // порог для data:URI (в байтах)
                const int MaxDataUriBytes = 3 * 1024 * 1024; // 3 MB

                if (bytes.Length <= MaxDataUriBytes)
                {
                    var ext = Path.GetExtension(full).ToLowerInvariant();
                    var mime = ext switch
                    {
                        ".webp" => "image/webp",
                        ".png" => "image/png",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".svg" => "image/svg+xml",
                        ".gif" => "image/gif",
                        _ => "application/octet-stream"
                    };
                    var b64 = Convert.ToBase64String(bytes);
                    return $"data:{mime};base64,{b64}";
                }

                // Для больших файлов возвращаем file:/// URI (в WebView может работать или нет в зависимости от настроек)
                return new Uri(full).AbsoluteUri;
            }
            catch
            {
                // В случае ошибки — fallback
                return full;
            }
        }

        private static string RemoveExcessive(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Удаляет атрибуты:
            // - contenteditable
            // - contenteditable="true"
            // - contenteditable='true'
            // - contenteditable=true
            // НЕ трогает contenteditable="false"
            var pattern = @"\s*contenteditable(?:\s*=\s*(?:'true'|""true""|true))?";
            var result = Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);

            var blPattern = @"\s*_bl_[0-9A-Za-z\-]+(?:\s*=\s*(?:'[^']*'|""[^""]*""|[^\s>]+))?";
            result = Regex.Replace(result, blPattern, string.Empty, RegexOptions.IgnoreCase);

            var commentPattern = @"<!--[\s\S]*?-->";
            result = Regex.Replace(result, commentPattern, string.Empty, RegexOptions.Multiline);

            return result;
        }


        protected WCommand saveForm;
        public WCommand SaveForm { get { return saveForm; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
