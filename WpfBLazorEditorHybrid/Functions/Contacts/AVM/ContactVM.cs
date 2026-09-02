using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.Control;
using WpfBLazorHybridClient.Functions.Contacts.DM;
using WpfBLazorHybridClient.Main.AVM.Tab;

namespace WpfBLazorHybridClient.Functions.Contacts.AVM
{
    public delegate void DeleteContactHandler(UC_ContactItem? item);
    public delegate void UpdateContactHandler();
    public delegate Task AddContactHandler(ContactContent? contact);
    public class ContactVM
    {
        public event TabAddHandler Notify_new;

        ContactListDM contactListDM;

        ListTabVM tab;

        ObservableCollection<UC_ContactItem> list;
        public ObservableCollection<UC_ContactItem> List { get { return list; } }

        public ContactVM(ContactListDM _contactListDM, ListTabVM _tab)
        {
            contactListDM = _contactListDM;
            tab = _tab;
            list = new ObservableCollection<UC_ContactItem>();

            openAddPage = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Добавить контакт"
                };

                var model = new ContactAddVM(contactListDM.CreateAddNewContactExampleDM(), page);
                model.Notify_close_page += tab.TabClose;
                model.Notify_add += AddItem;

                page.Content = new UC_ContactItemAdd(model);
                Notify_new?.Invoke(page);
            });

        }

        public async Task InitializeAsync()
        {
            try
            {
                await contactListDM.InitializeAsync();
                foreach (var item in contactListDM.DMList)
                {
                    var vm = new ContactItemVM(item, tab);
                    var ucitem = new UC_ContactItem(vm);
                    vm.UCContactItem = ucitem;
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

        }


        public void DeleteItem(UC_ContactItem item)
        {
            List.Remove(item);
        }

        public async Task AddItem(ContactContent? content)
        {
            try
            {
                var dm = await contactListDM.CtreateContactExampleDM(content);
                var vm = new ContactItemVM(dm, tab);
                var ucitem = new UC_ContactItem(vm);
                vm.UCContactItem = ucitem;
                ucitem.Model.Notify_new_page += tab.TabAdd;
                ucitem.Model.Notify_delete += DeleteItem;
                List.Add(ucitem);
            }
            catch (ScopedExeption ex)
            {
                Logger.Log(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }


        WCommand openAddPage;
        public WCommand OpenAddPage { get { return openAddPage; } }



    }
}
