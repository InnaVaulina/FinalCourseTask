using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public class EditingBlogVM : INotifyPropertyChanged, IBlogEdit
    {
        public event UpdateBlogHandler Notify_update;
        public event TabCloseHandler Notify_close_page;

        BlogExampleDM blogExampleDM;
        TabVM page;
        public EditingBlogVM(BlogExampleDM _blogExampleDM, TabVM _page)
        {
            blogExampleDM = _blogExampleDM;
            page = _page;

            string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
            string savePath = @$"{imageSavePath}{blogExampleDM.Content.IllustrationId}";
            imageFilePath = new FileInfo(savePath);


            if (!imageFilePath.Exists)
            {
                Picture = new BitmapImage();
            }
            else       
            {
                using (var fileStream = new FileStream(imageFilePath.FullName, FileMode.Open, FileAccess.Read))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = fileStream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    Picture = bitmapImage;
                }
            }
                

            saveBlog = new WCommand(async _ =>
            {
                if (Title == "")
                {
                    MessageBox.Show("Название статьи не заполнено!");
                    return;
                }
                if (Article == "")
                {
                    MessageBox.Show("Текст не заполнено!");
                    return;
                }

                try
                {
                    blogExampleDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    var response = await blogExampleDM.ChangeBlogContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<BlogContent>();
                    var newblog = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newblog != null) 
                    {
                        await blogExampleDM.UpdateDM(newblog);
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_update?.Invoke();
                        Notify_close_page?.Invoke(page);
                    }
                       
                }
                catch(ScopedExeption ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.ToString());
                    MessageBox.Show(ex.ToString());
                }
            });
        }

        public string? Date
        {
            get { return blogExampleDM.Content.PostDate; }
        }

        public string Title
        {
            get { return blogExampleDM.Content.Title; }
            set { blogExampleDM.Content.Title = value; OnPropertyChanged("Title"); }
        }

        public string Article
        {
            get { return blogExampleDM.Content.Article; }
            set { blogExampleDM.Content.Article = value; }
        }

        public bool State
        {
            get
            {
                if (blogExampleDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) blogExampleDM.Content.Status = "InWork";
                else blogExampleDM.Content.Status = "Published";
                OnPropertyChanged("State");
            }
        }

        FileInfo imageFilePath;
        public FileInfo ImageFilePath
        {
            get { return imageFilePath; }
            set { imageFilePath = value; }
        }

        BitmapImage picture;
        public BitmapImage Picture
        {
            get { return picture; }
            set { picture = value; OnPropertyChanged("Picture"); }
        }

        public void SaveImageContent(FileInfo imageFilePath)
        {
            ImageFileModel fileInfo = new ImageFileModel()
            {
                FileName = imageFilePath.Name,
                Content = File.ReadAllBytes(imageFilePath.FullName),
                ContentType = "application/octet-stream"
            };
            blogExampleDM.SaveImageContent(fileInfo);
        }

        protected WCommand saveBlog;
        public WCommand SaveBlog { get { return saveBlog; } }

        UC_BlogItem2 ucBlogItem;
        public UC_BlogItem2 UCBlogItem { set { ucBlogItem = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
