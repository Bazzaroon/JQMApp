using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQMApp.App.Models;
using JQMApp.Models;

namespace JQMApp.Controllers
{
    public class SiteUserController : Controller
    {
        //
        // GET: /SiteUser/

        public ActionResult Index(int id)
        {
            var userObj = new Users();
            var user = userObj.GetById(id);

            ViewBag.UserEmail = user.Email;

            var graphicItem = new GraphicItem();
            var graphicItems = graphicItem.GetAllForUser(id);

            return View(graphicItems);
        }

    }
}
