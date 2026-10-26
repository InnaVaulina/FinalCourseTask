
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Admin;
using TTClassLibrary.Functions.Authorization;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Functions.Admin.Control;
using WpfBLazorHybridClient.Main.AVM.Account;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Service;

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


        string token;
        public string Token
        {
            get { return token; }
            set { token = value; OnPropertyChanged("Token"); }
        }

        

        public PasswordResetVM(UserItemDM _userItemDM)
        {
            userItemDM = _userItemDM;


            saveNewPassword = new WCommand(async _ => 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () =>
                {
                    Token = await userItemDM.GetChangePasswordToken();
                    var message = new TokenResponseMessage(Token);
                    message.Show();
                    //MessageBox.Show($"Запрос выполнен. Токен: {Token}");
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
