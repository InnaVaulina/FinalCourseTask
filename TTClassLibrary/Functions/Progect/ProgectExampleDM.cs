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
using TTClassLibrary.DataModel;

namespace TTClassLibrary.Functions.Progect
{
    public class ProgectExampleDM
    {
        IProgectRequestSender requestMaker;

        ProgectContent content;
        ImageFileModel fileInfo;

        public ProgectContent Content
        {
            get { return content; }
        }


        public ProgectExampleDM(IProgectRequestSender _requestMaker, ProgectContent _content)
        {
            requestMaker = _requestMaker;
            content = _content;
            fileInfo = new ImageFileModel()
            {
                FileName = "",
                Content = Array.Empty<byte>()
            };   
        }

        public static async Task<ProgectExampleDM> CreateAsync(IProgectRequestSender _requestMaker, ProgectContent _content)
        {
            var dm = new ProgectExampleDM(_requestMaker, _content);
            await dm.InitializeAsync();
            return dm;
        }

        public static async Task<ProgectExampleDM> CreateAsync(ProgectRequestSender _requestMaker, int id)
        {
            var response = await _requestMaker.GetProgect(id);
            var jsonSerializer = new HttpResponseMessageDeserialize<ProgectContent>();
            ProgectContent _content = await jsonSerializer.DeserealizeResultToContentAsync(response);
            return await CreateAsync(_requestMaker, _content);
        }

        public async Task UpdateDM(ProgectContent newcontent)
        {
            content = newcontent;
            fileInfo = new ImageFileModel()
            {
                FileName = "",
                Content = Array.Empty<byte>()
            };
            await InitializeAsync();
        }
        private async Task InitializeAsync()
        {
            if (content.IllustrationId != "")
                await requestMaker.GetImage(content.IllustrationId);
        }

        public async Task DeleteAsync()
        {
            await requestMaker.DeleteProgect(Content.ID);
        }

        public void SaveImageContent(ImageFileModel _fileInfo)
        {
            fileInfo = _fileInfo;
        }

        public async Task<HttpResponseMessage> ChangeProgectContentAsync()
        {
            MultipartFormDataContent formData = new MultipartFormDataContent();

            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "progectContent");

            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "progectPicture", fileInfo.FileName);

            var response = await requestMaker.UpdateProgect(formData, Content.ID);
            return response;
        }
    }
}
