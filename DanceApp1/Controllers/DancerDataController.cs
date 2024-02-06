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
using DanceApp1.Migrations;
using DanceApp1.Models;
using System.Diagnostics;

namespace DanceApp1.Controllers
{
    public class DancerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Return all dancers in the system
        /// </summary>
        /// <returns></returns>
        // GET: api/DancerData/ListDancers
        [HttpGet]
        public IEnumerable<DancerDto> ListDancers()
        {
            List<Dancer> Dancers = db.Dancers.ToList();
            List<DancerDto> DancerDtos = new List<DancerDto>();

            Dancers.ForEach(d => DancerDtos.Add(new DancerDto()
            {
                dancerId = d.dancerId,
                firstName = d.firstName,
                lastName = d.lastName,
                danceStyle = d.danceStyle,
                dancerBio = d.dancerBio,
                groupName = d.Group.groupName
            }));

            return DancerDtos;
        }

        /// <summary>
        /// Return infor about all dancers related to a particular group ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: All dancers in the database, including their associated group
        /// </returns>
        /// <param name="id">Group ID</param>
        /// <example>GET: api/DancerData/ListDancersForGroup/5</example>
        [HttpGet]
        public IEnumerable<DancerDto> ListDancersForGroup(int id)
        {
            List<Dancer> Dancers = db.Dancers.Where(d => d.groupId == id).ToList();
            List<DancerDto> DancerDtos = new List<DancerDto>();

            Dancers.ForEach(d => DancerDtos.Add(new DancerDto()
            {
                dancerId = d.dancerId,
                firstName = d.firstName,
                lastName = d.lastName,
                danceStyle = d.danceStyle,
                dancerBio = d.dancerBio,
                groupName = d.Group.groupName
            }));

            return DancerDtos;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/DancerData/FindDancer/5
        [ResponseType(typeof(Dancer))]
        [HttpGet]
        public IHttpActionResult FindDancer(int id)
        {
            Dancer Dancer = db.Dancers.Find(id);
            
            DancerDto DancerDto = new DancerDto()
            {
                dancerId = Dancer.dancerId,
                firstName = Dancer.firstName,
                lastName = Dancer.lastName,
                danceStyle = Dancer.danceStyle,
                dancerBio = Dancer.dancerBio,
                groupName = Dancer.Group?.groupName
            };

            if (Dancer == null)
            {
                return NotFound();
            }

            return Ok(DancerDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dancer"></param>
        /// <returns></returns>
        /// POST: api/DancerData/UpdateDancer/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDancer(int id, Dancer dancer)
        {
            Debug.WriteLine("The update dancer method is reached");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != dancer.dancerId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + dancer.dancerId);
                Debug.WriteLine("POST parameter" + dancer.firstName);
                Debug.WriteLine("POST parameter" + dancer.lastName);
                return BadRequest();
            }

            db.Entry(dancer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DancerExists(id))
                {
                    Debug.WriteLine("Dancer not found");
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
        /// <param name="dancer"></param>
        /// <returns></returns>
        // POST: api/DancerData/AddDancer
        [ResponseType(typeof(Dancer))]
        [HttpPost]
        public IHttpActionResult AddDancer(Dancer dancer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dancers.Add(dancer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = dancer.dancerId }, dancer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: api/DancerData/DeleteDancer/5
        [ResponseType(typeof(Dancer))]
        [HttpPost]
        public IHttpActionResult DeleteDancer(int id)
        {
            Dancer dancer = db.Dancers.Find(id);
            if (dancer == null)
            {
                return NotFound();
            }

            db.Dancers.Remove(dancer);
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

        private bool DancerExists(int id)
        {
            return db.Dancers.Count(e => e.dancerId == id) > 0;
        }
    }
}