using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sketch_mar21a
{
    public partial class Devices 
    {
        [Key]
        public int Id {get; set;}
        
        [Required]
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
