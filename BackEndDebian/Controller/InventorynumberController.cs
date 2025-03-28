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
    internal class InventorynumberController
    {
        public async static void getInventorynumber(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Person>(db => db.Persons);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void getInventorynumberId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Inventorynumber? inventorynumber = await db.Inventorynumbers.FirstOrDefaultAsync(i => i.Deviceid == id);
                if (inventorynumber != null)
                {
                    string jsonInvent = JsonSerializer.Serialize<Inventorynumber>(inventorynumber);
                    await DataHendler.SendJsonResponse(context, jsonInvent);
                }
            }
        }
    }
}
