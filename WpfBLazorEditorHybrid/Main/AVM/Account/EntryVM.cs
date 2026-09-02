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
using WpfBLazorHybridClient.Client.Account;
using WpfBLazorHybridClient.Client.Account.UserModel;
using WpfBLazorHybridClient.Command;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Error;
using WpfBLazorHybridClient.Main.AVM.Start;
using TTClassLibrary.Support;

namespace WpfBLazorHybridClient.Main.AVM.Account
{
   
    public class EntryVM : INotifyPropertyChanged
    {
        //public event TabAddHandler Notify_new;
        public event LogInUserHandler Notify;


        UserEntryM entry;
        AccountClientBase queryMaker;


        public string LoginProp
        {
            get { return entry.LoginProp; }
            set { entry.LoginProp = value; OnPropertyChanged("LoginProp"); }
        }

       
        public string PassWord
        {
            get { return entry.PassWord; }
            set { entry.PassWord = value; OnPropertyChanged("PassWord"); }
        }

        

        public EntryVM(AccountClientBase _queryMaker)
        {
            queryMaker = _queryMaker;
            entry = new UserEntryM();
            user = new User();            
            result = null;

            logUser = new WCommand(async o => { await ExecuteLoginUser(); });


            //_________
            entry.LoginProp = "Admin";
            entry.PassWord = "123qwe";

            //___________
        }

        WCommand logUser;
        public WCommand LogUser { get { return logUser; } }

        User user;

        User User
        {
            get { return user; }
            set
            {
                user = value;
                Notify?.Invoke(user);
            }
        }

        HttpResponseMessage result;

        public async Task ExecuteLoginUser()
        {
            result = await queryMaker.Login(entry);
            if (result.IsSuccessStatusCode)
            {
                var token = queryMaker.DeserializeToken(result);
                if (token != null)
                {
                    var identity = new ClaimsIdentity(JwtHelpers.ParseClaimsFromJwt(token), "jwt");
                    User = new User() 
                        {
                        Id = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                        UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                        UserRoles = identity.FindAll(ClaimTypes.Role).Select(r => new UserRole() { Role = r.Value }).ToList(),
                        Token = token 
                        };
                }
                
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
