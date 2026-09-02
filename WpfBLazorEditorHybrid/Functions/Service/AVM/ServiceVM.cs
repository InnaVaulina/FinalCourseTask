using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Windows;
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

    public delegate void DeleteServiceHandler(UC_ServiceItem? item);
    public delegate void UpdateServiceHandler();
    public delegate void AddServiceHandler(ServiceContent service);
    public class ServiceVM
    {
        public event TabAddHandler Notify_new;

        ListTabVM tab;
        ServiceListDM serviceListDM;

        ObservableCollection<UC_ServiceItem> list;
        public ObservableCollection<UC_ServiceItem> List { get { return list; } }

        public ServiceVM(ServiceListDM _serviceListDM, ListTabVM _tab)
        {
            tab = _tab;
            serviceListDM = _serviceListDM;
            list = new ObservableCollection<UC_ServiceItem>();

            openAddPage = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Добавить услугу"
                };

                var model = new AddServiceVM(serviceListDM.CreateAddNewServiceExampleDM(), page);
                model.Notify_close_page += tab.TabClose;
                model.Notify_add += AddItem;

                page.Content = new UC_ServiceItemAdd(model);
                Notify_new?.Invoke(page);

            });

        }


        WCommand openAddPage;
        public WCommand OpenAddPage { get { return openAddPage; } }

        public async Task InitializeAsync()
        {
            try
            {
                await serviceListDM.InitializeAsync();
                List.Clear();
                foreach (var item in serviceListDM.DMList)
                {
                    var vm = new ServiceItemVM(item, tab);
                    var ucitem = new UC_ServiceItem(vm);
                    vm.UCServiceItem = ucitem;
                    ucitem.Model.Notify_new_page += tab.TabAdd;
                    ucitem.Model.Notify_delete += DeleteItem;
                    List.Add(ucitem);
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
        }
        

        public void DeleteItem(UC_ServiceItem item)
        {
            list.Remove(item);
        }


        public void AddItem(ServiceContent service)
        {
            try
            {
                var dm = serviceListDM.CreateServiceExampleDM(service);
                var vm = new ServiceItemVM(dm, tab);
                var ucitem = new UC_ServiceItem(vm);
                vm.UCServiceItem = ucitem;
                ucitem.Model.Notify_new_page += tab.TabAdd;
                ucitem.Model.Notify_delete += DeleteItem;
                List.Add(ucitem);
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
        }

        
    }
}
