using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
        public int AlbumId { get; set; }

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

    }




}