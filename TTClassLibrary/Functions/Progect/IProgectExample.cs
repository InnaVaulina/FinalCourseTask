using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Progect
{
    public interface IProgectExample
    {
        ProgectContent Content { get; }
        void SaveImageContent(ImageFileModel _fileInfo);
    }
}
