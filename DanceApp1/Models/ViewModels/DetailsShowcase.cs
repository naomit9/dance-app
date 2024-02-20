using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceApp1.Models.ViewModels
{
    public class DetailsShowcase
    {

        public ShowcaseDto SelectedShowcase { get; set; }
        public IEnumerable<GroupDto> ShowcaseGroups { get; set; }
    }
}