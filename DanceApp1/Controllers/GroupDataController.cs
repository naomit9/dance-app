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
    public class GroupDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all the groups in the system
        /// </summary>
        /// <returns>Returns a list of group's names linking to their bio</returns>
        /// <example>GET: api/GroupData/ListGroups</example>
        [HttpGet]
        public IEnumerable<GroupDto> ListGroups()
        {
            List<Group> Groups = db.Groups.ToList();
            List<GroupDto> GroupDtos = new List<GroupDto>();

            Groups.ForEach(g => GroupDtos.Add(new GroupDto()
            {
                groupId = g.groupId,
                groupName = g.groupName,
                groupStyle = g.groupStyle,
                groupBio = g.groupBio
            }));
            return GroupDtos;
        }

        /// <summary>
        /// Returns info about a group
        /// </summary>
        /// <param name="id">ID of group</param>
        /// <returns>Returns info about one particular group </returns>
        /// <example>GET: api/GroupData/FindGroup/5</example>
        [ResponseType(typeof(Group))]
        [HttpGet]
        public IHttpActionResult FindGroup(int id)
        {
            Group Group = db.Groups.Find(id);
            GroupDto GroupDto = new GroupDto()
            {
                groupId = Group.groupId,
                groupName = Group.groupName,
                groupStyle = Group.groupStyle,
                groupBio = Group.groupBio
            };

            if (Group == null)
            {
                return NotFound();
            }

            return Ok(GroupDto);
        }

        /// <summary>
        /// To send a POST request to the database to update the existing group through a json file
        /// </summary>
        /// <param name="id">ID of group</param>
        /// <param name="group">Info about the group</param>
        /// <returns>Returns new info about the group</returns>
        /// POST: api/GroupData/UpdateGroup/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateGroup(int id, Group group)
        {
            Debug.WriteLine("The update group method is reached");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id != group.groupId)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + group.groupId);
                Debug.WriteLine("POST parameter" + group.groupId);
                Debug.WriteLine("POST parameter" + group.groupId);
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    Debug.WriteLine("Group not found");
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
        /// To send a POST request to the database to create a new group to the system
        /// </summary>
        /// <param name="group">Info about the group</param>
        /// <returns>Returns info of the new group </returns>
        /// <example>POST: api/GroupData/AddGroup/5</example>
        [ResponseType(typeof(Group))]
        [HttpPost]
        public IHttpActionResult AddGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = group.groupId }, group);
        }

        /// <summary>
        /// To send a POST request to the database to delete a group by their ID
        /// </summary>
        /// <param name="id">ID of group</param>
        /// <returns>The deleted group</returns>
        /// <example>DELETE: api/GroupData/DeleteGroup/5</example>
        [ResponseType(typeof(Group))]
        [HttpPost]
        public IHttpActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
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

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.groupId == id) > 0;
        }
    }
}