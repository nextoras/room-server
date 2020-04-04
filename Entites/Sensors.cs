using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sketch_mar21a
{
    public partial class Sensors 
    {
        [Key]
        public int Id {get; set;}
        
        [Required]
        public string Name { get; set; }
        public string unit { get; set; }
    }
}
