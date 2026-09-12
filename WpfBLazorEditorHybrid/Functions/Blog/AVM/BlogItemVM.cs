using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using TTClassLibrary.Functions.Blog;
using WpfBLazorHybridClient.Main.AVM.Tab;
using System.Configuration;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Blog.AVM
{
    public class BlogItemVM : INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new_page;
        public event DeleteBlogHandler Notify_delete;
        public event UpdateBlogHandler Notify_update;


        ListTabVM tab;

        BlogExampleDM blogExampleDM;

        public BlogItemVM(BlogExampleDM _blogExampleDM, ListTabVM _tab)
        {
            
            blogExampleDM = _blogExampleDM;
            tab = _tab;

            picture = PictureLoader.LoadIllustration(blogExampleDM.Content.IllustrationId);


            openEditingPage = new WCommand(o => {
                TabVM page = new TabVM()
                {
                    Header = "Изменить пост"
                };

                var _blogVM = new EditingBlogVM(this.blogExampleDM, page);
                _blogVM.UCBlogItem = ucBlogItem;
                _blogVM.Notify_update += NotyfyUpdate;
                _blogVM.Notify_close_page += tab.TabClose;

                page.Content = new UC_BlogItemAdd(_blogVM);
                Notify_new_page?.Invoke(page);

            });

            deleteItem = new WCommand(async _ => {
                try
                {
                    await blogExampleDM.DeleteAsync();
                    MessageBox.Show("Запрос выполнен успешно");
                    Notify_delete?.Invoke(ucBlogItem);
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

        public BlogContent Blog { get { return blogExampleDM.Content; } }

        public string PostDate { get { return blogExampleDM.Content.PostDate; } }

        public string Title { get { return blogExampleDM.Content.Title; } }

        public FlowDocument Document
        {
            get
            {
                var document = Converter.Execute(blogExampleDM.Content.Article, 300);
                document.FontSize = 11;
                return document;
            }
        }

        BitmapImage picture;
        public BitmapImage Picture
        {
            get { return picture; }
            set { picture = value; OnPropertyChanged("Picture"); }
        }


        WCommand openEditingPage;
        public WCommand OpenEditingPage { get { return openEditingPage; } }

        WCommand deleteItem;
        public WCommand DeleteItem { get { return deleteItem; } }

        

        UC_BlogItem2 ucBlogItem;
        public UC_BlogItem2 UCBlogItem { set { ucBlogItem = value; } }



        public void NotyfyUpdate() 
        {
            Picture = PictureLoader.LoadIllustration(blogExampleDM.Content.IllustrationId);
            OnPropertyChanged("Document");
            OnPropertyChanged("Title");
            OnPropertyChanged("PostDate");
        }

        


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
