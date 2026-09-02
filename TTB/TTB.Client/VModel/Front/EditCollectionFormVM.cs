using System.Text.RegularExpressions;
using TTClassLibrary.Functions.CollectionForm;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Front
{
    public delegate void ChangeImageHandler(string objectUrl);
    public delegate Task ChangeModelHandler();

    public class EditCollectionFormVM
    {
        public event ChangeImageHandler ChangeImage;
        public event ChangeModelHandler ChangeModel;

        EditCollectionFormDM editCollectionFormDM;
        public EditCollectionFormDM EditCollectionFormDM { get { return editCollectionFormDM; } }

        public EditCollectionFormVM(EditCollectionFormDM _editCollectionFormDM)
        {
            editCollectionFormDM = _editCollectionFormDM;
        }

        public void SaveImageContent(MakeImageContent makeImageContent) 
        {
            editCollectionFormDM.SaveImageContent(makeImageContent.ImageFile);
            ChangeImage?.Invoke(makeImageContent.ObjectUrl);
        }

        public async Task SaveForm() 
        {
            editCollectionFormDM.HtmlPattern = RemoveExcessive(editCollectionFormDM.HtmlPattern);
            var result = await editCollectionFormDM.SaveCollectionFormContentAsync();
            if (result)
            {
                ChangeModel?.Invoke();
            }
        }


        private static string RemoveExcessive(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Удаляет атрибуты:
            // - contenteditable
            // - contenteditable="true"
            // - contenteditable='true'
            // - contenteditable=true
            // НЕ трогает contenteditable="false"
            var pattern = @"\s*contenteditable(?:\s*=\s*(?:'true'|""true""|true))?";
            var result = Regex.Replace(input, pattern, string.Empty, RegexOptions.IgnoreCase);

            var blPattern = @"\s*_bl_[0-9A-Za-z\-]+(?:\s*=\s*(?:'[^']*'|""[^""]*""|[^\s>]+))?";
            result = Regex.Replace(result, blPattern, string.Empty, RegexOptions.IgnoreCase);

            var commentPattern = @"<!--[\s\S]*?-->";
            result = Regex.Replace(result, commentPattern, string.Empty, RegexOptions.Multiline);

            return result;
        }
    }
}
