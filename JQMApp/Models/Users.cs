using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace JQMApp.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool Administrator { get; set; }
        public int AlbumId { get; set; }
        public string PasswordHash { get; set; }



        public Users Get(string emailaddress, int albumId, string passwordhash)
        {
            var wed = new WeddingData();
            string query = "select * from Users where Email = '" + emailaddress + "' and AlbumId = " + albumId + " and PasswordHash = '" + PasswordHash + "'";
            var user = wed.ExecuteObject<Users>(query);

            return user.FirstOrDefault();
        }

        public void RegisterUser(Users user)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            var cmd = new SqlCommand();
            string query = "insert into users (UserName, Email, PasswordHash, AlbumId) values (@username, @mailaddress, @pw, @albumid)";
            cmd.CommandText = query;

            cmd.Parameters.Add(new SqlParameter("@username", user.UserName));
            cmd.Parameters.Add(new SqlParameter("@mailaddress", user.Email));
            cmd.Parameters.Add(new SqlParameter("@pw", user.PasswordHash));
            cmd.Parameters.Add(new SqlParameter("@albumid", user.AlbumId));

            cmd.Connection = new SqlConnection(connStr);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (SqlException sqlException)
            {
                cmd.Connection.Close();
            }

        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}