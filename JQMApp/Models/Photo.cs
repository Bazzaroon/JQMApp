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

        static string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
        SqlConnection conn = new SqlConnection(conStr);

        public int Add(string pic
            )
        {
            JObject photo = JObject.Parse(pic);

            if ((int) photo["Id"] == 0)
            {
                var query = "Insert into Photo ";
                query += "(OTop,OLeft,Width,PageNumber,AlbumId,GraphicId) OUTPUT Inserted.Id Values (";
                query += (int) photo["OTop"] + "," + (int) photo["OLeft"] + "," + (int) photo["Width"] + "," +
                         (int) photo["PageNumber"] + "," + (int) photo["AlbumId"] + "," + (int) photo["GraphicId"] + ")";

                var cmd = new SqlCommand(query, conn);
                cmd.Connection.Open();

                try
                {
                    var id = cmd.ExecuteScalar();
                    cmd.Connection.Close();
                    return (int) id;
                }
                catch (SqlException exception)
                {
                    cmd.Connection.Close();
                }
            }
            else
            {
                string query = "Update photo set OTop = " + (int) photo["OTop"] + ", OLeft = " + (int) photo["OLeft"];
                query += ", Width = " + (int) photo["width"] + ", PageNumber = " + (int) photo["PageNUmber"];
                query += ", AlbumId = " + (int) photo["AlbumId"] + ", GraphicId = " + (int) photo["GraphicId"];
                
                var cmd = new SqlCommand(query, conn);
                cmd.Connection.Open();

                try
                {
                    var id = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                }
                catch (SqlException exception)
                {
                    cmd.Connection.Close();
                }
            }

            return -1;
        }

        public void Delete(int Id)
        {
            string query = "delete from photo where Id = " + Id.ToString();
            var cmd = new SqlCommand(query, conn);
            cmd.Connection.Open();

            try
            {
                var id = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (SqlException exception)
            {
                cmd.Connection.Close();
            }

        }

    }
}