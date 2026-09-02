using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using TTB.Client.VModel;
using TTClassLibrary.Functions.CollectionForm;


namespace TTB.VModel.FrontVM
{
    public delegate Task ChangeModelHandler();
    public class ShowCollectionFormVM
    {
        public event ChangeModelHandler ChangeModel;
        private readonly ShowCollectionFormDM showFormDM;

        public ShowCollectionFormDM ShowCollectionFormDM { get { return showFormDM; } }
        public ShowCollectionFormVM(ShowCollectionFormDM _showFormDM)
        {
            showFormDM = _showFormDM;
            fragment = builder => builder.AddMarkupContent(0, showFormDM.HtmlPattern);
        }

        public async Task OnModelChange() 
        {
            await showFormDM.InitializeAsync();
            fragment = builder => builder.AddMarkupContent(0, showFormDM.HtmlPattern);
            ChangeModel?.Invoke();
        }



        RenderFragment fragment;
        public RenderFragment Fragment { get { return fragment; } }

    }
}
