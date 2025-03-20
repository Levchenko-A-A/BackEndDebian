using BackEndDebian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackEndDebian.Controller.StaticServic;

namespace BackEndDebian.Controller
{
    class LocationController
    {
        public async static void getLocation(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Location>(db => db.Locations);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void getCategoryId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Location? location = await db.Locations.FirstOrDefaultAsync(p => p.Locationid == id);
                if (location != null)
                {
                    Console.WriteLine(location.Name);
                    string jsonPerson = JsonSerializer.Serialize<Location>(location);
                    string responseText = jsonPerson;
                    await DataHendler.SendJsonResponse(context, responseText);
                }
            }
        }
        public async static void addLocation(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Location? locations = JsonSerializer.Deserialize<Location>(json);
                if (locations == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Location? user = await db.Locations.FirstOrDefaultAsync(u => u.Name == locations!.Name);
                if (user == null)
                {
                    db.Locations.Add(new Location()
                    {
                        Name = locations!.Name,
                        Description = locations.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void delLocation(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Location locations = db.Locations.Find(id)!;
                if (locations != null)
                {
                    db.Locations.Remove(locations);
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
        public async static void updateLocation(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Location? temp = JsonSerializer.Deserialize<Location>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Locations.FindAsync(temp.Locationid) is Location found)
                    {
                        found.Name = temp.Name;
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
