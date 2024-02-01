using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DanceApp1.Controllers
{
    public class ShowcaseController : Controller
    {
        // GET: Showcase
        public ActionResult Index()
        {
            return View();
        }

        // GET: Showcase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Showcase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Showcase/Create
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

        // GET: Showcase/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Showcase/Edit/5
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

        // GET: Showcase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Showcase/Delete/5
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
