using Microsoft.AspNetCore.Components;
using System.Globalization;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Functions.Progect;

namespace TTB.VModel.ProgectVM
{
    public interface INotAuthProgectItemVM
    {
        int Id { get; }
        string ImagePath { get; }
        string Title { get; }

        RenderFragment Fragment { get; }
    }
    public class ProgectItemVM: INotAuthProgectItemVM
    {
        ProgectExampleDM progectExampleDM;

        public int Id { get { return progectExampleDM.Content.ID;  } }
        public string Title { get { return progectExampleDM.Content.Title; }  }


        RenderFragment fragment;
        public RenderFragment Fragment { get { return fragment; } }

        string imagePath;
        public string ImagePath { get { return imagePath; } }

        public ProgectItemVM(ProgectExampleDM _progectExampleDM)
        {
            progectExampleDM = _progectExampleDM;
            fragment = builder => builder.AddMarkupContent(0, progectExampleDM.Content.Description);
            imagePath = "img/" + progectExampleDM.Content.IllustrationId;
        }

        public async Task DeletePost()
        {
            await progectExampleDM.DeleteAsync();
        }
    }
}
