using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using System.Net.Http.Json;
using TTClassLibrary.RequestSender;

namespace TTClassLibrary.Functions.Authorization
{
    public class RegistrationDM
    {
        IHttpRequestSender requestSender;

        RegisterModel registrationModel;

        public RegisterModel RegisterModel
        {
            get { return registrationModel; }
            set { registrationModel = value; }
        }
        public RegistrationDM(IHttpRequestSender _requestSender)
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
