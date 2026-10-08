using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;
using TTClassLibrary.DataModel;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public class AddUserInRoleVM : INotifyPropertyChanged
    {

        public event UpdateUserHandler Notify_change;
        public event TabCloseHandler Notify_close;

        UserItemDM userItemDM;

        TabVM page;

        public TabVM Page { get { return page; } set { page = value; } }

        public string Id { get { return userItemDM.UserContent.Id; } }

        public string UserName { get { return userItemDM.UserContent.UserName; } }


        ObservableCollection<UC_RoleItem> list;

        public ObservableCollection<UC_RoleItem> List { get { return list; } }

        public AddUserInRoleVM(UserItemDM _userItemDM, TabVM _page) 
        {
            userItemDM = _userItemDM;
            page = _page;

            list = new ObservableCollection<UC_RoleItem>();
            for (int i = 0; i < RoleDescr.roles.Length; i++)
            {
                RoleItemVM roleItem = new RoleItemVM()
                {
                    Role = RoleDescr.roles[i],
                    RoleName = RoleDescr.functionName[i],
                    Description = RoleDescr.deskr[i]
                };
                foreach (var role in userItemDM.UserContent.Roles) 
                {
                    if(role.Role == RoleDescr.roles[i])
                        roleItem.RoleChecked = true;
                }
                
                list.Add(new UC_RoleItem(roleItem)); 
            }


            saveRoles = new WCommand(async _=> 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    List<UserRole> newUserRoles = new List<UserRole>();
                    foreach (var role in List)
                    {
                        if (role.Model.RoleChecked == true)
                            newUserRoles.Add(new UserRole() { Role = role.Model.Role });
                    }
                    await userItemDM.ChangeUserRoles(newUserRoles);
                    MessageBox.Show($"Запрос выполнен.");
                    Notify_change?.Invoke();
                    Notify_close?.Invoke(page);
                
                });

            });
        }


        WCommand saveRoles;
        public WCommand SaveRoles { get { return saveRoles; } }


        UC_UserItem ucUserItem;
        public UC_UserItem UCUserItem { set { ucUserItem = value; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
