using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DanceApp1.Controllers
{
    public class DancerController : Controller
    {
        // GET: Dancer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Dancer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dancer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dancer/Create
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

        // GET: Dancer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dancer/Edit/5
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

        // GET: Dancer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dancer/Delete/5
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
