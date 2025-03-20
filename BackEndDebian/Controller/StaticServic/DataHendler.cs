using BackEndDebian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEndDebian.Controller.StaticServic
{
    public static class DataHendler
    {
        public static async Task<string> GetDataAsJson<T>(Func<DbinventoryContext, IEnumerable<T>> dataSelector)
        {
            using (DbinventoryContext db = new DbinventoryContext())
            {
                List<T> data = dataSelector(db).ToList();
                return JsonSerializer.Serialize(data);
            }
        }
        public static async Task SendJsonResponse(HttpListenerContext context, string json)
        {
            string responseText = json;
            SendResponse(context, responseText);
        }
        private async static void SendResponse(HttpListenerContext context, string message)
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
