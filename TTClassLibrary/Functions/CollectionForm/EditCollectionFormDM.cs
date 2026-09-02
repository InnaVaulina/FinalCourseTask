using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TTClassLibrary.Support;

namespace TTClassLibrary.Functions.CollectionForm
{
    public class EditCollectionFormDM
    {
        CollectionFormRequestSender requestMaker;
        HttpResponseMessageDeserialize<HeaderContent> jsonSerializer;
        HeaderContent content;
        MultipartFormDataContent formData;
        bool existHeader;

        public string HtmlPattern
        {
            get { return content.HtmlPattern; }
            set { content.HtmlPattern = value; }
        }

        public string Title
        {
            get { return content.Title; }
            set { content.Title = value; }
        }
        public string Aims
        {
            get { return content.Aims; }
            set { content.Aims = value; }
        }
        public string Motto
        {
            get { return content.Motto; }
            set { content.Motto = value; }
        }
        public string ButtonText
        {
            get { return content.ButtonText; }
            set { content.ButtonText = value; }
        }

        public string ImagePath
        {
            get { return content.ImagePath; }
            set { content.ImagePath = value; }
        }

        public EditCollectionFormDM(CollectionFormRequestSender _requestMaker)
        {
            requestMaker = _requestMaker;
            jsonSerializer = new HttpResponseMessageDeserialize<HeaderContent>();
            existHeader = false;
            content = new HeaderContent()
            {
                ID = 0,
                HtmlPattern = "",
                Title = "Название сайта",
                Aims = "<p class=\"hero-descr\">Цель 1: описание</p> <p class=\"hero-descr\">Цель 2: описание</p>",
                Motto = "Девиз организации",
                ButtonText = "Что сделать",
                ImagePath = ""
            };

            formData = new MultipartFormDataContent();
        }

        public static async Task<EditCollectionFormDM> CreateAsync(CollectionFormRequestSender _requestMaker)
        {
            var dm = new EditCollectionFormDM(_requestMaker);
            await dm.InitializeAsync();
            return dm;
        }

        public async Task InitializeAsync()
        {
            var response = await requestMaker.GetHeader();
            var newContent = await jsonSerializer.DeserealizeResultToContentAsync(response);
            if (newContent != null)
            {
                content = newContent;
                existHeader = true;
            }
            if (content.ImagePath != "")
            {
                await requestMaker.GetImage(content.ImagePath);
            }
        }

      

        public void SaveImageContent(ImageFileModel fileInfo)
        {
            var imageContent = new ByteArrayContent(fileInfo.Content, 0, fileInfo.Content.Length);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            formData.Add(imageContent, "headerPicture", fileInfo.FileName);
        }

        public async Task<bool> SaveCollectionFormContentAsync()
        {
            var json = JsonSerializer.Serialize(content);
            formData.Add(new StringContent(json, Encoding.UTF8, "application/json"), "headerContent");

            if (existHeader)
            {               

                var response = await requestMaker.SaveHeader(formData);
                if (response.IsSuccessStatusCode) 
                {
                    await this.InitializeAsync();
                    return true;
                }
                    
            }
            else
            {

                var response = await requestMaker.CreateHeader(formData);
                if (response.IsSuccessStatusCode)
                {
                    await this.InitializeAsync();
                    return true;
                }
            }
            return false;
        }
    }


    

    public class HeaderContent
    {
        public int ID { get; set; }
        public string HtmlPattern { get; set; }
        public string Title { get; set; }
        public string Aims { get; set; }
        public string Motto { get; set; }
        public string ButtonText { get; set; }
        public string ImagePath { get; set; }
    }
}
