﻿using BackEndDebian.Controller.StaticServic;
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
            using (DbinventoryContext db = new DbinventoryContext())
            {
                List<Device> devices = db.Devices.ToList();
                string json = JsonSerializer.Serialize<List<Device>>(devices);
                string responseText = json;
                DataHendler.SendResponse(context, responseText);
            }
        }
        public async static void addDevice(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Device? device = JsonSerializer.Deserialize<Device>(json);
                if (device == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Device? dev = await db.Devices.FirstOrDefaultAsync(u => u.Name == device!.Name);
                if (dev == null)
                {
                    db.Devices.Add(new Device()
                    {
                        Name = device!.Name,
                        Categoryid = device.Categoryid,
                        Manufacturerid = device.Manufacturerid,
                        Locationid = device.Locationid,
                        Description = device.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
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
                await DataHendler.SendJsonResponse(context, responseText);
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
                        found.Categoryid = temp.Categoryid;
                        found.Manufacturerid = temp.Manufacturerid;
                        found.Locationid = temp.Locationid;
                        found.Description = temp.Description;
                    }
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
    }
}
