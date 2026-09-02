using TTB.VModel.BlogVM;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;

namespace TTB.VModel.ProgectVM
{
    public class AuthProgectListVM
    {
        ProgectListDM progectListDM;

        List<ProgectItemVM> progectItemVMs;
        public List<ProgectItemVM> ProgectItemVMs { get { return progectItemVMs; } }

        public AuthProgectListVM(ProgectListDM _progectListDM)
        {
            progectListDM = _progectListDM;
            progectItemVMs = new List<ProgectItemVM>();
            ShowAll = true;
        }

        bool showAll;
        public bool ShowAll
        {
            get { return showAll; }
            set
            {
                showAll = value;
                if (showAll)
                {
                    ShowPublished = false;
                    ShowInWork = false;
                    progectListDM.Parametres.Search = "ShowAll";
                }
            }
        }

        bool showPublished;
        public bool ShowPublished
        {
            get { return showPublished; }
            set
            {
                showPublished = value;
                if (showPublished)
                {
                    ShowAll = false;
                    ShowInWork = false;
                    progectListDM.Parametres.Search = "ShowPublished";
                }
            }
        }

        bool showInWork;
        public bool ShowInWork
        {
            get { return showInWork; }
            set
            {
                showInWork = value;
                if (showInWork)
                {
                    ShowAll = false;
                    ShowPublished = false;
                    progectListDM.Parametres.Search = "ShowInWork";
                }
            }
        }

        public string Search
        {
            get { return progectListDM.Parametres.Search; }
            set
            {
                switch (value)
                {
                    case "ShowAll":
                        ShowAll = true;
                        break;
                    case "ShowPublished":
                        ShowPublished = true;
                        break;
                    case "ShowInWork":
                        ShowInWork = true;
                        break;
                    default:
                        Search = "ShowAll";
                        break;
                }
            }
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
            progectItemVMs.Clear();
            foreach (var item in progectListDM.DMList)
            {
                var vm = new ProgectItemVM(item);
                progectItemVMs.Add(vm);
            }

        }

    }
}
