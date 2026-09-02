using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TTClassLibrary.DataModel;
using TTClassLibrary.Functions.Blog;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.Progect
{
    public class AddNewProgectExampleDM
    {
        IProgectRequestSender requestMaker;


        ProgectContent content;
        MultipartFormDataContent formData;

        public ProgectContent Content
        {
            get { return content; }
        }

        public AddNewProgectExampleDM(IProgectRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;

            content = new ProgectContent()
            {
                ID = 0,
                Title = "Новый проект",
                Description= "",
                Status = "в работе",
                IllustrationId = ""
            };
            formData = new MultipartFormDataContent();
        }

        public void SaveImageContent(ImageFileModel fileInfo)
        {
            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "progectPicture", fileInfo.FileName);
        }

        public async Task<HttpResponseMessage> CreateProgectContentAsync()
        {
            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "progectContent");

            var response = await requestMaker.SaveProgect(formData);
            return response;
        }
    }
}
