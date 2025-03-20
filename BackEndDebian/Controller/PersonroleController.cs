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
    class PersonroleController
    {
        public async static void getPersonRole(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Personrole>(db => db.Personroles);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void addPersonRole(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Personrole? personRoles = JsonSerializer.Deserialize<Personrole>(json);
                if (personRoles == null)
                {
                    SendResponse(context, "Ошибка: некорректные данные");
                }
                Personrole? user = await db.Personroles.FirstOrDefaultAsync(u => u.Personid == personRoles!.Personid && u.Roleid == personRoles!.Roleid);
                if (user == null)
                {
                    db.Personroles.Add(new Personrole()
                    {
                        Personid = personRoles!.Personid,
                        Roleid = personRoles.Roleid
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                SendResponse(context, responseText);
            }
        }
        public async static void delPersonRole(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Personrole personRoles = db.Personroles.Find(id)!;
                if (personRoles != null)
                {
                    db.Personroles.Remove(personRoles);
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
        public async static void updatePersonRole(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Personrole? temp = JsonSerializer.Deserialize<Personrole>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Personroles.FindAsync(temp.Personroleid) is Personrole found)
                    {
                        found.Personid = temp.Personid;
                        found.Roleid = temp.Roleid;
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
