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
    class RoleController
    {
        public async static void getRole(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Role>(db => db.Roles);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void getRoleId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Role? role = await db.Roles.FirstOrDefaultAsync(p => p.Roleid == id);
                if (role != null)
                {
                    string jsonPerson = JsonSerializer.Serialize<Role>(role);
                    string responseText = jsonPerson;
                    await DataHendler.SendJsonResponse(context, responseText);
                }
            }
        }
        public async static void addRole(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Role? roles = JsonSerializer.Deserialize<Role>(json);
                if (roles == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Role? user = await db.Roles.FirstOrDefaultAsync(u => u.Rolename == roles!.Rolename);
                if (user == null)
                {
                    db.Roles.Add(new Role()
                    {
                        Rolename = roles!.Rolename,
                        Description = roles.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void delRole(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Role roles = db.Roles.Find(id)!;
                if (roles != null)
                {
                    db.Roles.Remove(roles);
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
        public async static void updateRole(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Role? temp = JsonSerializer.Deserialize<Role>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Roles.FindAsync(temp.Roleid) is Role found)
                    {
                        found.Rolename = temp.Rolename;
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
