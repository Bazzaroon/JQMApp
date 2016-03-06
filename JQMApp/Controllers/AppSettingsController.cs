using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JQMApp.App.Models;
using JQMApp.Models;
using Newtonsoft.Json;

namespace JQMApp.Controllers
{
    public class AppSettingsController : Controller
    {
        //
        // GET: /AppSettings/

        public ActionResult Index()
        {

            var data = new AlbumSettings();
            
            return View(data.Get(1000));
        }

        //
        // GET: /AppSettings/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AppSettings/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AppSettings/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AppSettings/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AppSettings/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AppSettings/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AppSettings/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

 
    }
}
