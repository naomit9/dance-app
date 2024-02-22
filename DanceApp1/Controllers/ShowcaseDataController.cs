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
        /// Returns all the showcases in the system
        /// </summary>
        /// <returns>A list of showcase events</returns>
        /// <example>GET: api/ShowcaseData/ListShowcases</example>
        [HttpGet]
        [ResponseType(typeof(ShowcaseDto))]
        public IHttpActionResult ListShowcases()
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

            return Ok(ShowcaseDtos);
        }


        /// <summary>
        /// Returns a list of showcases that a particular group performed in
        /// </summary>
        /// <param name="id">Group ID</param>
        /// <returns>List of Showcases</returns>
        /// <example>api/ShowcaseData/ListShowcasesForGroup/10</example>
        [HttpGet]
        [ResponseType(typeof(ShowcaseDto))]
        public IHttpActionResult ListShowcasesForGroup(int id)
        {
            List<Showcase> Showcases = db.Showcases.Where(
                s => s.Groups.Any(
                    g => g.groupId == id
                )).ToList();
            List<ShowcaseDto> ShowcaseDtos = new List<ShowcaseDto>();

            Showcases.ForEach(s => ShowcaseDtos.Add(new ShowcaseDto()
            {
                showcaseId = s.showcaseId,
                showcaseName = s.showcaseName,
                Date = s.Date,
                showcaseLocation = s.showcaseLocation
            }));
            return Ok(ShowcaseDtos);
        }



        /// <summary>
        /// Associates a particular group with a particular showcase
        /// </summary>
        /// <param name="showcaseid">Showcase Id</param>
        /// /// <param name="groupId">Group Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// OR HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST curl -d "" -v "https://localhost:44306/api/showcasedata/AssociateShowcaseWithGroup/4/6"
        /// </example>

        [HttpPost]
        [Route("api/showcasedata/AssociateShowcaseWithGroup/{showcaseId}/{groupId}")]
        public IHttpActionResult AssociateShowcaseWithGroup(int showcaseId, int groupId)
        {
            Showcase SelectedShowcase = db.Showcases.Include(s => s.Groups).Where(s => s.showcaseId == showcaseId).FirstOrDefault();
            Group SelectedGroup = db.Groups.Find(groupId);

            if (SelectedShowcase == null || SelectedGroup == null)
            {
                return NotFound();
            }

            SelectedShowcase.Groups.Add(SelectedGroup);
            db.SaveChanges();


            return Ok();
        }



        /// <summary>
        /// Removes an association between a particular group with a particular showcase
        /// </summary>
        /// <param name="showcaseid">Showcase Id</param>
        /// /// <param name="groupId">Group Id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// OR HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST curl -d "" -v https://localhost:44306/api/showcasedata/UnAssociateShowcaseWithGroup/4/6
        /// </example>

        [HttpPost]
        [Route("api/showcasedata/UnAssociateShowcaseWithGroup/{showcaseId}/{groupId}")]
        public IHttpActionResult UnAssociateShowcaseWithGroup(int showcaseId, int groupId)
        {
            Showcase SelectedShowcase = db.Showcases.Include(s => s.Groups).Where(s => s.showcaseId == showcaseId).FirstOrDefault();
            Group SelectedGroup = db.Groups.Find(groupId);

            if (SelectedShowcase == null || SelectedGroup == null)
            {
                return NotFound();
            }

            SelectedShowcase.Groups.Remove(SelectedGroup);
            db.SaveChanges();


            return Ok();
        }





        /// <summary>
        /// Returns one particular showcase
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <returns>Returns info of one particular showcase event</returns>
        /// <example>GET: api/ShowcaseData/FindShowcase/5</example>
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
        /// To send a POST request to the database to update the existing showcase through a json file
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <param name="showcase">Info about the showcase</param>
        /// <returns>Returns new info about the showcase</returns>
        /// <example>POST: api/ShowcaseData/UpdateShowcase/5</example>
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
        /// To send a POST request to the database to create a new showcase to the system
        /// </summary>
        /// <param name="showcase">Info about the showcase</param>
        /// <returns>Returns info about the new showcase</returns>
        /// <example>POST: api/ShowcaseData/AddShowcase/5</example>
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
        /// To send a POST request to the database to delete a showcase by their ID
        /// </summary>
        /// <param name="id">ID of showcase</param>
        /// <returns>The deleted showcase</returns>
        /// <example>DELETE: api/ShowcaseData/DeleteShowcase/5</example>
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