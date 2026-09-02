
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public class PasswordResetVM : INotifyPropertyChanged
    {
        public event TabCloseHandler Notify_close;


        AccountClientExt queryMaker;

        TabVM page;

        public TabVM Page { get { return page; } set { page = value; } }

        string id;
        public string Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value; OnPropertyChanged("UserName");
            }
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


        public PasswordResetVM(UserItemVM _user, AccountClientExt _queryMaker)
        {
            queryMaker = _queryMaker;

            id = _user.Id;
            userName = _user.UserName;

            saveNewPassword = new WCommand(o => 
            {
                ExResetPassword();
            });

        }

        public void ExResetPassword()
        {
            PasswordModel model = new PasswordModel()
            {                
                Password = this.Password
            };

            HttpResponseMessage result = Task.Run(() => queryMaker.ResetUserPassword(this.id, model).GetAwaiter().GetResult()).Result;
            if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
            {
                MessageBox.Show($"Запрос выполнен.");
                Notify_close?.Invoke(page);
            }
            else
            {
                //ErrorResultMessage.Show(result);
            }
        }

        WCommand saveNewPassword;
        public WCommand SaveNewPassword { get { return saveNewPassword; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
