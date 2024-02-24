using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DanceApp1.Models
{
    public class Dancer
    {
        [Key] // This is to confirm that that dancerId is the primary key
        public int dancerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string danceStyle { get; set; }
        public string dancerBio { get; set; }
        public bool DancerHasPic { get; set; }
        public string PicExtension { get; set; }

        // This is a foreign key to the group entity
        // A dancer belongs to a group
        // A group has many dancers
        [ForeignKey("Group")]
        public int groupId { get; set; }
        public virtual Group Group { get; set; }

    }

    public class DancerDto
    {
        public int dancerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string danceStyle { get; set; }
        public string dancerBio { get; set; }
        public string groupName { get; set; }
    }
}