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
    class ManufacturerController
    {
        public async static void getManufacturer(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Manufacturer>(db => db.Manufacturers);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void addManufacturer(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Manufacturer? manufacturer = JsonSerializer.Deserialize<Manufacturer>(json);
                if (manufacturer == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Manufacturer? user = await db.Manufacturers.FirstOrDefaultAsync(u => u.Name == manufacturer!.Name);
                if (user == null)
                {
                    db.Manufacturers.Add(new Manufacturer()
                    {
                        Name = manufacturer!.Name,
                        Description = manufacturer.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void getCategoryId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Manufacturer? manuf = await db.Manufacturers.FirstOrDefaultAsync(p => p.Manufacturerid == id);
                if (manuf != null)
                {
                    Console.WriteLine(manuf.Name);
                    string jsonPerson = JsonSerializer.Serialize<Manufacturer>(manuf);
                    string responseText = jsonPerson;
                    await DataHendler.SendJsonResponse(context, responseText);
                }
            }
        }
        public async static void delManufacturer(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Manufacturer manufacturer = db.Manufacturers.Find(id)!;
                if (manufacturer != null)
                {
                    db.Manufacturers.Remove(manufacturer);
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
        public async static void updateManufacturer(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Manufacturer? temp = JsonSerializer.Deserialize<Manufacturer>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Manufacturers.FindAsync(temp.Manufacturerid) is Manufacturer found)
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
