using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HomeQuest.Models
{
    public class Image
    {
        [Key]
        public int Id{ get; set; }
        public string? FileName{ get; set; }
        public string? URL{get; set; }
        public bool? IsPrimaryImage{ get; set; }
        
        public int PropertyId { get; set; }
        public Property Property { get; set; }

    }
}





// public class FileModel
//     {
//         public string FileName { get; set; }
//         public string FileUrl { get; set; }
//     }