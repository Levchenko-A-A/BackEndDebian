using System.IdentityModel.Tokens.Jwt;
using System.Net;
using BackEndDebian.Controller;
using BackEndDebian.Model;
//Scaffold-DbContext "Host=193.104.57.148;Port=5432;Database=dbinventory;Username=debianone;Password=toor" Npgsql.EntityFrameworkCore.PostgreSQL

List<JwToken> jwToken = new List<JwToken>();
var jwtService = new JwtService("Cifra39-Cifra39-Cifra39-Cifra39-Cifra39", "BackEndDebian", "FrontClient");
HttpClient httpClient = new HttpClient();
HttpListener server = new HttpListener();
//server.Prefixes.Add("http://193.104.57.148:8080/connection/");
server.Prefixes.Add("http://127.0.0.1:8888/connection/");
server.Start();
while (true)
{
    var context = await server.GetContextAsync();       //ожидает входящие запросы
    var body = context.Request.InputStream;
    var method = context.Request.HttpMethod;
    var encoding = context.Request.ContentEncoding;
    var reader = new StreamReader(body, encoding);
    string query = reader.ReadToEnd();
    string table = context.Request.Headers["table"]!;

    Console.WriteLine($"Received reguest: {context.Request.Url}");
    Console.WriteLine($"Metod: {method}");
    Console.WriteLine($"Table: {table}");
    Console.WriteLine($"Headers[0]: {context.Request.Headers[0]}");
    Console.WriteLine($"Headers[1]: {context.Request.Headers[1]}");
    Console.WriteLine($"QueryString: {context.Request.QueryString}");
    Console.WriteLine($"UserAgent: {context.Request.UserAgent}");
    Console.WriteLine($"HemoteEndPoint: {context.Request.RemoteEndPoint}");
    Console.WriteLine("-------------------------");

    string authHeader = context.Request.Headers["Authorization"]!;
    string token = authHeader?.StartsWith("Bearer ") == true ? authHeader.Substring(7) : null;
    bool requiresAuth = !(method == "POST" && table == "verifyPasswordPerson");

    if (method == "POST" && table == "verifyPasswordPerson")
        PersonController.chekPassword(query, context, jwToken);

    if (jwToken.Contains(context.Request.Headers[0]()))
    if (method == "POST")
    {
        switch (table)
        {
            case "person": PersonController.addPerson(query, context); break;
            case "manufacturer": ManufacturerController.addManufacturer(query, context); break;
            case "category": CategoryController.addCategory(query, context); break;
            case "location": LocationController.addLocation(query, context); break;
            case "role": RoleController.addRole(query, context); break;
            case "personrole": PersonroleController.addPersonRole(query, context); break;
            case "device": DeviceController.addDevice(query, context); break;
            //case "verifyPasswordPerson": PersonController.chekPassword(query, context, jwToken); break;
            //case "ValidateToken": PersonController.validateToken(query, context); break;
        }
    }
    else if (method == "GET")
    {
        switch (table)
        {
            case "person":
                {
                    if (query == "getPersonAll") PersonController.getPerson(context);
                    else PersonController.getPersonId(query, context);
                }
                break;
            case "manufacturer":
                {
                    if (query == "getManufacturerAll") ManufacturerController.getManufacturer(context);
                    else ManufacturerController.getCategoryId(query, context);
                }
                break;
            case "category":
                {
                    if (query == "getCategoryAll") CategoryController.getCategory(context);
                    else CategoryController.getCategoryId(query, context);
                }
                break;
            case "location":
                {
                    if (query == "getLocationAll") LocationController.getLocation(context);
                    else LocationController.getCategoryId(query, context);
                }
                break;
            case "role":
                {
                    if (query == "getRolleAll") RoleController.getRole(context);
                    else RoleController.getRoleId(query, context);
                }
                break;

            case "personrole": PersonroleController.getPersonRole(context); break;
            case "device": DeviceController.getDevices(context); break;
        }
    }
    else if (method == "PUT")
    {
        switch (table)
        {
            case "manufacturer": ManufacturerController.updateManufacturer(query, context); break;
            case "category": CategoryController.updateCategory(query, context); break;
            case "location": LocationController.updateLocation(query, context); break;
            case "role": RoleController.updateRole(query, context); break;
            case "personrole": PersonroleController.updatePersonRole(query, context); break;
            case "device": DeviceController.updateDevice(query, context); break;
        }
    }
    else if (method == "DELETE")
    {
        switch (table)
        {
            case "person": PersonController.delPerson(query, context); break;
            case "manufacturer": ManufacturerController.delManufacturer(query, context); break;
            case "category": CategoryController.delCategory(query, context); break;
            case "location": LocationController.delLocation(query, context); break;
            case "role": RoleController.delRole(query, context); break;
            case "personrole": PersonroleController.delPersonRole(query, context); break;
            case "device": DeviceController.delDevice(query, context); break;
        }
    }
}
//server.Stop();
