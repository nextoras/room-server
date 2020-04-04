using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO.Ports;
using sketch_mar21a.Models;
using sketch_mar21a;

namespace sketch_mar21a.Controllers
{
    public class MainDataController : Controller
    {
        private readonly weatherContext _uow;

        public MainDataController(weatherContext uow)
        {
            _uow = uow;
        }
        [HttpGet("/get")]
        public async Task<string> Post(string token, int type, int value)
        {
            try
            {
                var SerialPort1 = new SerialPort("COM3")
                {
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None
                };
                SerialPort1.DataReceived += SerialPortDataReceived;
                SerialPort1.Open();
                var _continue = true;
                StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
                while (_continue)
                {
                    string message = Console.ReadLine();

                    if (stringComparer.Equals("quit", message))
                    {
                        _continue = false;
                    }
                    else
                    {
                        // SerialPort1.WriteLine(
                        //     String.Format(message));
                        int humidity = int.Parse(message.Substring(0, 4));
                        float temperature = int.Parse(message.Substring(6, 5));
                        var Seconds = new Seconds()
                        {
                            Date = DateTime.Now,
                            Temperature = temperature,
                            Humidity = humidity,
                        };
                        _uow.Seconds.Add(Seconds);
                        _uow.Seconds.Update(Seconds);
                        //_uow.Hour.SaveChanges;
                    }
                }
                return "asd";
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;

            // Read the data that's in the serial buffer.
            var serialdata = serialPort.ReadExisting();

            // Write to debug output.
            Debug.Write(serialdata);
        }
    }
}

