using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Contacts.AVM;
using WpfBLazorHybridClient.Functions.Contacts.AVM.AddContact;
using WpfBLazorHybridClient.Functions.Contacts.AVM.ChangeContact;
using WpfBLazorHybridClient.Functions.Contacts.DM;

namespace WpfBLazorHybridClient.Client
{
    public class ContactClient: HttpRequestSender
    {
        public ContactClient(User user) : base(user) { }


        
        public async Task<HttpResponseMessage> GetAllContacts()
        {
            var url = $"{baseAddress}/api/Contact/GetAllContacts";
            return await Get(url);
        }

        public async Task<HttpResponseMessage> GetContact(int id)
        {
            var url = $"{baseAddress}/api/Contact/GetContact?id={id}";
            return await Get(url);
        }


        public async Task<HttpResponseMessage> SaveContact(AddNewContactExampleDM contactDM)
        {
            var url = $"{baseAddress}/api/Contact/SaveContact";
            using (var formData = new MultipartFormDataContent())
            {
                var json = JsonSerializer.Serialize(contactDM.Content);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "contactContent");

                if (contactDM.AddressDM.FileInfo != null)
                {
                    var content = MakeImageContent(contactDM.AddressDM.FileInfo);
                    if (content != null)
                        formData.Add(content, "addressPicture", contactDM.AddressDM.FileInfo.Name);
                }

                foreach (var linkDM in contactDM.SocialLinksListDM)
                {
                    var content = MakeImageContent(linkDM.FileInfo);
                    if (content != null)
                        formData.Add(content, "socialIcons", linkDM.FileInfo.Name);
                }

                return await Post(url, formData);
            }
        }


        public async Task<HttpResponseMessage> UpdateContact(ContactExampleDM contactDM)
        {
            var url = $"{baseAddress}/api/Contact/UpdateContact?id={contactDM.Content.ID}";
            using (var formData = new MultipartFormDataContent())
            {
                var json = JsonSerializer.Serialize(contactDM.Content);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "contactContent");

                json = JsonSerializer.Serialize(contactDM.DeletedContent);
                formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "deletedContactContent");

                if (contactDM.AddressDM.FileInfo != null)
                {
                    var content = MakeImageContent(contactDM.AddressDM.FileInfo);
                    if (content != null)
                        formData.Add(content, "addressPicture", contactDM.AddressDM.FileInfo.Name);
                }

                foreach (var linkDM in contactDM.SocialLinksListDM.ContentList)
                {
                    var content = MakeImageContent(linkDM.FileInfo);
                    if (content != null)
                        formData.Add(content, "socialIcons", linkDM.FileInfo.Name);
                }

                return await Put(url, formData);
            }
        }

        public async Task<HttpResponseMessage> DeleteContact(ContactContent contact)
        {
            var url = $"{baseAddress}/api/Contact/DeleteContact?id={contact.ID}";
            return await Delete(url);
        }


    }
}
