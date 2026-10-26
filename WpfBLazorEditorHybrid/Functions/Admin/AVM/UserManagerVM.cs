
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using TTClassLibrary.Functions.Admin;
using TTClassLibrary.Functions.Blog;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Main.AVM.Account;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;
using TTClassLibrary.DataModel;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public delegate void ExecuteDeleteUserHandler();

    public delegate void DeleteUserHandler(UC_UserItem? item);
    public delegate void UpdateUserHandler(UserModel user);
    //public delegate Task AddUserHandler(BlogContent newcontent);

    /*
     * управление пользователями: получение списка пользователей, регистрация нового ползователя
     */
    public class UserManagerVM
    {
        public event TabAddHandler Notify_new;

        ListTabVM tab;
        UserManagerDM userManagerDM;

        ObservableCollection<UC_UserItem> list;

        public ObservableCollection<UC_UserItem> List { get { return list; } }


        public UserManagerVM(UserManagerDM _userManagerDM, ListTabVM _tab) 
        {
            userManagerDM = _userManagerDM;
            tab = _tab;
            list = new ObservableCollection<UC_UserItem>();
        }

       

        public async Task InitializeAsync()
        {
            await CatchExeption.ExecuteWithCatchAsync(async () =>
            {
                list.Clear();
                await userManagerDM.SelectUsers();
                foreach (var user in userManagerDM.List)
                {
                    var vm = new UserItemVM(userManagerDM.CreateUserItemDM(user), tab);
                    var ucitem = new UC_UserItem(vm);
                    vm.UCUserItem = ucitem;
                    ucitem.Model.Notify_new_page += tab.TabAdd;
                    ucitem.Model.Notify_delete += DeleteItem;
                    List.Add(ucitem);
                }
            });
        }

        public void DeleteItem(UC_UserItem item)
        {
            List.Remove(item);
        }


        void TabNotify(TabVM page)
        {
            Notify_new?.Invoke(page);
        }

    }
}
