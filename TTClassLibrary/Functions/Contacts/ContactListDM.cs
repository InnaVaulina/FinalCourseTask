using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Contacts
{
    public class ContactListDM
    {
        IContactRequestSender requestMaker;

        List<ContactExampleDM> dmList;
        public List<ContactExampleDM> DMList
        {
            get { return dmList; }
        }
        public ContactListDM(IContactRequestSender requestMaker) 
        {
            this.requestMaker = requestMaker;
            dmList = new List<ContactExampleDM>();
        }

        public async Task InitializeAsync()
        {
            dmList.Clear();
            var response = await requestMaker.GetAllContacts();
            var jsonSerializer1 = new HttpResponseMessageDeserialize<List<ContactContent>>();
            var contentList = await jsonSerializer1.DeserealizeResultToContentAsync(response);
            foreach (var content in contentList)
            {
                var contactDM = await CtreateContactExampleDM(content);
                dmList.Add(contactDM);
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
