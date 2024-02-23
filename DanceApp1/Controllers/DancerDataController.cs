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
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.IO;
using System.Web;

namespace DanceApp1.Controllers
{
    public class DancerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Return all dancers in the system
        /// </summary>
        /// <returns>A list of dancer's first name and last name that links to their bio page</returns>
        /// <example>GET: api/DancerData/ListDancers</example>
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
        /// Return info about all dancers related to a particular group ID
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
        /// To retrieve info about one particular dancer by passing in their dancer ID
        /// </summary>
        /// <param name="id">ID of the dancer</param>
        /// <returns>Info about one particular dancer</returns>
        /// <example>GET: api/DancerData/FindDancer/5</example>
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
        /// To send a POST request to the database to update the existing dancer through a json file
        /// </summary>
        /// <param name="id">ID of the dancer</param>
        /// <param name="dancer">Info of dancer including first and last name, dance style and their grouop ID</param>
        /// <returns>New info of the dancer</returns>
        /// <example>POST: api/DancerData/UpdateDancer/5</example>
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

            // Picture update is handled by another method
            db.Entry(dancer).Property(d => d.DancerHasPic).IsModified = false;
            db.Entry(dancer).Property(d => d.PicExtension).IsModified = false;
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


        [HttpPost]
        public IHttpActionResult UploadDancerPic(int id)
        {
            // Check if the request contains multipart form data
            if (Request.Content.IsMimeMultipartContent())
            {
                // Get the file from the request
                var dancerPic = HttpContext.Current.Request.Files[0];

                // Check if the file is not null and has content
                if (dancerPic != null && dancerPic.ContentLength > 0)
                {
                    try
                    {
                        // Define the directory path to save the file
                        string directoryPath = HttpContext.Current.Server.MapPath("~/Content/Images/Dancers/");

                        // Create the directory if it doesn't exist
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Generate a unique file name based on the dancer ID and file extension
                        string fileName = id + Path.GetExtension(dancerPic.FileName);

                        // Combine the directory path and file name to get the full file path
                        string filePath = Path.Combine(directoryPath, fileName);

                        // Save the file to the server
                        dancerPic.SaveAs(filePath);

                        // Update the dancer record in the database with the file path
                        Dancer selectedDancer = db.Dancers.Find(id);
                        selectedDancer.PicExtension = filePath; // Assuming DancerPicPath is a property in the Dancer model
                        db.Entry(selectedDancer).State = EntityState.Modified;
                        db.SaveChanges();

                        return Ok(filePath); // Return the file path if needed
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to save dancer image: " + ex.Message);
                        return InternalServerError(ex);
                    }
                }
                else
                {
                    return BadRequest("No file uploaded or file is empty.");
                }
            }
            else
            {
                return BadRequest("Invalid request format.");
            }
        }




        /// <summary>
        /// To send a POST request to the database to create a new dancer to the system
        /// </summary>
        /// <param name="dancer">Info about the dancer</param>
        /// <returns>Info of the new dancer being created</returns>
        /// <example>POST: api/DancerData/AddDancer</example>
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
        /// To send a POST request to the database to delete a dancer by their ID
        /// </summary>
        /// <param name="id">ID of dancer</param>
        /// <returns>Returns the dancer that has been deleted</returns>
        /// <example>POST: api/DancerData/DeleteDancer/5</example>
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