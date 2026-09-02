using TTClassLibrary.Support;
using TTClassLibrary.DataModel;

namespace TTClassLibrary.Functions.CollectionForm
{
    public class ShowCollectionFormDM
    {
        private readonly CollectionFormRequestSender requestMaker;
        HttpResponseMessageDeserialize<HeaderContent> jsonSerializer;
        HeaderContent content;
        bool existHeader;
 

        public string HtmlPattern
        {
            get { return content.HtmlPattern; }
        }

        public string ButtonText
        {
            get { return content.ButtonText; }
        }


        public string ImagePath
        {
            get { return content.ImagePath; }
            set { content.ImagePath = value; }
        }

        public ShowCollectionFormDM(CollectionFormRequestSender _requestMaker)
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
        }

        public static async Task<ShowCollectionFormDM> CreateAsync(CollectionFormRequestSender _requestMaker)
        {
            var dm = new ShowCollectionFormDM(_requestMaker);
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

        

    }
}
