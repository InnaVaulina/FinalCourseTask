using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Functions.Progect.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.Progect.AVM
{
    public class EditingProgectVM: INotifyPropertyChanged, IProgectEdit
    {
        public event UpdateProgectHandler Notify_update;
        public event TabCloseHandler Notify_close_page;

        ProgectExampleDM progectExampleDM;
        TabVM page;
        public EditingProgectVM(ProgectExampleDM _progectExampleDM, TabVM _page)
        {
            progectExampleDM = _progectExampleDM;
            page = _page;

            string imageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];
            string savePath = @$"{imageSavePath}{progectExampleDM.Content.IllustrationId}";
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

            saveProgect = new WCommand(async _ =>
            {
                if (Title == "")
                {
                    MessageBox.Show("Название не заполнено!");
                    return;
                }
                if (Description == "")
                {
                    MessageBox.Show("Текст не заполнено!");
                    return;
                }

                try
                {
                    progectExampleDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    var response = await progectExampleDM.ChangeProgectContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<ProgectContent>();
                    var newprogect = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newprogect != null)
                    {
                        await progectExampleDM.UpdateDM(newprogect);
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_update?.Invoke();
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


        public string Title
        {
            get { return progectExampleDM.Content.Title; }
            set { progectExampleDM.Content.Title = value; OnPropertyChanged("Title"); }
        }


        public string Description
        {
            get { return progectExampleDM.Content.Description; }
            set { progectExampleDM.Content.Description = value; }
        }

        public bool State
        {
            get
            {
                if (progectExampleDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) progectExampleDM.Content.Status = "InWork";
                else progectExampleDM.Content.Status = "Published";
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
            progectExampleDM.SaveImageContent(fileInfo);
        }

        protected WCommand saveProgect;
        public WCommand SaveProgect { get { return saveProgect; } }


        UC_ProgectItem ucProgectItem;
        public UC_ProgectItem UCProgectItem { set { ucProgectItem = value; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
