using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DanceApp1.Models;

namespace DanceApp1.Controllers
{
    public class ShowcasesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ShowcasesData
        public IQueryable<Showcase> GetShowcases()
        {
            return db.Showcases;
        }

        // GET: api/ShowcasesData/5
        [ResponseType(typeof(Showcase))]
        public IHttpActionResult GetShowcase(int id)
        {
            Showcase showcase = db.Showcases.Find(id);
            if (showcase == null)
            {
                return NotFound();
            }

            return Ok(showcase);
        }

        // PUT: api/ShowcasesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutShowcase(int id, Showcase showcase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != showcase.showcaseId)
            {
                return BadRequest();
            }

            db.Entry(showcase).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowcaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ShowcasesData
        [ResponseType(typeof(Showcase))]
        public IHttpActionResult PostShowcase(Showcase showcase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Showcases.Add(showcase);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = showcase.showcaseId }, showcase);
        }

        // DELETE: api/ShowcasesData/5
        [ResponseType(typeof(Showcase))]
        public IHttpActionResult DeleteShowcase(int id)
        {
            Showcase showcase = db.Showcases.Find(id);
            if (showcase == null)
            {
                return NotFound();
            }

            db.Showcases.Remove(showcase);
            db.SaveChanges();

            return Ok(showcase);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShowcaseExists(int id)
        {
            return db.Showcases.Count(e => e.showcaseId == id) > 0;
        }
    }
}