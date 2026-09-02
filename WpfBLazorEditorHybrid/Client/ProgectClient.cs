using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Windows;
using WpfBLazorHybridClient.DataModel;
using WpfBLazorHybridClient.Functions.Blog.AVM;
using WpfBLazorHybridClient.Functions.Progect.AVM;

namespace WpfBLazorHybridClient.Client
{
    public class ProgectClient: HttpRequestSender
    {
        public ProgectClient(User user): base(user) { }


        public async Task<HttpResponseMessage> DeleteProgect(ProgectContent progect)
        {
            var url = $"{baseAddress}/api/Progect/DeleteProgect?id={progect.ID}";
            try
            {
                return await Delete(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<HttpResponseMessage> SaveProgect(AddProgectVM model)
        {
            var url = $"{baseAddress}/api/Progect/SaveProgect";
            using (var formData = new MultipartFormDataContent())
            {
                try
                {
                    if (model.ImageFilePath != null && model.ImageFilePath.Exists)
                    {
                        var fileBytes = File.ReadAllBytes(model.ImageFilePath.FullName);
                        var fileContent = new ByteArrayContent(fileBytes);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/" + Path.GetExtension(model.ImageFilePath.FullName).TrimStart('.'));
                        formData.Add(fileContent, "image", Path.GetFileName(model.ImageFilePath.FullName));
                    }

                    formData.Add(new StringContent(model.Title), "title");
                    formData.Add(new StringContent(model.Description), "description");
                    formData.Add(new StringContent(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)), "date");
                    if (model.State == true)
                        formData.Add(new StringContent("готово"), "status");
                    else
                        formData.Add(new StringContent("в работе"), "status");

                    return await Post(url,formData);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
               
            }
        }



        //public async Task<HttpResponseMessage> UpdateProgect(AddProgectVM model)
        //{
        //    var url = $"{baseAddress}/api/Progect/UpdateProgect?id={model.ID}";
        //    using (var formData = new MultipartFormDataContent())
        //    {
        //        try
        //        {
        //            if (model.ImageFilePath != null && model.ImageFilePath.Exists)
        //            {
        //                var fileBytes = File.ReadAllBytes(model.ImageFilePath.FullName);
        //                var fileContent = new ByteArrayContent(fileBytes);
        //                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/" + Path.GetExtension(model.ImageFilePath.FullName).TrimStart('.'));
        //                formData.Add(fileContent, "image", Path.GetFileName(model.ImageFilePath.FullName));
        //            }


        //            formData.Add(new StringContent(model.Title), "title");
        //            formData.Add(new StringContent(model.Description), "description");
        //            formData.Add(new StringContent(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)), "date");
        //            if (model.State == true)
        //                formData.Add(new StringContent("готово"), "status");
        //            else
        //                formData.Add(new StringContent("в работе"), "status");

        //            return await Put(url,formData);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
                
        //    }
        //}


        public async Task<HttpResponseMessage> GetAllProgects()
        {
            var url = $"{baseAddress}/api/Progect/GetAllProgects";
            try
            {
                return await Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<HttpResponseMessage> GetProgect(Uri? getprogecturl)
        {
            if(getprogecturl == null)
                throw new Exception("GetProgect: getprogecturl is null");
            try
            {
                return await Get(getprogecturl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
