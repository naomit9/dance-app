using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DanceApp1.Models
{
    public class Showcase
    {

        [Key]
        public int showcaseId { get; set; }
        public string showcaseName { get; set; }
        public DateTime Date { get; set; }

        // DateTime myDateTime = DateTime.Now;
        //string formattedDateTime = myDateTime.ToString("dd/MM/yyyy");
        public string showcaseLocation { get; set; }

        // A showcase has many dance groups
        public ICollection<Group> Groups { get; set; }

    }


    public class ShowcaseDto
    {
        public int showcaseId { get; set; }
        public string showcaseName { get; set; }
        public DateTime Date { get; set; }
        public string showcaseLocation { get; set; }
    }
}