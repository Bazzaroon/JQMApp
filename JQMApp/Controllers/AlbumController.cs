using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQMApp.App.Models;

namespace JQMApp.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/

        public ActionResult Index()
        {
            var graphicItem = new GraphicItem();
            var graphicItems = graphicItem.GetAllForUser(0);

            return View(graphicItems);
        }

    }
}
