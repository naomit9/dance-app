using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanceApp1.Models.ViewModels
{
    public class UpdateDancer
    {
        // This viewmodel is a class which stores info that we need to present to Dancer/Update/{}

        // The existing dancer info
        public Dancer selecteddancer {  get; set; }

        // All groups to choose from when updating this dancer
        public IEnumerable<GroupDto> GroupOptions { get; set; }
    }
}