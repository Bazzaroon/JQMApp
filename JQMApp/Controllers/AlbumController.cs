using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQMApp.App.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JQMApp.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/

        public ActionResult Index(int slideCount)
        {
            
            var graphicItem = new GraphicItem();
            
            var graphicItems = graphicItem.GetAllForUser(0, slideCount);

            return View(graphicItems);
        }

    }
}
