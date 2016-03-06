using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JQMApp.App.Models;

namespace JQMApp.Models
{
    public class AlbumSettings
    {
        public Album album { get; set; }

        public List<Users> Users { get; set; }

        public List<GraphicItem> GraphicItems { get; set; } 

        public AlbumSettings()
        {
            Users = new List<Users>();
            GraphicItems = new List<GraphicItem>();
        }

        public AlbumSettings Get(int albumId)
        {
            var data = new WeddingData();
            this.album = data.GetAlbumSettings(albumId);
            this.Users = data.GetAllUsers(albumId).ToList();
            this.GraphicItems = data.GetAllGraphicItems(albumId).ToList();

            return this;
        }
    }
}