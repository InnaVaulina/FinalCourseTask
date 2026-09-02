using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Service;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Progect.AVM;
using WpfBLazorHybridClient.Functions.Service.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Service.AVM
{

    public class ServiceItemVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new_page;
        public event DeleteServiceHandler Notify_delete;
        public event UpdateServiceHandler Notify_update;


        ListTabVM tab;

        ServiceExampleDM serviceExampleDM;


        public ServiceItemVM(ServiceExampleDM _serviceExampleDM, ListTabVM _tab)
        {
            serviceExampleDM = _serviceExampleDM;
            tab = _tab;
            

            openEditingPage = new WCommand(o => {
                TabVM page = new TabVM()
                {
                    Header = "Изменить услугу"
                };

                var _serviceVM = new EditingServiceVM(this.serviceExampleDM, page);
                _serviceVM.UCServiceItem = ucServiceItem;
                _serviceVM.Notify_update += NotyfyUpdate;
                _serviceVM.Notify_close_page += tab.TabClose;

                page.Content = new UC_ServiceItemAdd(_serviceVM);
                Notify_new_page?.Invoke(page);

            });

            deleteItem = new WCommand(async _ => {
                try
                {
                    await serviceExampleDM.DeleteAsync();
                    MessageBox.Show("Запрос выполнен успешно");
                    Notify_delete?.Invoke(ucServiceItem);
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

        public ServiceContent Service { get { return serviceExampleDM.Content; } }

        public string Title { get { return serviceExampleDM.Content.Title; } }

        public FlowDocument Document
        {
            get
            {
                var document = Converter.Execute(serviceExampleDM.Content.Description, 3000);
                document.FontSize = 11;
                return document;
            }
        }


        WCommand openEditingPage;
        public WCommand OpenEditingPage { get { return openEditingPage; } }

        WCommand deleteItem;
        public WCommand DeleteItem { get { return deleteItem; } }

        UC_ServiceItem ucServiceItem;
        public UC_ServiceItem UCServiceItem { set { ucServiceItem = value; } }

        public void NotyfyUpdate()
        {
            ucServiceItem.Model = this;
            OnPropertyChanged("Title");
            OnPropertyChanged("Document");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
