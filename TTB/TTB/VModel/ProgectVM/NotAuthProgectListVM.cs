using TTB.VModel.BlogVM;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;

namespace TTB.VModel.ProgectVM
{
    public class NotAuthProgetListVM
    {
        ProgectListDM progectListDM;

        List<INotAuthProgectItemVM> iprogectItemVMs;
        public List<INotAuthProgectItemVM> IprogectItemVMs { get { return iprogectItemVMs; } }
        public NotAuthProgetListVM(ProgectListDM _progectListDM)
        {
            progectListDM = _progectListDM;
            iprogectItemVMs = new List<INotAuthProgectItemVM>();
            progectListDM.Parametres.Search = "ShowPublished";
        }

      

        public int? Page
        {
            get { return progectListDM.Parametres.Page; }
            set { progectListDM.Parametres.Page = value; }
        }

        public int TotalPages { get { return progectListDM.TotalPages; } }

        public async Task SetListAsync()
        {
            await progectListDM.SetListAsync();
            iprogectItemVMs.Clear();
            foreach (var item in progectListDM.DMList)
            {
                var vm = new ProgectItemVM(item);
                iprogectItemVMs.Add(vm);
            }

        }
    }

}
