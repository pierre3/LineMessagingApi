using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAppSample.Controllers
{
    public class ImagesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string file, string size)
        {
            var root = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var path = Path.Combine(root, "Images", file, size + ".png");

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(path, FileMode.Open, FileAccess.Read));
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = $"{size}.png";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

            return response;
        }
    }
}