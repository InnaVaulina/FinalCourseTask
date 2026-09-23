using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Work;

namespace TTB.VModel.WorkVM
{
    public delegate void SendRequestHandler();
    public class SendRequestVM
    {
        RequestExampleDM requestExampleDM;

        public event SendRequestHandler Notify_requestSent;
        public SendRequestVM(RequestExampleDM _requestExampleDM)
        {
            requestExampleDM = _requestExampleDM;
            ClearFields();
        }

        public void ClearFields()
        {
            FullName = string.Empty;
            RequestText = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
        }

        public string FullName
        {
            get { return requestExampleDM.Content.FullName; }
            set { requestExampleDM.Content.FullName = value; }
        }
        public string RequestText
        {
            get { return requestExampleDM.Content.RequestText; }
            set { requestExampleDM.Content.RequestText = value; }
        }

        string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        string errorMessage;
        public string ErrorMessage 
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        string successMessage;
        public string SuccessMessage
        {
            get { return successMessage; }
            set { successMessage = value; }
        }

        public async Task<bool> SendRequest()
        {
            if(string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(RequestText))
            {
                errorMessage = "Поля 'ФИО' и 'Текст запроса' обязательны для заполнения.";
                return false;
            }
            if(string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
            {
                errorMessage = "Необходимо указать хотя бы один способ связи: телефон или email.";
                return false;
            }

            requestExampleDM.Content.Contact = $"Phone: {phone}, Mail: {email}";
            requestExampleDM.Content.RequestIn = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            await requestExampleDM.SaveRequestEx();
            ClearFields();
            SuccessMessage = "Запрос успешно отправлен!";
            return true;

        }
    }
}
