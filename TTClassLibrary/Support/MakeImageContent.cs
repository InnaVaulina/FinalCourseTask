using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TTClassLibrary.Support
{
    public class MakeImageContent
    {
        public ImageFileModel? ImageFile { get; set; } = null;
        public string? ObjectUrl { get; set; } = null;

    }

    public class ImageFileModel
    {
        public string FileName { get; set; } = "";
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "";
    }
}
