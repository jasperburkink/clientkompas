using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Diagnosis
    {
        public int Id { get; set; }

        public virtual Client Client { get; set; }

        public string Name { get; set; }
    }
}
