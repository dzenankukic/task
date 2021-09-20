using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_V2.Models
{
    public class Rezultati
    {
        public int ID { get; set; }
        public int GradoviID { get; set; }
        public Gradovi Grad { get; set; }
        public int KandidatiID { get; set; }
        public Kandidati Kandidat { get; set; }
        public int BrojGlasova { get; set; }
        public decimal Procenat { get; set; }
        public bool isGreska { get; set; }
    }
}
