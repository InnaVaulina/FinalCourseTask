using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using TTClassLibrary.Functions.Admin;
using TTClassLibrary.Support;
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Start;
using WpfBLazorHybridClient.Service;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
   
    public class EntryVM : INotifyPropertyChanged
    {
        //public event TabAddHandler Notify_new;
        public event LogInUserHandler Notify;

        EntryDM entryDM;

        public string LoginProp
        {
            get { return entryDM.Entry.LoginProp; }
            set { entryDM.Entry.LoginProp = value; OnPropertyChanged("LoginProp"); }
        }

        public string Password
        {
            get { return entryDM.Entry.PassWord; }
            set { entryDM.Entry.PassWord = value; OnPropertyChanged("Password"); }
        }

        public EntryVM(EntryDM _entryDM)
        {
            entryDM = _entryDM;

            logUser = new WCommand(async o => 
            {
                await CatchExeption.ExecuteWithCatchAsync(async () => 
                {
                    if (LoginProp == "" && Password == "") 
                    {
                        MessageBox.Show("Введите логин и пароль.");
                        return;
                    }
                    var result = await entryDM.LoginNewUser();
                    var token = entryDM.DeserializeToken(result);
                    if (token != null)
                    {
                        var identity = new ClaimsIdentity(JwtHelpers.ParseClaimsFromJwt(token), "jwt");
                        var user = new User()
                        {
                            Id = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                            UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                            UserRoles = identity.FindAll(ClaimTypes.Role).Select(r => new UserRole() { Role = r.Value }).ToList(),
                            Token = token
                        };
                        Notify?.Invoke(user);
                    }
                });
            });


            //_________
            LoginProp = "Admin";
            Password = "123qwe";

            //___________
        }

        WCommand logUser;
        public WCommand LogUser { get { return logUser; } }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
