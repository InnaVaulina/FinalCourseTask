using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBLazorHybridClient.Client.Account.UserModel
{
    internal class UserModelForDeserialization
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string UserRole { get; set; }
    }
}
