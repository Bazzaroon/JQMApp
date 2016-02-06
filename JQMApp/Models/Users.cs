using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebGrease.Activities;

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

        readonly string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();

        public Users Get(string emailaddress, int albumId, string passwordhash)
        {
            var wed = new WeddingData();
            string query = "select * from Users where Email = '" + emailaddress + "' and AlbumId = " + albumId + " and PasswordHash = '" + PasswordHash + "'";
            var user = wed.ExecuteObject<Users>(query);

            return user.FirstOrDefault();
        }

        public void Update(string _user)
        {
            JObject user = JObject.Parse(_user);

            var cmd = new SqlCommand();
            string query = "update users set UserName = @username, Email = @email, Administrator = @administrator, Active = @active";
            query += " where Id = @userId";
            cmd.CommandText = query;

            cmd.Parameters.Add(new SqlParameter("@username", user["UserName"].ToString()));
            cmd.Parameters.Add(new SqlParameter("@email", user["Email"].ToString()));
            cmd.Parameters.Add(new SqlParameter("@administrator", (bool)user["Administrator"]));
            cmd.Parameters.Add(new SqlParameter("@active", (bool)user["Active"]));
            cmd.Parameters.Add(new SqlParameter("@userId", (int)user["Id"]));
            
            cmd.Connection = new SqlConnection(_connStr);

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

        public void RegisterUser(Users user)
        {
            var cmd = new SqlCommand();
            string query = "insert into users (UserName, Email, PasswordHash, AlbumId) values (@username, @mailaddress, @pw, @albumid)";
            cmd.CommandText = query;
            
            Logging.log(query);
            
            cmd.Parameters.Add(new SqlParameter("@username", user.UserName));
            cmd.Parameters.Add(new SqlParameter("@mailaddress", user.Email));
            cmd.Parameters.Add(new SqlParameter("@pw", user.PasswordHash));
            cmd.Parameters.Add(new SqlParameter("@albumid", user.AlbumId));

            cmd.Connection = new SqlConnection(_connStr);

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (SqlException sqlException)
            {
                Logging.log(sqlException.Message);
                cmd.Connection.Close();
            }

        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Users GetById(int id)
        {
            string query = "select * from Users where Id = " + id.ToString();
            var data = new WeddingData();
            return data.ExecuteObject<Users>(query).FirstOrDefault();
        }
    }

}