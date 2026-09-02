using TTClassLibrary.Support;

namespace TTB.Client.VModel.Progect
{
    public interface IProgectEdit
    {
        string Title { get; set; }
        string? ImagePath { get; set; }
        string Description { get; set; }
        void SaveImageContent(MakeImageContent makeImageContent);
        bool State { get; set; }
    }
}
