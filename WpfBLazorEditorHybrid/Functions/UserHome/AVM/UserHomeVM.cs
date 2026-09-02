
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Functions.Admin.AVM;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.UserHome.AVM
{
    public class UserHomeVM : INotifyPropertyChanged
    {
        AccountClientExt queryMaker;

        User user;
        public User User { get { return user; } }

        public UserHomeVM(User _user)
        {
            user = _user;
            queryMaker = new AccountClientExt(user);

            list = new ObservableCollection<UC_RoleItem>();
            for (int i = 0; i < user.UserRoles.Count; i++)
            {
                RoleItemVM roleItem = new RoleItemVM()
                {
                    Role = user.UserRoles[i].Role,
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


            changePassword = new WCommand(o => 
            {
                ExChangePassword();
            });
        }

        public string UserName { get { return user.UserName; } }


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

        public void ExChangePassword() 
        {
            PasswordChangeModel model = new PasswordChangeModel()
            {
                OldPassword = this.OldPassword,
                NewPassword = this.Password
            };

            HttpResponseMessage result = Task.Run(() => queryMaker.ChangeCurrentUserPassword(model).GetAwaiter().GetResult()).Result;
            if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
            {
                MessageBox.Show($"Запрос выполнен.");
            }
            else
            {
                //ErrorResultMessage.Show(result);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
