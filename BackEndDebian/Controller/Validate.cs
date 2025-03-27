using BackEndDebian.Controller.StaticServic;
using BackEndDebian.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackEndDebian.Controller
{
    class Validate
    {
        public async static void validateToken(string json, HttpListenerContext context, List<JwToken> jwToken)
        {
            string responseText;
            string token = JsonSerializer.Deserialize<string>(json)!;
            if(JwtService.ValidateToken(token, jwToken))
                {
                    responseText = "OK";
                }
            else
                {
                    responseText = "Error";
                }
            await DataHendler.SendJsonResponse(context, responseText);
        }
    }
}
