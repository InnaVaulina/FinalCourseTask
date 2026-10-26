using TTClassLibrary.DataModel;
using TTClassLibrary.RequestSender;
using System.Net.Http.Json;

namespace TTClassLibrary.Functions.Authorization
{
    public class ResetPasswordDM
    {
        IHttpRequestSender requestSender;

        ResetPasswordModel resetPasswordModel;

        public ResetPasswordModel ResetPasswordModel
        {
            get { return resetPasswordModel; }
            set { resetPasswordModel = value; }
        }

        public ResetPasswordDM(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
            resetPasswordModel = new ResetPasswordModel();
        }

        public async Task ResetUserPassword()
        {
            var jsonContent = JsonContent.Create(resetPasswordModel);
            await requestSender.ResetUserPasswordWithToken(jsonContent);
        }

    }
}
