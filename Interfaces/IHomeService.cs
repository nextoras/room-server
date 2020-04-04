using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Timers;

namespace sketch_mar21a
{
    public interface IHomeService
    {
        Task SetSecondMetering(SerialPort serialPort, Regex regex);
    }
}
