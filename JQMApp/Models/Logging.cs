using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace JQMApp.Models
{
    static public class Logging
    {
        private static string path = Directory.GetCurrentDirectory() + "log.txt";

        public static void log(string msg)
        {
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            var cmd = new SqlCommand();
            string query = "insert into Error (ErrorDate, Description) values (@errordate, @description)";
            cmd.CommandText = query;

            cmd.Parameters.Add(new SqlParameter("@errordate", DateTime.Now ));
            cmd.Parameters.Add(new SqlParameter("@description", msg));

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

        }

 }
