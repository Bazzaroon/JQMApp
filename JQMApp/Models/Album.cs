using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding.Binders;
using Newtonsoft.Json.Linq;

namespace JQMApp.App.Models
{
    public class Album
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }
        public int PageCount { get; set; }
        public int Slides { get; set; }
        public string CoverImage { get; set; }
        public bool StopUsers { get; set; }

        public void AddPage(int id)
        {
            using (SqlConnection con = new SqlConnection(SqlDataConnection.Connect()))
            {
                using (SqlCommand cmd = new SqlCommand("AddPage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@AlbumId", SqlDbType.Int).Value = id;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemovePage()
        {
            
        }

        public void Update(string album)
        {
            JObject _album = JObject.Parse(album);
            string query = "update album set Slides = @slides, CoverImage = @coverimage, StopUsers = @stopusers where Id = @id";

            using (SqlConnection con = new SqlConnection(SqlDataConnection.Connect()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@id", (int) _album["Id"]);
                    cmd.Parameters.Add("@slides", (int) _album["Slides"]);
                    cmd.Parameters.Add("@coverimage", _album["CoverImage"].ToString());
                    cmd.Parameters.Add("@stopusers", (bool) _album["StopUsers"]);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception)
                    {
                        con.Close();
                    }
                }
            }
        }

        public void UpdateHomePage(string fileName, int albumId)
        {
            string query = "update album set CoverImage = @coverimage where Id = @albumid";
            using (SqlConnection con = new SqlConnection(SqlDataConnection.Connect()))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@coverimage", fileName);
                    cmd.Parameters.Add("@albumid", albumId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception)
                    {
                        con.Close();
                    }

                }
            }
        }

    }




}