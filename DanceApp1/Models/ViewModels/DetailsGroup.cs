using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceApp1.Models.ViewModels
{
    public class DetailsGroup
    {
        public GroupDto SelectedGroup { get; set; }
        public IEnumerable<DancerDto> RelatedDancers { get; set;}
        public IEnumerable<ShowcaseDto> ShowcaseAttended { get; set; }

    }
}