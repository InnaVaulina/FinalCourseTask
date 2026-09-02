using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTB.Client.VModel.Blog
{
    public interface IBlogEdit
    {
        string? Date { get; }
        string Title { get; set; }
        string? ImagePath { get; set; }

        string Article { get; set; }
        void SaveImageContent(MakeImageContent makeImageContent);

        bool State { get; set; }

    }
}
