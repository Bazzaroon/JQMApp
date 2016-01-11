using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JQMApp.App.Models;
using JQMApp.Models;
using Newtonsoft.Json.Linq;

namespace JQMApp.Controllers
{
    public class HomeController : Controller
    {
        private int albumId;
        private int userId;
        private string orientation;


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

        public ActionResult AddImage(HttpPostedFileBase file)
        {
            
            albumId = int.Parse(Request.Form["albumid"]);
            userId = int.Parse(Request.Form["userid"]);
            orientation = Request.Form["orientation"];
            var scaler = new Scaler();

            HttpPostedFileBase postedFile = Request.Files["uploadfile"];

            if (Request.Files["uploadfile"].FileName == "") Index();

            byte[] buffer = scaler.ScaleImage(postedFile, 996, orientation);

            string ftpUrl = string.Concat(System.Configuration.ConfigurationManager.AppSettings["FtpUploadUrl"], Path.GetFileName(postedFile.FileName));

            var request = (FtpWebRequest)FtpWebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Proxy = null;
            request.UseBinary = true;
            string hostName = HttpContext.Request.ServerVariables["HTTP_HOST"]; 
            if ( hostName == "192.168.0.19" || hostName == "localhost")
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

            var dataModel = new GraphicItem();

            //TODO: Incomplete
            dataModel.Add(string.Concat("Images/", Path.GetFileName(postedFile.FileName)), albumId, userId);

            CreateThumb(postedFile);

            TempData["FilePosted"] = true;

            TempData["uploaded"] = "uploaded";

            var cookie = Server.UrlDecode(Request.Cookies["userdata"].Value);
            
            JObject J = JObject.Parse(cookie);

            return RedirectToAction("Index", "SiteUser", new {id = (int)J["Id"]});

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
            if (hostName == "192.168.0.19" || hostName == "localhost")
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

    }
}
