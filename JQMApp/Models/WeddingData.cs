using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using JQMApp.App.Models;
using Newtonsoft.Json;

namespace JQMApp.Models
{
    public class WeddingData
    {
        readonly string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();

        public List<Users> GetUserAddresses(int albumId)
        {
            string json = string.Empty;
            string query = "select * from Users where albumId = " + albumId;

            var userNames = ExecuteObject<Users>(query);
            json = JsonConvert.SerializeObject(userNames);

            return userNames.ToList();
        }

        public string GetPhotosForPage(int albumId, int pageNUmber)
        {
            string json = string.Empty;
            string query = "select p.OTop, p.OLeft, p.Width, p.Height, g.Url, p.Scale, p.Id, p.GraphicId ";
            query += "from Photo p join Graphic g on p.GraphicId = g.Id where p.albumId = " + albumId + " and p.pageNumber = " + pageNUmber;

            var photos = ExecuteObject<RevisedImage>(query);

            json = JsonConvert.SerializeObject(photos);
            return json;
        }

        public string GetAllPhotos(int albumId){
            string json = string.Empty;
            string query = "select * from photos where AlbumId = " + albumId.ToString();

            var photos = ExecuteObject<Photo>(query);
            json = JsonConvert.SerializeObject(photos);

            return json;
        }

        public string GetAllThumbs(int albumId)
        {
            string json = string.Empty;
            string query = "select * from graphic where albumid = " + albumId;
            var photos = ExecuteObject<GraphicItem>(query);

            json = JsonConvert.SerializeObject(photos);

            return json;
        }

        public string GetAlbum(int albumId)
        {
            var json = string.Empty;
            string query = "select * from album where Id = " + albumId;

            var albumDetails = ExecuteObject<Album>(query);

            json = JsonConvert.SerializeObject(albumDetails);
            return json;
        }

        private SqlDataReader SelectQuery(string query)
        {
            var conn = new SqlConnection(_connStr);
            var cmd = new SqlCommand();
            cmd.Connection = conn;

            SqlDataReader reader = null;
            cmd.CommandText = query;
            cmd.Connection.Open();
            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (SqlException sqlException)
            {
                cmd.Connection.Close();
            }
            return reader;
        }

        public IEnumerable<T> ExecuteObject<T>(string sql)
        {
            var items = new List<T>();
            var data = SelectQuery(sql);
            while (data.Read())
            {
                var I = (T)Activator.CreateInstance(typeof(T));
                items.Add(I);
                var props = I.GetType().GetProperties();

                for (var i = 0; i < data.FieldCount; i++)
                {
                    var val = data[i];
                    props[i].SetValue(I, val, null);
                }
            }
            return items;
        }


    }
}