using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Progect.AVM
{
    public class AddProgectVM : INotifyPropertyChanged, IProgectEdit
    {
        protected AddNewProgectExampleDM newProgectDM;
        protected TabVM page;

        public event AddProgectHandler Notify_add;
        public event TabCloseHandler Notify_close_page;

        public AddProgectVM(AddNewProgectExampleDM _newProgectDM, TabVM _page)
        {
            newProgectDM = _newProgectDM;
            page = _page;

            picture = new BitmapImage();
            imageFilePath = null;


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
                    newProgectDM.Content.PostDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    var response = await newProgectDM.CreateProgectContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<ProgectContent>();
                    var newprogect = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newprogect != null)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_add?.Invoke(newprogect);
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
            get { return newProgectDM.Content.Title; }
            set { newProgectDM.Content.Title = value; OnPropertyChanged("Title"); }
        }


        public string Description
        {
            get { return newProgectDM.Content.Description; }
            set { newProgectDM.Content.Description = value; }
        }


        public bool State
        {
            get
            {
                if (newProgectDM.Content.Status == "InWork") return true;
                else return false;
            }
            set
            {
                if (value == true) newProgectDM.Content.Status = "InWork";
                else newProgectDM.Content.Status = "Published";
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
            newProgectDM.SaveImageContent(fileInfo);
        }

        protected WCommand saveProgect;
        public WCommand SaveProgect { get { return saveProgect; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
