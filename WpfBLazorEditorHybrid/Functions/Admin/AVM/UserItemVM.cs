using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Client.Account;
using System.Net.Http;
using System.Windows;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Client.Account.UserModel;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    /// <summary>
    /// управление информацией о пользователе: удаление, изменение роли, сброс пароля
    /// </summary>
    public class UserItemVM: INotifyPropertyChanged
    {
        public event TabAddHandler Notify_new;
        public event ExecuteDeleteUserHandler Notify_delete;

        AccountClientExt queryMaker;


        string id;
        public string Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        string userName;
        public string UserName { get { return userName; } set { userName = value; OnPropertyChanged("UserName"); } }


        ObservableCollection<UserRole> userRoles;
        public ObservableCollection<UserRole> UserRoles { get { return userRoles; } set { userRoles = value; } }


        public UserItemVM(AccountClientExt _queryMaker) 
        {
            
            queryMaker = _queryMaker;
            

            deleteUser = new WCommand(o => 
            { 
                ExDeleteUser();
                
            });

            editUserRoles = new WCommand(o =>
            {
                AddUserInRoleVM model = new AddUserInRoleVM(this, queryMaker);
                
                TabVM page = new TabVM()
                {
                    Header = "Изменить роли",
                    Content = new UC_AddUserInRole(model)
                };
                model.Page = page;
                model.Notify_change += ExChangeRole;
                model.Notify_close += model.Page.ClosePage;
                Notify_new?.Invoke(page);
            });


            resetPassword = new WCommand(o => 
            {
                PasswordResetVM model = new PasswordResetVM(this, queryMaker);

                TabVM page = new TabVM()
                {
                    Header = "Сбросить пароль",
                    Content = new UC_PasswordReset(model)
                };
                model.Page = page;
                model.Notify_close += model.Page.ClosePage;
                Notify_new?.Invoke(page);
            });
        }

        WCommand editUserRoles;
        public WCommand EditUserRoles { get { return editUserRoles; } }

        WCommand resetPassword;
        public WCommand ResetPassword { get { return resetPassword; } }

        WCommand deleteUser;
        public WCommand DeleteUser { get { return deleteUser; } }


        public void ExDeleteUser()
        {
            UserModelForAdmin usermodel = new UserModelForAdmin()
            {
                Id = this.Id,
                UserName = this.UserName,
                UserRoles = this.userRoles.ToList()
            };
            HttpResponseMessage result = Task.Run(() => queryMaker.DeleteUser(usermodel).GetAwaiter().GetResult()).Result;
            if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
            {
                MessageBox.Show($"Запрос выполнен.");
                Notify_delete?.Invoke();
            }
            else
            {
                //ErrorResultMessage.Show(result);
            }

        }

        public void ExChangeRole() 
        {
            Notify_delete?.Invoke();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
