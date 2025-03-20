using BackEndDebian.Controller.StaticServic;
using BackEndDebian.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEndDebian.Controller
{
    class DeviceController
    {
        public async static void getDevices(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Device>(db => db.Devices);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void addDevice(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Device? device = JsonSerializer.Deserialize<Device>(json);
                if (device == null)
                {
                    SendResponse(context, "Ошибка: некорректные данные");
                }
                Device? dev = await db.Devices.FirstOrDefaultAsync(u => u.Name == device!.Name);
                if (dev == null)
                {
                    db.Devices.Add(new Device()
                    {
                        Name = device!.Name,
                        Categoryid = device.Categoryid,
                        Manufacturer = device.Manufacturer,
                        Location = device.Location,
                        Description = device.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                SendResponse(context, responseText);
            }
        }
        public async static void delDevice(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Device device = db.Devices.Find(id)!;
                if (device != null)
                {
                    db.Devices.Remove(device);
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                {
                    responseText = "Error";
                }
                SendResponse(context, responseText);
            }
        }
        public async static void updateDevice(string json, HttpListenerContext context)
        {   
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Device? temp = JsonSerializer.Deserialize<Device>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Devices.FindAsync(temp.Deviceid) is Device found)
                    {
                        found.Name = temp.Name;
                        found.Category = temp.Category;
                        found.Manufacturer = temp.Manufacturer;
                        found.Location = temp.Location;
                        found.Description = temp.Description;
                    }
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                SendResponse(context, responseText);
            }
        }
        public async static void SendResponse(HttpListenerContext context, string message)
        {
            var response = context.Response;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            using Stream output = response.OutputStream;
            await output.WriteAsync(buffer);
            await output.FlushAsync();
            Console.WriteLine("Запрос обработан");
        }
    }
}
