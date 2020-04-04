using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sketch_mar21a
{
    public partial class MeteringWriteDTO 
    {        
        public int Id { get; set; }
        public int SensorId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public int MeteringTypeId { get; set; }
    }
}
