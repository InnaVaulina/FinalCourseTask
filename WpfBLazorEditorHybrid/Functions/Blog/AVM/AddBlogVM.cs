using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public class AddBlogVM : INotifyPropertyChanged, IBlogEdit
    {
        protected AddNewBlogExampleDM newBlogDM;
        protected TabVM page;

        public event AddBlogHandler Notify_add;
        public event TabCloseHandler Notify_close_page;

        public AddBlogVM(AddNewBlogExampleDM _newBlogDM, TabVM _page) 
        {
            newBlogDM = _newBlogDM;
            page = _page;
            
            picture = new BitmapImage();
            imageFilePath = null;


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
                    newBlogDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    var response = await newBlogDM.CreateBlogContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<BlogContent>();
                    var newblog = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if(newblog != null) 
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_add?.Invoke(newblog);
                        Notify_close_page?.Invoke(page);
                    } 
                }
                catch (ScopedExeption ex)
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
            get { return newBlogDM.Content.PostDate; }
        }


        public string Title
        {
            get { return newBlogDM.Content.Title; }
            set { newBlogDM.Content.Title = value; OnPropertyChanged("Title"); }
        }


        public string Article 
        {
            get { return newBlogDM.Content.Article; }
            set { newBlogDM.Content.Article = value; }
        }


        public bool State
        {
            get
            {
                if (newBlogDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) newBlogDM.Content.Status = "InWork";
                else newBlogDM.Content.Status = "Published";
                OnPropertyChanged("State");
            }
        }

        BitmapImage picture;
        public BitmapImage Picture
        {
            get { return picture; }
            set { picture = value; OnPropertyChanged("Picture"); }
        }

        FileInfo imageFilePath;
        public FileInfo ImageFilePath 
        { 
            get { return imageFilePath; } 
            set { imageFilePath = value; }
        }

        public void SaveImageContent(FileInfo imageFilePath)
        {
            ImageFileModel fileInfo = new ImageFileModel() 
            {
                FileName = imageFilePath.Name,
                Content = File.ReadAllBytes(imageFilePath.FullName),
                ContentType = "application/octet-stream"
            };
            newBlogDM.SaveImageContent(fileInfo);
        }


        protected WCommand saveBlog;
        public WCommand SaveBlog { get { return saveBlog; } }

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
