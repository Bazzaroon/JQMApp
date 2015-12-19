using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace JQMApp.App.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int OTop { get; set; }
        public int OLeft { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PageNumber { get; set; }
        public int AlbumId { get; set; }
        public int GraphicId { get; set; }
        public int Scale { get; set; }


        public void Add(string pic
            )
        {
            var conStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            var conn = new SqlConnection(conStr);
            JObject photo = JObject.Parse(pic);

            var query = "Insert into Photo ";
            query += "(OTop,OLeft,Width,PageNumber,AlbumId,GraphicId) Values (";
            query += (int)photo["OTop"] + "," + (int)photo["OLeft"] + "," + (int)photo["Width"] + "," + (int)photo["PageNumber"] + "," + (int)photo["AlbumId"] + "," + (int)photo["GraphicId"] + ")";

            var cmd = new SqlCommand(query, conn);
            cmd.Connection.Open();

            try
            {
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (SqlException exception)
            {
                cmd.Connection.Close();
            }

        }

    }
}