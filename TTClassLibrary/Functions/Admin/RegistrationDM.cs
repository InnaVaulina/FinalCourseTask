using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using System.Net.Http.Json;

namespace TTClassLibrary.Functions.Admin
{
    public class RegistrationDM
    {
        UserManagerRequestSender requestSender;

        RegisterModel registrationModel;

        public RegisterModel RegisterModel
        {
            get { return registrationModel; }
            set { registrationModel = value; }
        }
        public RegistrationDM(UserManagerRequestSender _requestSender)
        {
            requestSender = _requestSender;
            registrationModel = new RegisterModel();
        }
        public async Task RegisterNewUser()
        {
            var jsonContent = JsonContent.Create(registrationModel);
            await requestSender.Register(jsonContent);
        }

    }
}
