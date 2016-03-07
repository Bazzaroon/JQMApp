using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JQMApp.App.Models;
using JQMApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JQMApp.Controllers
{
    public class HomeController : Controller
    {
        private int albumId;
        private int userId;
        private string orientation;

        public string HomePageFileName { get; set; }
        public int AlbumId { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImageManager()
        {
            return View();
        }

        public ActionResult GetUserAddresses(int albumId)
        {
            var data = new WeddingData();
            var users = data.GetUserAddresses(albumId);
            return View(users);
        }

        public ActionResult GetPreview(HttpPostedFileBase file)
        {
            
            ViewBag.Mode = "";
            if (Request.Form["userid"] == "homepage")
            {
                AddImage(file);
                var album = new Album();
                album.UpdateHomePage("Images/" + HomePageFileName, 1000);
                return RedirectToAction("Index", "Home");
            }

            HttpPostedFileBase postedFile = Request.Files["uploadfile"];
            var scaler = new Scaler();

            ViewBag.UploadedFileName = postedFile.FileName;

            var streams =  scaler.CreatePreviewThumbs(postedFile, 996);
            return View(streams);
        }

        public void AddImageFromPreview()
        {
            var memStore = new MemoryStream();

            string P = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                P = reader.ReadToEnd();
            }

            var fName = DateTime.UtcNow.ToString().Replace(@"/","").Replace(@":","").Replace(" ","-");
            
            byte[] buffer = JsonConvert.DeserializeObject<byte[]>(P);

            Stream stream = new MemoryStream(buffer);
 
            PostFileViaFtp(buffer, fName + ".jpg");
            
            var dataModel = new GraphicItem();

            //TODO: Incomplete
            var cookie = Server.UrlDecode(Request.Cookies["userdata"].Value);
            var cookieObj = JsonConvert.DeserializeObject<Users>(cookie);

            dataModel.Add(string.Concat("Images/", fName + ".jpg"), cookieObj.AlbumId, cookieObj.Id);

            var MS = new MemoryStream(buffer);

            CreatePreviewThumb(stream, fName);

            TempData["FilePosted"] = true;

            TempData["uploaded"] = "uploaded";

            JObject J = JObject.Parse(cookie);

        }

        public void AddHomePageImage()
        {
            
        }

        public ActionResult AddImage(HttpPostedFileBase file)
        {

            int imageWidth = 996;
            if (Request.Form["userid"] == "homepage")
            {
                imageWidth = 1920;
            }
            else
            {
                userId = int.Parse(Request.Form["userid"]);
            }

            albumId = int.Parse(Request.Form["albumid"]);
            orientation = Request.Form["orientation"];
            var scaler = new Scaler();

            HttpPostedFileBase postedFile = Request.Files["uploadfile"];


            if (Request.Files["uploadfile"].FileName == "") Index();

            byte[] buffer = scaler.ScaleImage(postedFile, imageWidth, orientation);
           
            HomePageFileName =  Path.GetFileName(postedFile.FileName).Replace(".","_home.");
 
            var dataModel = new GraphicItem();

            //TODO: Incomplete
            if (Request.Form["userid"] == "homepage")
            {
                dataModel.Add(string.Concat("Images/", HomePageFileName), albumId, userId);
                PostFileViaFtp(buffer, HomePageFileName);
                var album = new Album();
                album.UpdateHomePage("Images/" + HomePageFileName, albumId);
            }
            else
            {
                dataModel.Add(string.Concat("Images/", Path.GetFileName(postedFile.FileName)), albumId, userId);
                PostFileViaFtp(buffer, postedFile.FileName);
            }

            CreateThumb(postedFile);

            TempData["FilePosted"] = true;

            TempData["uploaded"] = "uploaded";

            var cookie = Server.UrlDecode(Request.Cookies["userdata"].Value);
            
            JObject J = JObject.Parse(cookie);

            if (Request.Form["userid"] == "homepage")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "SiteUser", new {id = (int) J["Id"]});
            }
        }

        private void CreatePreviewThumb(Stream fileData, string fileName)
        {
            var scaler = new Scaler();
            byte[] buffer = scaler.CreateThumbNailFromPreview(fileData, 120);
            string ftpUrl = string.Concat(System.Configuration.ConfigurationManager.AppSettings["FtpUploadUrl"], fileName + "_t.jpg");
            PostFileViaFtp(buffer, fileName + "_t.jpg");
        }

        public void CreateThumb(HttpPostedFileBase file)
        {
            var scaler = new Scaler();

            HttpPostedFileBase postedFile = Request.Files["uploadfile"];

            byte[] buffer = scaler.CreateThumbNail(file, 120);
            var fileName = Path.GetFileName(postedFile.FileName).Replace(Path.GetExtension(postedFile.FileName), "");

            string ftpUrl = string.Concat(System.Configuration.ConfigurationManager.AppSettings["FtpUploadUrl"], Path.GetFileName(fileName + "_t.jpg"));

            var request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Proxy = null;
            request.UseBinary = true;
            string hostName = HttpContext.Request.ServerVariables["HTTP_HOST"];
            if (hostName == "192.168.0.7" || hostName == "localhost")
            {
                request.Credentials = new NetworkCredential("Barry Tait", "repro20");
            }
            else
            {
                request.Credentials = new NetworkCredential("ftp80152901-0", "Reprosoft1");
            }

            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(buffer, 0, buffer.Length);
            }

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        }
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private void PostFileViaFtp(byte[] buffer, string fileName)
        {
            string ftpUrl = string.Concat(System.Configuration.ConfigurationManager.AppSettings["FtpUploadUrl"], fileName);

            var request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Proxy = null;
            request.UseBinary = true;
            string hostName = HttpContext.Request.ServerVariables["HTTP_HOST"];
            if (hostName == "192.168.0.7" || hostName == "localhost")
            {
                request.Credentials = new NetworkCredential("Barry Tait", "repro20");
            }
            else
            {
                request.Credentials = new NetworkCredential("ftp80152901-0", "Reprosoft1");
            }

            using (Stream writer = request.GetRequestStream())
            {
                writer.Write(buffer, 0, buffer.Length);
            }

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            
        }

    }
}
