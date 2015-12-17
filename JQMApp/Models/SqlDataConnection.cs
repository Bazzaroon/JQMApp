using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JQMApp.App.Models
{
    static public class SqlDataConnection
    {
        static public string Connect()
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["weddingconnection"].ToString();
            return conn;
        }
    }
}