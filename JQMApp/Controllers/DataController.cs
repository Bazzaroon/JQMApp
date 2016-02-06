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
        public void DeletePhoto(int Id)
        {
            var photo = new Photo();
            photo.Delete(Id);
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
            Logging.log("Added a page to album " + albumId.ToString());
            var album = new Album();
            album.AddPage(albumId);
        }

        [HttpPost]
        public void UpdateUser()
        {
            string P = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                P = reader.ReadToEnd();
            }
            var _user = new Users();
            _user.Update(P);
        }

        [HttpGet]
        public string GetHomePageImages(int albumId)
        {
            var data = new WeddingData();
            return data.GetHomePageImages(albumId);
        }

        [HttpPost]
        public int AddImageToPage()
        {
            string P = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                P = reader.ReadToEnd();
            }
            var _photo = new Photo();
            return _photo.Add(P);
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
            Logging.log(data);
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

        [HttpPost]
        public void UpdateAlbum()
        {
            string data = string.Empty;
            using (var reader = new StreamReader(HttpContext.Request.InputStream))
            {
                data = reader.ReadToEnd();
            }
            var _album = new Album();
            _album.Update(data);
        }

        [HttpGet]
        public void UpdateHomePage(string fileName, int albumId)
        {
            var album = new Album();
            album.UpdateHomePage(fileName, albumId);
        }
    }
}
