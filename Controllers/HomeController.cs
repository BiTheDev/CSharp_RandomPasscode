using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RandomPasscode.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace RandomPasscode.Controllers
{
    public static class SessionExtensions
{
    // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        // This helper function simply serializes theobject to JSON and stores it as a string in session
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
       
    // generic type T is a stand-in indicating that we need to specify the type on retrieval
    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        string value = session.GetString(key);
        // Upon retrieval the object is deserialized based on the type we specified
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}

    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("Count") == null){
                HttpContext.Session.SetInt32("Count", 0);
                int? PasscodeCount = HttpContext.Session.GetInt32("Count");
                ViewBag.counter = PasscodeCount;
                return View("index", TempData["passcode"]);
                
            }else{
                int? PasscodeCount = HttpContext.Session.GetInt32("Count");
                ViewBag.counter = PasscodeCount;
                return View("index", TempData["passcode"]);
            }
        }
        [Route("generate")]
        public IActionResult Generate(){
            Random rand = new Random();
            char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            string passcode = "";
            for (int i = 0; i < 15 ;i++)
            {
                int choice = rand.Next(Chars.Length);
                passcode += Chars[choice];
            }
            TempData["passcode"] = passcode;
            int? IntVariable = HttpContext.Session.GetInt32("Count");
            int count = IntVariable.GetValueOrDefault() +1;
            HttpContext.Session.SetInt32("Count", count);
            return RedirectToAction("Index");
        }

    }
}
