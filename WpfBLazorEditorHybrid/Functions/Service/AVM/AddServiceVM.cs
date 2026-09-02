using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Service;
using WpfBLazorHybridClient.Functions.Service.AVM;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.Service.AVM
{
    public class AddServiceVM : INotifyPropertyChanged, IServiceEdit
    {
        protected AddNewServiceExampleDM newServiceDM;
        protected TabVM page;

        public event AddServiceHandler Notify_add;
        public event TabCloseHandler Notify_close_page;

        public AddServiceVM(AddNewServiceExampleDM _newServiceDM, TabVM _page)
        {
            newServiceDM = _newServiceDM;
            page = _page;

            saveService = new WCommand(async _ =>
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
                    var response = await newServiceDM.CreateServiceContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<ServiceContent>();
                    var newservice = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newservice != null)
                    {
                        MessageBox.Show("Запрос выполнен успешно");
                        Notify_add?.Invoke(newservice);
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
            get { return newServiceDM.Content.Title; }
            set { newServiceDM.Content.Title = value; OnPropertyChanged("Title"); }
        }


        public string Description
        {
            get { return newServiceDM.Content.Description; }
            set { newServiceDM.Content.Description = value; }
        }

        protected WCommand saveService;
        public WCommand SaveService { get { return saveService; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
