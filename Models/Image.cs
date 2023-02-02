using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQuest.Models
{
    public class Image
    {
        public int Id{ get; set; }
        public string URL{get; set; }
        public int Order{ get; set; }
        public DateTime CreatedDate{ get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }

    }
}