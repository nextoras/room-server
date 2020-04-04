using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using sketch_mar21a.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Globalization;

namespace sketch_mar21a.Controllers
{
    public class HomeController : Controller
    {
        private static weatherContext db = new weatherContext();
        private readonly IHomeService _homeService;
        private static int meteringCount = db.Meterings.Max(u => u.Id);

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }
        public IActionResult Index()
        {
            weatherContext db = new weatherContext();
            var serialPort = new SerialPort("COM3")
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };
            serialPort.Open();
            var reg = new Regex(@"[0-9]{2}\.[0-9]{2}\s[0-9]{2}\.[0-9]{2}");
            //_homeService.SetSecondMetering(serialPort, reg);

            var asd = db.Users.FirstOrDefault();
            // List <UserDevices> userDevices = new List<UserDevices>();
            List<DeviceDTO> deviceDTOs = new List<DeviceDTO>();
            var userDevices = db.UserDevices.Where(d => d.UserId == 0);
            foreach (var userDevice in userDevices)
            {
                var device = db.Devices.Where(d => d.Id == userDevice.DeviceId).FirstOrDefault();
                if (device != null)
                {
                    DeviceDTO deviceDTO = new DeviceDTO()
                    {
                        Id = device.Id,
                        Status = device.Status,
                        Name = device.Name
                    };
                    deviceDTOs.Add(deviceDTO);
                }
            }
            while (true)
            {
                DeviceActivator(serialPort, deviceDTOs);
                string message = serialPort.ReadExisting();
                Console.WriteLine("message is: " + message);
                var match = reg.Match(message);
                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                if (match.Success)
                {
                    string a = message.Substring(0, 5);
                    double humidity = double.Parse(a, formatter);
                    double temperature = double.Parse(message.Substring(6, 5), formatter);

                    List<MeteringWriteDTO> meterings = new List<MeteringWriteDTO>();
                    var meteringTemperature = new MeteringWriteDTO()
                    {
                        Id = meteringCount + 1,
                        Date = DateTime.UtcNow,
                        Value = temperature,
                        SensorId = 0,
                    };
                    meterings.Add(meteringTemperature);
                    //meteringCount += 1;
                    var meteringHumidity = new MeteringWriteDTO()
                    {
                        Id = meteringCount + 1,
                        Date = DateTime.UtcNow,
                        Value = humidity,
                        SensorId = 1,
                    };
                    //meteringCount += 1;
                    meterings.Add(meteringHumidity);
                    SetSecondMetering(meterings);
                    //db.SaveChanges();
                    Console.WriteLine(message + DateTime.UtcNow + '\n');
                    System.Threading.Thread.Sleep(1000);
                }
                if (DateTime.UtcNow.Second == 0)
                {
                    SetMinuteMetering();
                }
                if (DateTime.UtcNow.Minute == 0 && DateTime.UtcNow.Second == 0)
                {
                    SetHourMetering();
                }
                if (DateTime.UtcNow.Hour == 0 && DateTime.UtcNow.Minute == 0 && DateTime.UtcNow.Second == 0)
                {
                    AddDay();
                }
            }
            return View();
        }
        public static void SetSecondMetering(List<MeteringWriteDTO> meterings)
        {
            weatherContext db1 = new weatherContext();
            var countMeteringRows = db1.Meterings.Count();
            foreach (var meteringDTO in meterings)
            {
                meteringCount += 1;
                Meterings metering = new Meterings()
                {
                    Id = meteringCount,
                    SensorId = meteringDTO.SensorId,
                    Date = meteringDTO.Date,
                    Value = meteringDTO.Value,
                    MeteringTypeId = 0
                };
                db.Meterings.Add(metering);
                db.SaveChanges();
                Console.WriteLine(metering.ToString() + '\n' + " second saved" + '\n');
            }
        }

        public static void SetMinuteMetering()
        {
            var userSensors = db.UserSensors.ToList();
            foreach (var userSensor in userSensors)
            {
                var secondsAll = db.Meterings
                .Where(e => e.MeteringTypeId == 0 && e.SensorId == userSensor.SensorId)
                .ToList();
                double average = 0;
                var countMeteringRows = db.Meterings.Count();
                meteringCount += 1;
                foreach (var second in secondsAll)
                {
                    average = average + second.Value;
                }
                Meterings metering = new Meterings()
                {
                    Id = meteringCount,
                    SensorId = userSensor.SensorId,
                    Date = DateTime.UtcNow,
                    Value = average / secondsAll.Count(),
                    MeteringTypeId = 1
                };
                db.Meterings.Add(metering);
                foreach (var second in secondsAll)
                {
                    db.Meterings.Remove(second);
                }
                db.SaveChanges();
                Console.WriteLine(metering.ToString() + '\n' + "minute saved" + '\n');
                System.Threading.Thread.Sleep(1000);
            }

        }

        public static void SetHourMetering()
        {
            var userSensors = db.UserSensors.ToList();
            foreach (var userSensor in userSensors)
            {
                var minutesAll = db.Meterings
                .Where(e => e.MeteringTypeId == 1 && e.SensorId == userSensor.SensorId)
                .ToList();
                double average = 0;
                var countMeteringRows = db.Meterings.Count();
                meteringCount += 1;
                foreach (var second in minutesAll)
                {
                    average = average + second.Value;
                }
                Meterings metering = new Meterings()
                {
                    Id = meteringCount,
                    SensorId = userSensor.SensorId,
                    Date = DateTime.UtcNow,
                    Value = average / minutesAll.Count(),
                    MeteringTypeId = 2
                };
                db.Meterings.Add(metering);
                foreach (var minute in minutesAll)
                {
                    db.Meterings.Remove(minute);
                }
                db.SaveChanges();
                Console.WriteLine(metering.ToString() + '\n' + "hour saved" + '\n');
                System.Threading.Thread.Sleep(1000);
            }

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void DeviceActivator(SerialPort serialPort, List<DeviceDTO> deviceDTOs)
        {
            weatherContext db1 = new weatherContext();
            foreach (var deviceDTO in deviceDTOs)
            {
                var device = db1.Devices.Where(d => d.Id == deviceDTO.Id).FirstOrDefault();
                if (device != null)
                {
                    var tCheck = db1.Meterings.Where(m => m.MeteringTypeId == 0).OrderByDescending(m => m.Date).FirstOrDefault();
                    //if (tCheck != null && tCheck.Value > 29) serialPort.Write("1");
                    //if (tCheck != null && tCheck.Value < 28.9) serialPort.Write("0");
                    if (device.Status != deviceDTO.Status)
                    {
                        if (device.Status == true)
                        {
                            serialPort.Write("1");
                        }
                        else
                        {
                            serialPort.Write("0");
                        }
                        deviceDTO.Status = !deviceDTO.Status;
                    }
                }
            }
        }

        private static void AddMinute()
        {
            var secondsAll = db.Seconds.ToList();
            double averageTemperature = 0;
            double averageHumidity = 0;
            foreach (var second in secondsAll)
            {
                averageTemperature += Math.Round((double)second.Temperature, 2);
                averageHumidity += Math.Round((double)second.Humidity, 2);
            }
            var countMinutesRows = db.Minutes.Count();
            countMinutesRows += 1;
            Minutes minute = new Minutes()
            {
                Id = countMinutesRows,
                Date = DateTime.UtcNow,
                Temperature = Math.Round(averageTemperature / secondsAll.Count(), 2),
                Humidity = Math.Round(averageHumidity / secondsAll.Count(), 2)
            };
            foreach (var second in secondsAll)
            {
                db.Seconds.Remove(second);
            }
            db.Minutes.Add(minute);
            Console.WriteLine("minute was created!  \n");
            db.SaveChanges();
            System.Threading.Thread.Sleep(1000);
        }

        private static void AddHour()
        {
            var minutesAll = db.Minutes.ToList();
            double averageTemperature = 0;
            double averageHumidity = 0;
            foreach (var minuteItem in minutesAll)
            {
                averageTemperature += (double)minuteItem.Temperature;
                averageHumidity += (double)minuteItem.Humidity;
            }
            var countHoursRows = db.Hours.Count();
            countHoursRows += 1;
            Hours hour = new Hours()
            {
                Id = countHoursRows,
                Date = DateTime.UtcNow,
                Temperature = Math.Round(averageTemperature / minutesAll.Count(), 3),
                Humidity = Math.Round(averageHumidity / minutesAll.Count(), 3)
            };
            foreach (var minuteItem in minutesAll)
            {
                db.Minutes.Remove(minuteItem);
            }
            db.Hours.Add(hour);
            Console.WriteLine("hour was created!  \n");
            db.SaveChanges();
            System.Threading.Thread.Sleep(1000);
        }

        private static void AddDay()
        {
            var hoursAll = db.Hours.ToList();
            double averageTemperature = 0;
            double averageHumidity = 0;
            foreach (var hourItem in hoursAll)
            {
                averageTemperature += (double)hourItem.Temperature;
                averageHumidity += (double)hourItem.Humidity;
            }
            var countDaysRows = db.Days.Count();
            countDaysRows += 1;
            Days day = new Days()
            {
                Id = countDaysRows,
                Date = DateTime.UtcNow,
                Temperature = Math.Round(averageTemperature / hoursAll.Count(), 3),
                Humidity = Math.Round(averageHumidity / hoursAll.Count(), 3)
            };
            foreach (var hourItem in hoursAll)
            {
                db.Hours.Remove(hourItem);
            }
            db.Days.Add(day);
            Console.WriteLine("day was created!  \n");
            db.SaveChanges();
            System.Threading.Thread.Sleep(1000);
        }
    }
}
