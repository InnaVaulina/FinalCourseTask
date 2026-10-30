using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Blog
{
    public interface IBlogExample
    {
        BlogContent Content { get; }
        void SaveImageContent(ImageFileModel _fileInfo);
    }
}
