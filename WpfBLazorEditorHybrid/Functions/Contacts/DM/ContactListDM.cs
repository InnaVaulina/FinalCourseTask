using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBLazorHybridClient.Client;
using WpfBLazorHybridClient.DataModel;


namespace WpfBLazorHybridClient.Functions.Contacts.DM
{
    public class ContactListDM
    {
        ContactClient requestMaker;
        HttpResponseMessageDeserialize<List<ContactContent>> jsonSerializer1;
        HttpResponseMessageDeserialize<ContactContent> jsonSerializer2;


        List<ContactExampleDM> dmList;
        public List<ContactExampleDM> DMList
        {
            get { return dmList; }
        }
        public ContactListDM(ContactClient _requestMaker)
        {
            requestMaker = _requestMaker;
            jsonSerializer1 = new HttpResponseMessageDeserialize<List<ContactContent>>(requestMaker);
            dmList = new List<ContactExampleDM>();
        }

        public async Task InitializeAsync()
        {
            dmList.Clear();
            var response = await requestMaker.GetAllContacts();
            var contentList = await jsonSerializer1.DeserealizeResultToContentAsync(response);
            foreach (var content in contentList)
            {
                var blogDM = await CtreateContactExampleDM(content);
                dmList.Add(blogDM);
            }
        }

        public async Task<ContactExampleDM> CtreateContactExampleDM(ContactContent content)
        {
            var dm = await ContactExampleDM.CreateAsync(requestMaker, content);
            return dm;
        }

        public AddNewContactExampleDM CreateAddNewContactExampleDM()
        {
            var dm = new AddNewContactExampleDM(requestMaker);
            return dm;
        }
    }
}
