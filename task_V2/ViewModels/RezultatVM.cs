using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_V2.ViewModels
{
    public class RezultatVM
    {
        public string GradID { get; set; }
        //public string NazivGrada { get; set; }
        public List<SelectListItem> gradovi { get; set; }
        public bool prikaz { get; set; }

    }
}
