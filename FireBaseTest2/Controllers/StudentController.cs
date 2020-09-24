using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FireBaseTest2.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FireBaseTest2.Controllers
{
    public class StudentController : Controller
    {
        //config object gives permission for configurations/data manipulation
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Q8WjRRQtLN6J8xprcIvGAnLhgat1JyzXVeF5kJaz",
            BasePath = "https://fir-test-1a7f2.firebaseio.com/" //locates database location
        };

        IFirebaseClient db; //Firebase database object

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _StudentPartial()
        {
            /* - Access firebase services through FireSharp API
               - FirebaseClient is an class inside firebase 
               - A FirebaseClient class object (client) allows 
                 a third-party to utilise firebase services or utilise 
                 FirebaseClient public attributes/methods*/
            db = new FireSharp.FirebaseClient(config); 
            FirebaseResponse response = db.Get("Students");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Person>();
            foreach(var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Person>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        public PartialViewResult All()
        {
            db = new FireSharp.FirebaseClient(config); 
            FirebaseResponse response = db.Get("Students");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Person>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Person>(((JProperty)item).Value.ToString()));
            }

            return PartialView("_StudentPartial", list);
        }

        public string Top3()
        {
            return "Ajax is Working for TOP 3";
        }

        public string Bot3()
        {
            return "Ajax is Working for BOT 3";
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person s)
        {
            try
            {
                AddStudentToFirebase(s);
                ModelState.AddModelError(string.Empty, "Student added successfully");
            }catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View();
        }

        private void AddStudentToFirebase(Person s)
        {
            db = new FireSharp.FirebaseClient(config);
            var data = s; //contains a student object/row
            PushResponse response = db.Push("Students/", data); //pushes student object in Student table/path

            //Generate an automatic/default firebase id for the record
            data.id = response.Result.name;
            SetResponse setRes = db.Set("Students/" + data.id, data);
        }
    }
}