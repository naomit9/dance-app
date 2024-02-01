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
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/GroupData/ListGroups
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/GroupData/FindGroup/5
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        // POST: api/GroupData/UpdateGroup/5
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
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        // POST: api/GroupData/AddGroup/5
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

        // DELETE: api/GroupData/DeleteGroup/5
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