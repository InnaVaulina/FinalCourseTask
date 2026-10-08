
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.Functions.Admin;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;
using TTClassLibrary.DataModel;

namespace WpfBLazorHybridClient.Functions.Admin.AVM
{
    public class PasswordResetVM : INotifyPropertyChanged
    {
        public event TabCloseHandler Notify_close;


        UserItemDM userItemDM;

        TabVM page;

        public TabVM Page { get { return page; } set { page = value; } }


        public string Id 
        {
            get { return userItemDM.UserContent.Id; }  
        }

        public string UserName
        {
            get { return userItemDM.UserContent.UserName; }
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


        public PasswordResetVM(UserItemDM _userItemDM)
        {
            userItemDM = _userItemDM;

            saveNewPassword = new WCommand(async _ => 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
                    {
                        MessageBox.Show($"Необходимо заполнить поля \"Новый пароль\" и \"Подтверждение пароля\".");
                        return;
                    }
                    if (Password != ConfirmPassword)
                    {
                        MessageBox.Show($"Пароли не совпадают.");
                        return;
                    }
                    await userItemDM.ResetPassword(new PasswordModel() { Password = this.Password });
                    MessageBox.Show($"Запрос выполнен.");
                    Notify_close?.Invoke(page);
                });
            });

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
