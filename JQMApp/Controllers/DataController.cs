using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JQMApp.App.Models;
using JQMApp.Models;
using Newtonsoft.Json;

namespace JQMApp.Controllers
{
    public class DataController : Controller
    {
        //
        // GET: /Data/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetAlbumDetails(int albumId)
        {
            var data = new WeddingData();
            return data.GetAlbum(albumId);
        }

        [HttpGet]
        public string GetPhotosForPage(int albumId, int pageNumber)
        {
            var data = new WeddingData();
            return data.GetPhotosForPage(albumId, pageNumber);
        }

        [HttpGet]
        public string GetAllPhotos(int albumId)
        {
            var data = new WeddingData();
            return data.GetAllPhotos(albumId);
        }

        [HttpGet]
        public string GetAllThumbs(int albumId)
        {
            var data = new WeddingData();
            return data.GetAllThumbs(albumId);
        }

        [HttpGet]
        public void AddPage(int albumId)
        {
            var album = new Album();
            album.AddPage(albumId);
        }

        [HttpPost]
        public void AddImageToPage()
        {
            string P = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                P = reader.ReadToEnd();
            }
            var _photo = new Photo();
            _photo.Add(P);
        }

        [HttpGet]
        public string GetUserAddresses(int albumId)
        {
            var data = new WeddingData();
            return JsonConvert.SerializeObject(data.GetUserAddresses(albumId));
        }

        [HttpPost]
        public string UserLogIn()
        {
            string data = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                data = reader.ReadToEnd();
            }
            var user = JsonConvert.DeserializeObject<Users>(data);

            var validUser = user.Get(user.Email, user.AlbumId, user.PasswordHash);

            return JsonConvert.SerializeObject(validUser);
        }

        [HttpPost]
        public void RegisterUser()
        {
            string data = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                data = reader.ReadToEnd();
            }
            var user = JsonConvert.DeserializeObject<Users>(data);

            user.RegisterUser(user);

        }

        [HttpGet]
        public string GetGraphicByUrl(string url)
        {
            string json = string.Empty;
            var GR = new GraphicItem();
            var GI = GR.GetItemByUrl(url);

            json = JsonConvert.SerializeObject(GI);

            return json;
        }

    }
}
