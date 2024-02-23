using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace DanceApp1.Models
{
    public class Group
    {
        [Key] //primary key
        public int groupId { get; set; }
        public string groupName { get; set; }
        public string groupStyle { get; set; }
        public string groupBio { get; set; }
        //public bool GroupHasPic { get; set; }
        //public string PicExtension { get; set; }

        // A dance group can participate in many showcases
        public ICollection<Showcase> Showcases { get; set; }

    }

    public class GroupDto
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public string groupStyle { get; set; }
        public string groupBio { get; set; }
    }
}