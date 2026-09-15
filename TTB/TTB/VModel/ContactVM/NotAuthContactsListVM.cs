using TTB.VModel.ServiceVM;
using TTClassLibrary.Functions.Contacts;
using TTClassLibrary.Functions.Service;

namespace TTB.VModel.ContactVM
{
    public class NotAuthContactsListVM
    {
        ContactListDM contactListDM;

        List<INotAuthContactItemVM> contactItemVMs;
        public List<INotAuthContactItemVM> IcontactItemVMs { get { return contactItemVMs; } }
        public NotAuthContactsListVM(ContactListDM _contactListDM)
        {
            contactListDM = _contactListDM;
            contactItemVMs = new List<INotAuthContactItemVM>();
        }

        public async Task SetListAsync()
        {
            await contactListDM.SetListAsync();
            contactItemVMs.Clear();
            foreach (var item in contactListDM.DMList)
            {
                var vm = new ContactItemVM(item);
                contactItemVMs.Add(vm);
            }

        }
    }
}
