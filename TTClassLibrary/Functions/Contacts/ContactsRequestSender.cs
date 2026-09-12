using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.IServices;

namespace TTClassLibrary.Functions.Contacts
{
    public interface IContactRequestSender 
    {
        Task<HttpResponseMessage> GetAllContacts();
        Task<HttpResponseMessage> GetContact(int id);
        Task<HttpResponseMessage> SaveContact(MultipartFormDataContent formData);
        Task<HttpResponseMessage> UpdateContact(MultipartFormDataContent formData, int id);
        Task<HttpResponseMessage> DeleteContact(int id);
        Task<FileInfo?> GetImage(string fileName);

    }

    public class ContactsRequestSender: IContactRequestSender
    {
        protected IHttpRequestSender requestSender;
        public ContactsRequestSender(IHttpRequestSender _requestSender)
        {
            requestSender = _requestSender;
        }

        public async Task<HttpResponseMessage> GetAllContacts()
        {
            var url = $"api/Contact/GetAllContacts";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> GetContact(int id)
        {
            var url = $"api/Contact/GetContact?id={id}";
            return await requestSender.Get(url);
        }

        public async Task<HttpResponseMessage> SaveContact(MultipartFormDataContent formData)
        {
            var url = $"api/Contact/SaveContact";
            return await requestSender.Post(url, formData);
        }

        public async Task<HttpResponseMessage> UpdateContact(MultipartFormDataContent formData, int id)
        {
            var url = $"api/Contact/UpdateContact?id={id}";
            return await requestSender.Put(url, formData);
        }

        public async Task<HttpResponseMessage> DeleteContact(int id)
        {
            var url = $"api/Contact/DeleteContact?id={id}";
            return await requestSender.Delete(url);
        }

        public async Task<FileInfo?> GetImage(string fileName)
        {
            return await requestSender.GetImage(fileName);
        }
    }
}
