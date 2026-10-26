
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.AVM;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Service;
using TTClassLibrary.DataModel;

namespace WpfBLazorHybridClient.Functions.UserHome.AVM
{
    public class UserHomeVM : INotifyPropertyChanged
    {


        UserHomeDM userHomeDM;


        public UserHomeVM(UserHomeDM _userHomeDM)
        {

            userHomeDM = _userHomeDM;
            list = new ObservableCollection<UC_RoleItem>();
            for (int i = 0; i < userHomeDM.UserContent.UserRoles.Count; i++)
            {
                RoleItemVM roleItem = new RoleItemVM()
                {
                    Role = userHomeDM.UserContent.UserRoles[i].Role,
                    RoleChecked = false
                };
                for(int j = 0; j< RoleDescr.roles.Length; j++)
                    if (roleItem.Role == RoleDescr.roles[j])
                    {
                        roleItem.RoleName = RoleDescr.functionName[j];
                        roleItem.Description = RoleDescr.deskr[j];
                    }
               

                list.Add(new UC_RoleItem(roleItem));
                list.Last().RoleChecked.Visibility = System.Windows.Visibility.Collapsed;
            }


            changePassword = new WCommand(async _ => 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    if(string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                    {
                        MessageBox.Show("Поля Новый пароль и Подтверждение пароля должны быть заполнены.");
                        return;
                    }

                    if(Password != ConfirmPassword)
                    {
                        MessageBox.Show("Новый пароль и Подтверждение пароля не совпадают.");
                        return;
                    }

                    if (!string.IsNullOrEmpty(OldPassword)) 
                    {
                        PasswordChangeModel model = new PasswordChangeModel()
                        {
                            OldPassword = this.OldPassword,
                            NewPassword = this.Password
                        };
                        await userHomeDM.ResetPassword(model);
                        MessageBox.Show($"Запрос выполнен.");
                    }

                });
            });
        }





        public string UserName { get { return userHomeDM.UserContent.UserName; } }


        public ObservableCollection<UC_RoleItem> list;
        public ObservableCollection<UC_RoleItem> List { get { return list; } }


        string oldPassword;
        public string OldPassword
        {
            get { return oldPassword; }
            set { oldPassword = value; OnPropertyChanged("OldPassword"); }
        }

        string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }


        WCommand changePassword;
        public WCommand ChangePassword { get { return changePassword; } }

       

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
