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
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;
using TTClassLibrary.Functions.Service;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Service.AVM;
using WpfBLazorHybridClient.Functions.Service.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Service.AVM
{
    public class EditingServiceVM : INotifyPropertyChanged, IServiceEdit
    {
        public event UpdateServiceHandler Notify_update;
        public event TabCloseHandler Notify_close_page;

        ServiceExampleDM serviceExampleDM;
        TabVM page;

        public EditingServiceVM(ServiceExampleDM _serviceExampleDM, TabVM _page)
        {
            serviceExampleDM = _serviceExampleDM;
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
                    var response = await serviceExampleDM.ChangeServiceContentAsync();
                    var jsonSerializer = new TTClassLibrary.Support.HttpResponseMessageDeserialize<ServiceContent>();
                    var newservice = await jsonSerializer.DeserealizeResultToContentAsync(response);
                    if (newservice != null)
                    {
                        await serviceExampleDM.UpdateDM(newservice);
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
            get { return serviceExampleDM.Content.Title; }
            set { serviceExampleDM.Content.Title = value; OnPropertyChanged("Title"); }
        }


        public string Description
        {
            get { return serviceExampleDM.Content.Description; }
            set { serviceExampleDM.Content.Description = value; }
        }

       

        protected WCommand saveService;
        public WCommand SaveService { get { return saveService; } }


        UC_ServiceItem ucServiceItem;
        public UC_ServiceItem UCServiceItem { set { ucServiceItem = value; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
