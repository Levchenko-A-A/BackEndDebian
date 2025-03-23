using BackEndDebian.Controller.StaticServic;
using BackEndDebian.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEndDebian.Controller
{
    class PersonController
    {
        public async static void getPerson(HttpListenerContext context)
        {
            string json = await DataHendler.GetDataAsJson<Person>(db => db.Persons);
            await DataHendler.SendJsonResponse(context, json);
        }
        public async static void getPersonId(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Person? person = await db.Persons.FirstOrDefaultAsync(p => p.Personid == id);
                if(person !=null)
                {
                    Console.WriteLine(person.Personname);
                    string jsonPerson = JsonSerializer.Serialize<Person>(person);
                    string responseText = jsonPerson;
                    await DataHendler.SendJsonResponse(context, responseText);
                }
            }
        }
        public async static void addPerson(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Person? person = JsonSerializer.Deserialize<Person>(json);
                if(person==null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Person? user = await db.Persons.FirstOrDefaultAsync(u => u.Personname == person.Personname);
                if (user == null)
                {
                    byte[] salt = GenerateSalt();
                    string hashedPassword = HashPassword(person!.Passwordhash, salt);
                    db.Persons.Add(new Person()
                    {
                        Personname = person.Personname,
                        Passwordhash = hashedPassword,
                        Salt = Convert.ToBase64String(salt)
                    });
                    await db.SaveChangesAsync();
                    responseText = "OK";
                }
                else
                    responseText = "Error";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void delPerson(string json, HttpListenerContext context)
        {
            int id = JsonSerializer.Deserialize<int>(json);
            using (DbinventoryContext db = new DbinventoryContext())
            {
                string responseText;
                Person person = db.Persons.Find(id)!;
                if (person != null)
                {
                    db.Persons.Remove(person);
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
        public async static void UpdatePerson(string json, HttpListenerContext context)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                Person? temp = JsonSerializer.Deserialize<Person>(json);
                if (await db.Persons.FindAsync(temp.Personid) is Person found)
                {
                    found.Personname = temp.Personname;
                    found.Passwordhash = temp.Passwordhash;
                    found.Salt = temp.Salt;
                }
                await db.SaveChangesAsync();
                string responseText = "OK";
                await DataHendler.SendJsonResponse(context, responseText);
            }
        }
        public async static void chekPassword(string json, HttpListenerContext context)
        {
            string answer;
            using (DbinventoryContext db = new DbinventoryContext())
            {
                var jsonUser = JsonSerializer.Deserialize<JsonUser>(json);
                if (jsonUser == null)
                {
                    await DataHendler.SendJsonResponse(context, "Ошибка: некорректные данные");
                }
                Person? user = await db.Persons.FirstAsync(u => u.Personname == jsonUser!.UserName);
                string per = jsonUser!.Password!.ToString();
                string pasHash = user.Passwordhash.ToString();
                string saltHash = user.Salt.ToString();
                bool isPasswordValid = VerifyPassword(per, pasHash, saltHash);
                System.Console.WriteLine(isPasswordValid);
                string userId = user.Personid.ToString();
                answer = isPasswordValid ? userId : "Erorr";
            }
            string jsonTwo = JsonSerializer.Serialize<string>(answer);
            string responseText = jsonTwo;
            await DataHendler.SendJsonResponse(context, responseText);
        }
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static string HashPassword(string password, byte[] salt)
        {
            using(var sha256 = SHA256.Create())
            {
                byte[] passwordByte = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordByte.Length + salt.Length];
                Buffer.BlockCopy(passwordByte, 0, saltedPassword, 0, passwordByte.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordByte.Length, salt.Length);
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashBytes);
            }
        }
        public static bool VerifyPassword(string enteredPassword, string storedHash, string storeSalt)
        {
            byte[] salt = Convert.FromBase64String(storeSalt);
            string hashEnteredPassword = HashPassword(enteredPassword, salt);
            return hashEnteredPassword == storedHash;
        }
    }
}
