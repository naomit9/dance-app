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
using System.Diagnostics;
using DanceApp1.Migrations;

namespace DanceApp1.Controllers
{
    public class ShowcaseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/ShowcaseData/ListShowcases
        [HttpGet]
        public IEnumerable<ShowcaseDto> ListShowcases()
        {
            List<Showcase> Showcases = db.Showcases.ToList();
            List<ShowcaseDto> ShowcaseDtos = new List<ShowcaseDto>();

            Showcases.ForEach(s => ShowcaseDtos.Add(new ShowcaseDto()
            {
                showcaseId = s.showcaseId,
                showcaseName = s.showcaseName,
                Date = s.Date,
                showcaseLocation = s.showcaseLocation
            }));

            return ShowcaseDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/ShowcaseData/FindShowcase/5
        [ResponseType(typeof(Showcase))]
        [HttpGet]
        public IHttpActionResult FindShowcase(int id)
        {
            Showcase Showcase = db.Showcases.Find(id);
            ShowcaseDto ShowcaseDto = new ShowcaseDto()
            {
                showcaseId = Showcase.showcaseId,
                showcaseName = Showcase.showcaseName,
                Date = Showcase.Date,
                showcaseLocation = Showcase.showcaseLocation
            };

            if (Showcase == null)
            {
                return NotFound();
            }

            return Ok(ShowcaseDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="showcase"></param>
        /// <returns></returns>
        // POST: api/ShowcaseData/UpdateShowcase/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateShowcase(int id, Showcase showcase)
        {
            Debug.WriteLine("The update showcase method is reached");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != showcase.showcaseId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + showcase.showcaseId);
                Debug.WriteLine("POST parameter" + showcase.showcaseId);
                Debug.WriteLine("POST parameter" + showcase.showcaseId);
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
                    Debug.WriteLine("Showcase not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Debug.WriteLine("None of the conditions triggered");
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="showcase"></param>
        /// <returns></returns>
        // POST: api/ShowcaseData/AddShowcase/5
        [ResponseType(typeof(Showcase))]
        [HttpPost]
        public IHttpActionResult AddShowcase(Showcase showcase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Showcases.Add(showcase);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = showcase.showcaseId }, showcase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/ShowcaseData/DeleteShowcase/5
        [ResponseType(typeof(Showcase))]
        [HttpPost]
        public IHttpActionResult DeleteShowcase(int id)
        {
            Showcase showcase = db.Showcases.Find(id);
            if (showcase == null)
            {
                return NotFound();
            }

            db.Showcases.Remove(showcase);
            db.SaveChanges();

            return Ok();
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