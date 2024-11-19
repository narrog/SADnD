using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class NotePerson : Note
    {
        public string Location { get; set; }
        public string Affiliation { get; set; }
    }
}
