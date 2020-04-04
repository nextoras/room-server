using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sketch_mar21a
{
    public partial class Users
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }

    }
}
