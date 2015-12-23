using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using JQMApp.Models;


namespace JQMApp.App.Models
{
    public class GraphicItem
    {
        SqlConnection conn = new SqlConnection();


        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int UserId { get; set; }

        public string Url { get; set; }



        public void Add(string fileStream, int albumId, int userId)
        {
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            conn.Open();
            using (SqlCommand command = new SqlCommand("INSERT INTO Graphic(Url, AlbumId, UserId) VALUES (@binaryValue, @albumId, @userId)", conn))
            {
                command.Parameters.Add("@binaryValue", SqlDbType.Text, fileStream.Length).Value = fileStream;
                command.Parameters.Add("@albumId", SqlDbType.Int).Value = albumId;
                command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                command.ExecuteNonQuery();
            }
            conn.Close();
        }

        public GraphicItem GetItemByUrl(string url)
        {
            string query = "select * from Graphic where Url = '" + url + "'";
            var data = new WeddingData();
            return data.ExecuteObject<GraphicItem>(query).FirstOrDefault();
        }

        public IEnumerable<GraphicItem> GetAllForUser(int userId)
        {
            string query = string.Empty;
            if (userId == 0)
            {
                query = "select * from Graphic";
            }
            else
            {
                query = "select * from Graphic where UserId = " + userId.ToString();
            }
            var data = new WeddingData();
            return data.ExecuteObject<GraphicItem>(query).ToList();
        }

    }
}