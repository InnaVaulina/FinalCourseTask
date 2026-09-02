using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Progect.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.Progect.AVM
{
    public class ProgectItemVM : INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new_page;
        public event DeleteProgectHandler Notify_delete;
        public event UpdateProgectHandler Notify_update;

        ListTabVM tab;

        ProgectExampleDM progectExampleDM;

        public ProgectItemVM(ProgectExampleDM _progectExampleDM, ListTabVM _tab)
        {
            progectExampleDM = _progectExampleDM;
            tab = _tab;

            picture = new BitmapImage();
            LoadIllustration();

            openEditingPage = new WCommand(o => {
                TabVM page = new TabVM()
                {
                    Header = "Изменить пост"
                };

                var _progectVM = new EditingProgectVM(this.progectExampleDM, page);
                _progectVM.UCProgectItem = ucProgectItem;
                _progectVM.Notify_update += NotyfyUpdate;
                _progectVM.Notify_close_page += tab.TabClose;

                page.Content = new UC_ProgectItemAdd(_progectVM);
                Notify_new_page?.Invoke(page);

            });

            deleteItem = new WCommand(async _ => {
                try
                {
                    await progectExampleDM.DeleteAsync();
                    MessageBox.Show("Запрос выполнен успешно");
                    Notify_delete?.Invoke(ucProgectItem);
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

        public ProgectContent Progect { get { return progectExampleDM.Content; } }

        public string Title { get { return progectExampleDM.Content.Title; } }

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


        UC_ProgectItem ucProgectItem;
        public UC_ProgectItem UCProgectItem { set { ucProgectItem = value; } }

        

        public void NotyfyUpdate()
        {
            LoadIllustration();
            OnPropertyChanged("Title");
        }

        public void LoadIllustration()
        {
            string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
            string savePath = @$"{imageSavePath}{progectExampleDM.Content.IllustrationId}";
            FileInfo fileInfo = new FileInfo(savePath);

            if (!fileInfo.Exists)
            {
                Picture = new BitmapImage();
                return;
            }
            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

