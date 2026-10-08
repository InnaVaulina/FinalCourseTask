using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Blog.Control;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    /// <summary>
    /// управление информацией о пользователе: удаление, изменение роли, сброс пароля
    /// </summary>
    public class UserItemVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new_page;
        public event DeleteUserHandler Notify_delete;

        ListTabVM tab;
        UserItemDM userItemDM;


        public string Id 
        { 
            get { return userItemDM.UserContent.Id; } 
        }

        public string UserName 
        { 
            get { return userItemDM.UserContent.UserName; } 
        }


        ObservableCollection<UserRole> userRoles;
        public ObservableCollection<UserRole> UserRoles 
        { 
            get { return userRoles; } 
        }


        public UserItemVM(UserItemDM _userItemDM, ListTabVM _tab) 
        {
            userItemDM = _userItemDM;
            userRoles = new ObservableCollection<UserRole>(userItemDM.UserContent.Roles);
            tab = _tab;

            deleteUser = new WCommand(async _ => 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                { 
                    await userItemDM.DeleteUser();
                    MessageBox.Show("Запрос выполнен успешно");
                    Notify_delete?.Invoke(ucUserItem);
                });
            });

            editUserRoles = new WCommand(o =>
            {
                TabVM page = new TabVM()
                {
                    Header = "Изменить роли"  
                };
                var userinrileVM = new AddUserInRoleVM(userItemDM, page);
                userinrileVM.UCUserItem = ucUserItem;
                userinrileVM.Notify_change += UpdateItem;
                userinrileVM.Notify_close += tab.TabClose;

                page.Content = new UC_AddUserInRole(userinrileVM);
                Notify_new_page?.Invoke(page);
            });


            resetPassword = new WCommand(o => 
            {
                PasswordResetVM model = new PasswordResetVM(userItemDM);

                TabVM page = new TabVM()
                {
                    Header = "Сбросить пароль",
                    Content = new UC_PasswordReset(model)
                };
                model.Page = page;
                model.Notify_close += model.Page.ClosePage;
                Notify_new_page?.Invoke(page);
            });
        }


        public void UpdateItem()
        {
            //userRoles.Clear();
            //foreach (var role in userItemDM.UserContent.Roles)
            //    userRoles.Add(role);
        }

        WCommand editUserRoles;
        public WCommand EditUserRoles { get { return editUserRoles; } }

        WCommand resetPassword;
        public WCommand ResetPassword { get { return resetPassword; } }

        WCommand deleteUser;
        public WCommand DeleteUser { get { return deleteUser; } }

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
