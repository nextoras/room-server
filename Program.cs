using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Timers;


namespace sketch_mar21a
{
    public class Program
    {
        private static System.Timers.Timer secondTimer;
        private readonly  weatherContext _db;
        private static int meteringCount;

        public Program(weatherContext db)
        {
            _db = db;
            meteringCount = _db.Meterings.Max(u => u.Id);
        }

        private static System.Timers.Timer minuteTimer;

        private static System.Timers.Timer hourTimer;

        private static System.Timers.Timer dayTimer;

        // private static System.Timers.Timer weekTimer;
        // private static System.Timers.Timer mounthTimer;




        public static void Main(string[] args)
        {
            // SetTimer();

            // Console.WriteLine("\nPress the Enter key to exit the application...\n");
            // Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            // Console.ReadLine();
            // secondTimer.Stop();
            // secondTimer.Dispose();
            // Console.WriteLine("Terminating the application...");
            CreateWebHostBuilder(args).Build().Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        // private static void SetTimer()
        // {
        //     var serialPort = new SerialPort("COM3")
        //     {
        //         BaudRate = 9600,
        //         Parity = Parity.None,
        //         StopBits = StopBits.One,
        //         DataBits = 8,
        //         Handshake = Handshake.None
        //     };
        //     serialPort.Open();
        //     var regex = new Regex(@"[0-9]{2}\.[0-9]{2}\s[0-9]{2}\.[0-9]{2}");

        //     secondTimer = new System.Timers.Timer(3000);
        //     secondTimer.Elapsed += (sender, e) => OnTimedSeconds(sender, e, serialPort, regex);
        //     secondTimer.AutoReset = true;
        //     secondTimer.Enabled = true;

        //     minuteTimer = new System.Timers.Timer(60000);
        //     minuteTimer.Elapsed += (sender, e) => OnTimedMinutes(sender, e);
        //     minuteTimer.AutoReset = true;
        //     minuteTimer.Enabled = true;

        //     // hourTimer = new System.Timers.Timer(360000);
        //     // minuteTimer.Elapsed += (sender, e) => OnTimedHours(sender, e);
        //     // hourTimer.AutoReset = true;
        //     // hourTimer.Enabled = true;

        //     // dayTimer = new System.Timers.Timer(1000);
        //     // minuteTimer.Elapsed += (sender, e) => OnTimedMinutes(sender, e);
        //     // dayTimer.AutoReset = true;
        //     // dayTimer.Enabled = true;

        //     // weekTimer = new System.Timers.Timer(1000);
        //     // minuteTimer.Elapsed += (sender, e) => OnTimedMinutes(sender, e);
        //     // weekTimer.AutoReset = true;
        //     // weekTimer.Enabled = true;

        //     // mounthTimer = new System.Timers.Timer(1000);
        //     // minuteTimer.Elapsed += (sender, e) => OnTimedMinutes(sender, e);
        //     // mounthTimer.AutoReset = true;
        //     // mounthTimer.Enabled = true;
        // }

        // private  static void OnTimedSeconds(Object source, ElapsedEventArgs e, SerialPort serialPort, Regex regex)
        // {
        //     string message = serialPort.ReadExisting();
        //     var match = regex.Match(message);
        //     IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        //     if (match.Success)
        //     {
        //         string a = message.Substring(0, 5);
        //         double humidity = double.Parse(a, formatter);
        //         double temperature = double.Parse(message.Substring(6, 5), formatter);
        //         List<MeteringWriteDTO> meterings = new List<MeteringWriteDTO>();
        //         var meteringTemperature = new MeteringWriteDTO()
        //         {
        //             Id = meteringCount + 1,
        //             Date = DateTime.UtcNow,
        //             Value = temperature,
        //             SensorId = 0,
        //         };
        //         meterings.Add(meteringTemperature);
        //         var meteringHumidity = new MeteringWriteDTO()
        //         {
        //             Id = meteringCount + 1,
        //             Date = DateTime.UtcNow,
        //             Value = humidity,
        //             SensorId = 1,
        //         };
        //         meterings.Add(meteringHumidity);
        //         foreach (var meteringDTO in meterings)
        //         {
        //             meteringCount += 1;
        //             Meterings metering = new Meterings()
        //             {
        //                 Id = meteringCount,
        //                 SensorId = meteringDTO.SensorId,
        //                 Date = meteringDTO.Date,
        //                 Value = meteringDTO.Value,
        //                 MeteringTypeId = 0
        //             };
        //             _db.Meterings.Add(metering);
        //             _db.SaveChanges();
        //             Console.WriteLine(metering.ToString() + '\n' + " second saved" + '\n');
        //         }
        //     }
        // }
        // private static void  OnTimedMinutes(Object source, ElapsedEventArgs e)
        // {
        //     var userSensors = _db.UserSensors.ToList();
        //     foreach (var userSensor in userSensors)
        //     {
        //         var secondsAll = _db.Meterings
        //         .Where(a => a.MeteringTypeId == 0 && a.SensorId == userSensor.SensorId)
        //         .ToList();
        //         double average = 0;
        //         var countMeteringRows = _db.Meterings.Count();
        //         meteringCount += 1;
        //         foreach (var second in secondsAll)
        //         {
        //             average = average + second.Value;
        //         }
        //         Meterings metering = new Meterings()
        //         {
        //             Id = meteringCount,
        //             SensorId = userSensor.SensorId,
        //             Date = DateTime.UtcNow,
        //             Value = average / secondsAll.Count(),
        //             MeteringTypeId = 1
        //         };
        //         _db.Meterings.Add(metering);
        //         foreach (var second in secondsAll)
        //         {
        //             _db.Meterings.Remove(second);
        //         }
        //         _db.SaveChanges();
        //         Console.WriteLine(metering.ToString() + '\n' + "minute saved" + '\n');
        //         // System.Threading.Thread.Sleep(1000);
        //     }
        // }
        // private async Task OnTimedHours(Object source, ElapsedEventArgs e)
        // {
        //     var userSensors = _db.UserSensors.ToList();
        //     foreach (var userSensor in userSensors)
        //     {
        //         var minutesAll = _db.Meterings
        //         .Where(a => a.MeteringTypeId == 1 && a.SensorId == userSensor.SensorId)
        //         .ToList();
        //         double average = 0;
        //         var countMeteringRows = _db.Meterings.Count();
        //         meteringCount += 1;
        //         foreach (var second in minutesAll)
        //         {
        //             average = average + second.Value;
        //         }
        //         Meterings metering = new Meterings()
        //         {
        //             Id = meteringCount,
        //             SensorId = userSensor.SensorId,
        //             Date = DateTime.UtcNow,
        //             Value = average / minutesAll.Count(),
        //             MeteringTypeId = 2
        //         };
        //         _db.Meterings.Add(metering);
        //         foreach (var minute in minutesAll)
        //         {
        //             _db.Meterings.Remove(minute);
        //         }
        //         _db.SaveChanges();
        //         Console.WriteLine(metering.ToString() + '\n' + "hour saved" + '\n');
        //         System.Threading.Thread.Sleep(1000);
        //     }
        // }
    }
}
