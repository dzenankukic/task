using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using task_V2.Models;

namespace task_V2.ViewModels
{
    public class RezultatiPrikazVM
    {
        public int RezultatID { get; set; }
        public int GradID { get; set; }
        public List<Rezultati> Rezultati { get; set; }
    }
}
