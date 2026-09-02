using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Main.AVM.Tab;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.Error;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
    public delegate void ExecuteDeleteUserHandler();
    public class RegistrationVM : INotifyPropertyChanged
    {        
        public event TabCloseHandler Notify_close;
        public event ExecuteDeleteUserHandler Notify_register;

        RegisterM register;
        AccountClientBase queryMaker;

        TabVM page;
        public TabVM Page { get { return page; } set { page = value; } }

        public string LoginProp
        {
            get { return register.LoginProp; }
            set { register.LoginProp = value; OnPropertyChanged("LoginProp"); }
        }

        public string Password
        {
            get { return register.Password; }
            set { register.Password = value; OnPropertyChanged("Password"); }
        }
        public string ConfirmPassword
        {
            get { return register.ConfirmPassword; }
            set { register.ConfirmPassword = value; OnPropertyChanged("ConfirmPassword"); }
        }


        public RegistrationVM(AccountClientBase _queryMaker) 
        {
            register = new RegisterM();
            queryMaker = _queryMaker;

            regUser = new WCommand(o =>
            {
                if (LoginProp != "" && Password != "" && ConfirmPassword != "")
                    ExecuteRegistrUser();
            });
        }

        public HttpResponseMessage result;
        public HttpResponseMessage Result
        {
            set
            {
                result = value;
                if ((int)result.StatusCode == 200 || (int)result.StatusCode == 201)
                {                  
                    MessageBox.Show($"Запрос выполнен.");
                    Notify_close?.Invoke(page);
                    Notify_register?.Invoke();
                }
                else
                {
                    //ErrorResultMessage.Show(result);
                }                       
            }
        }





        public void ExecuteRegistrUser()
        {
            Result = Task.Run(() => queryMaker.Register(register).GetAwaiter().GetResult()).Result;
        }

        WCommand regUser;
        public WCommand RegUser { get { return regUser; } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }



 
}
