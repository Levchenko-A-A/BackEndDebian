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
    class CategoryController
    {
        public async static void getCategory(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Category>(db => db.Categories);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void getCategoryId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Category? category = await db.Categories.FirstOrDefaultAsync(p => p.Categoryid == id);
                if (category != null)
                {
                    Console.WriteLine(category.Name);
                    string jsonPerson = JsonSerializer.Serialize<Category>(category);
                    string responseText = jsonPerson;
                    await DataHendler.SendJsonResponse(context, responseText);
                }
            }
        }
        public async static void addCategory(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Category? categories = JsonSerializer.Deserialize<Category>(json);
                if (categories == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Category? user = await db.Categories.FirstOrDefaultAsync(u => u.Name == categories!.Name);
                if (user == null)
                {
                    db.Categories.Add(new Category()
                    {
                        Name = categories!.Name,
                        Description = categories.Description
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void delCategory(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Category categories = db.Categories.Find(id)!;
                if (categories != null)
                {
                    db.Categories.Remove(categories);
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
        public async static void updateCategory(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Category? temp = JsonSerializer.Deserialize<Category>(json);
                if (temp == null)
                {
                    responseText = "error";
                }
                else
                {
                    if (await db.Categories.FindAsync(temp.Categoryid) is Category found)
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
