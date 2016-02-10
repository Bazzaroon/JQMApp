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

        public DateTime CaptureDate { get; set; }



        public void Add(string fileStream, int albumId, int userId)
        {
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            conn.Open();
            using (SqlCommand command = new SqlCommand("INSERT INTO Graphic(Url, AlbumId, UserId, CaptureDate) VALUES (@binaryValue, @albumId, @userId, @captureDate)", conn))
            {
                command.Parameters.Add("@binaryValue", SqlDbType.Text, fileStream.Length).Value = fileStream;
                command.Parameters.Add("@albumId", SqlDbType.Int).Value = albumId;
                command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
                command.Parameters.Add("@captureDate", SqlDbType.DateTime).Value = DateTime.Now;
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

        public IEnumerable<GraphicItem> GetAllForUser(int userId, int count)
        {
            var topCount = count == 0 ? "" : " top " + count.ToString(); 
            string query = string.Empty;
            if (userId == 0)
            {
                query = "select " + topCount + " * from Graphic order by CaptureDate DESC";
            }
            else
            {
                query = "select * from Graphic where UserId = " + userId.ToString() + " order by CaptureDate DESC";
            }
            var data = new WeddingData();
            return data.ExecuteObject<GraphicItem>(query).ToList();
        }

    }
}