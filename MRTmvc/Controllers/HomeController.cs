using MRTmvc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;


namespace MRT.Controllers
{
    public class HomeController : Controller
    {
        
        MrtContext db = new MrtContext();

        SessionIDManager manager = new SessionIDManager();
        // string newSessionId = SessionIDManager.CreateSessionID(SessionIDManager.m);

        public ActionResult Index()
        {
            if (Session ["RegiId"] != null) { }
                return View();
            } 

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        }

        public ActionResult ViewHistory()
        {
            //return View(db.Routes.ToList());
            // int userID = Convert.ToInt32(Session["RegId"]);
            //var register = db.Routes.Where(b => b.StoreID == userID).ToList();

            int usersID = Convert.ToInt32(Session["RegId"]);
            var register = db.Routes.Where(b => b.UserID == usersID).ToList();
            //return View(db.Registers.Where(p => RegId.userID(p.RegId).ToList();
            return View(register);

            //int userId = Convert.ToInt32(Session["RegId"]);
            //var Result = (from Route in db.Routes where Route.UserID == userId select Route).ToList();          

            //var result = db.Routes.Select(x => x).ToList().Where(x => x.StoreID == Session["RegId"]);
            //return View(Result);
        }

       public ActionResult Data()
        {

            if (Session["RegId"] != null)
            {
                int userID = Convert.ToInt32(Session["RegId"]);
                var register = db.Registers.Where(b => b.RegId == userID).ToList();
                //return View(db.Registers.Where(p => RegId.userID(p.RegId).ToList();
                return View(register);
            } else
            {
                return RedirectToAction("Index", "Home");
            }

        }



      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Register register = db.Registers.Find(id);
            if (register == null)
            {
                return HttpNotFound();
            }
            return View(register);
        } 
        

        // POST: Registers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegId,Username,Email,Nric,Password1,Password2")] Register register)
        {
            if (ModelState.IsValid)
            {
                db.Entry(register).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(register);
        }


        [HttpGet]
        public ActionResult BuyTicket()
        {

            if (Session["RegId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }



        }


        public ViewResult Register()
        {

            ViewBag.RegId = new SelectList(db.Registers, "RegId", "Username");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "RegId,Username,Email,Nric,Password1,Password2")] Register user)
        {

            if (ModelState.IsValid)
            {
                db.Registers.Add(user);
                db.SaveChanges();
                return RedirectToAction("login");
            }
            ViewBag.RegId = new SelectList(db.Registers, "RegId", "Username", user.RegId);
            return View(user);
        }

        public ViewResult Login()
        {
            if (Session["RegId"] != null)
            {
                return View("BuyTicket", new { Email = Session["Email"].ToString() });
            }
            else
            {
                return View();
            }

        }


        [HttpPost]
        public ActionResult Login(Login login)
        {
            var userLogin = db.Registers.SingleOrDefault(x => x.Email == login.Email && x.Password1 == login.Password1);

            if (userLogin != null)
            {
                if (login.Email == "admin" && login.Password1 == "admin")
                {
                    Session["Email"] = userLogin.Email.ToString();
                    return RedirectToAction("IndexAdmin","Adminn", new { Email = Session["Email"].ToString() });
                }
                else
                {
                    TempData["login"] = new Login();
                    ViewBag.message = "You are succesfulyy login";
                    Session["RegId"] = userLogin.RegId.ToString();
                    Session["Email"] = userLogin.Email.ToString();
                    Session["RegId"] = userLogin.RegId.ToString();
                    Session["Username"] = userLogin.Username.ToString();
                    Session["Nric"] = userLogin.Nric.ToString();

                    return RedirectToAction("BuyTicket", new { Email = Session["RegId"].ToString() });
                }
            }
            else
            {
               
                ModelState.AddModelError("Error", "Email or password not match");
                return View();
                
            }
        }

        [HttpPost]
  
        public ActionResult BuyTicket(Route routeRate)
        {
              if (ModelState.IsValid)
                {

            double[,] rate =
            {{ 0.80, 1.20, 1.80, 2.00, 2.60, 2.70, 3.10, 3.30, 3.20, 3.50, 3.30, 3.40, 3.10, 3.20, 3.30, 3.40, 3.50, 3.60, 3.70, 3.90, 4.00, 4.10, 4.30, 4.50, 4.60, 4.80, 4.80, 5.00, 5.30, 5.40, 5.50 },
             { 1.20, 0.80, 1.50, 1.80, 2.30, 2.70, 2.80, 3.10, 3.40, 3.30, 3.70, 3.30, 3.70, 3.80, 3.20, 3.30, 3.40, 3.50, 3.60, 3.80, 3.90, 4.00, 4.20, 4.40, 4.50, 4.60, 4.70, 4.90, 5.20, 5.20, 5.40 },
             { 1.80, 1.50, 0.80, 1.10, 1.80, 2.10, 2.60, 2.60, 3.00, 3.20, 3.30, 3.50, 3.40, 3.50, 3.60, 3.70, 3.20, 3.30, 3.40, 3.50, 3.60, 3.80, 3.90, 4.20, 4.30, 4.40, 4.50, 4.60, 4.90, 5.00, 5.10 },
             { 2.00, 1.80, 1.10, 0.80, 1.60, 1.90, 2.30, 2.60, 2.80, 3.00, 3.10, 3.30, 3.80, 3.40, 3.40, 3.60, 3.80, 3.20, 3.30, 3.40, 3.50, 3.70, 3.80, 4.00, 4.10, 4.30, 4.40, 4.50, 4.80, 4.00, 5.00 },
             { 2.60, 2.30, 1.80, 1.60, 0.80, 1.30, 1.80, 2.00, 2.40, 2.80, 3.00, 3.20, 3.30, 3.50, 3.60, 3.20, 3.40, 3.60, 3.70, 3.20, 3.20, 3.40, 3.50, 3.70, 3.90, 4.00, 4.10, 4.30, 4.60, 4.60, 4.80 },
             { 2.70, 2.70, 2.10, 1.90, 1.30, 0.80, 1.30, 1.70, 2.00, 2.40, 2.70, 2.90, 3.10, 3.30, 3.40, 3.60, 3.80, 3.40, 3.50, 3.70, 3.80, 3.20, 3.40, 3.60, 3.70, 3.90, 4.00, 4.10, 4.40, 4.50, 4.60 },
             { 3.10, 2.80, 2.60, 2.30, 1.80, 1.30, 0.80, 1.30, 1.70, 2.00, 2.60, 2.80, 3.20, 3.40, 3.10, 3.30, 3.50, 3.70, 3.20, 3.50, 3.60, 3.80, 3.20, 3.40, 3.60, 3.70, 3.80, 3.90, 4.20, 4.30, 4.40 },
             { 3.30, 3.10, 2.60, 2.60, 2.00, 1.70, 1.30, 0.80, 1.30, 1.70, 2.20, 2.50, 2.90, 3.10, 3.20, 3.40, 3.20, 3.40, 3.60, 3.80, 3.30, 3.40, 3.60, 3.80, 3.30, 3.40, 3.60, 3.80, 4.10, 4.20, 4.30 },
             { 3.20, 3.40, 3.00, 2.80, 2.40, 2.00, 1.70, 1.30, 0.80, 1.20, 1.80, 2.10, 2.80, 2.80, 2.90, 3.10, 3.40, 3.10, 3.30, 3.60, 3.70, 3.40, 3.60, 3.80, 3.20, 3.40, 3.50, 3.60, 3.90, 4.00, 4.10 },
             { 3.50, 3.30, 3.20, 3.00, 2.80, 2.40, 2.00, 1.70, 1.20, 0.80, 1.60, 1.80, 2.50, 2.70, 2.60, 2.80, 3.10, 3.30, 3.10, 3.30, 3.50, 3.70, 3.40, 3.60, 3.80, 3.20, 3.30, 3.50, 3.80, 3.90, 4.00 },
             { 3.30, 3.70, 3.30, 3.10, 3.00, 2.70, 2.60, 2.20, 1.80, 1.60, 0.80, 1.10, 1.80, 2.10, 2.20, 2.50, 2.80, 2.80, 3.00, 3.30, 3.50, 3.30, 3.50, 3.30, 3.50, 3.70, 3.80, 3.20, 3.50, 3.60, 3.70 },
             { 3.40, 3.30, 3.50, 3.30, 3.20, 2.90, 2.80, 2.50, 2.10, 1.80, 1.10, 0.80, 1.70, 1.90, 2.00, 2.30, 2.60, 2.60, 2.80, 3.10, 3.30, 3.10, 3.40, 3.70, 3.30, 3.50, 3.60, 3.10, 3.40, 3.50, 3.60 },
             { 3.10, 3.70, 3.40, 3.80, 3.30, 3.10, 3.20, 2.90, 2.80, 2.50, 1.80, 1.70, 0.80, 1.20, 1.30, 1.60, 1.90, 2.10, 2.30, 2.70, 2.70, 3.00, 3.30, 3.20, 3.40, 3.70, 3.20, 3.50, 3.10, 3.20, 3.30 },
             { 3.20, 3.80, 3.50, 3.40, 3.50, 3.30, 3.40, 3.10, 2.80, 2.70, 2.10, 1.90, 1.20, 0.80, 1.00, 1.30, 1.70, 1.80, 2.10, 2.50, 2.70, 2.80, 3.00, 3.50, 3.30, 3.50, 3.60, 3.30, 3.70, 3.80, 3.20 },
             { 3.30, 3.20, 3.60, 3.40, 3.60, 3.40, 3.10, 3.20, 2.90, 2.60, 2.20, 2.00, 1.30, 1.00, 0.80, 1.10, 1.50, 1.80, 1.90, 2.30, 2.50, 2.60, 2.90, 3.30, 3.20, 3.40, 3.50, 3.80, 3.60, 3.70, 3.20 },
             { 3.40, 3.30, 3.70, 3.60, 3.20, 3.60, 3.30, 3.40, 3.10, 2.80, 2.50, 2.30, 1.60, 1.30, 1.10, 0.80, 1.20, 1.50, 1.80, 2.10, 2.30, 2.60, 2.70, 3.10, 3.10, 3.40, 3.20, 3.30, 3.60, 3.50, 3.60 },
             { 3.50, 3.40, 3.20, 3.80, 3.40, 3.80, 3.50, 3.20, 3.40, 3.10, 2.80, 2.60, 1.90, 1.70, 1.50, 1.20, 0.80, 1.10, 1.40, 1.80, 1.90, 2.30, 2.70, 2.90, 3.10, 3.40, 3.10, 3.40, 3.30, 3.40, 3.60 },
             { 3.60, 3.50, 3.30, 3.20, 3.60, 3.40, 3.70, 3.40, 3.10, 3.30, 2.80, 2.60, 2.10, 1.80, 1.80, 1.50, 1.10, 0.80, 1.10, 1.50, 1.80, 2.10, 2.40, 2.60, 2.90, 3.20, 3.30, 3.20, 3.70, 3.30, 3.40 },
             { 3.70, 3.60, 3.40, 3.30, 3.70, 3.50, 3.20, 3.60, 3.30, 3.10, 3.00, 2.80, 2.30, 2.10, 1.90, 1.80, 1.40, 1.10, 0.80, 1.30, 1.50, 1.80, 2.20, 2.70, 2.70, 3.00, 3.20, 3.10, 3.60, 3.70, 3.30 },
             { 3.90, 3.80, 3.50, 3.40, 3.20, 3.70, 3.50, 3.30, 3.60, 3.30, 3.30, 3.10, 2.70, 2.50, 2.30, 2.10, 1.80, 1.50, 1.30, 0.80, 1.10, 1.50, 1.80, 2.30, 2.60, 2.70, 2.80, 3.20, 3.30, 3.20, 3.60 },
             { 4.00, 3.90, 3.60, 3.50, 3.20, 3.80, 3.60, 3.40, 3.70, 3.50, 3.50, 3.30, 2.70, 2.70, 2.50, 2.30, 1.90, 1.80, 1.50, 1.10, 0.80, 1.30, 1.70, 2.10, 2.40, 2.80, 2.70, 3.00, 3.10, 3.30, 3.50 },
             { 4.10, 4.00, 3.80, 3.70, 3.40, 3.20, 3.80, 3.60, 3.40, 3.70, 3.30, 3.10, 3.00, 2.80, 2.60, 2.60, 2.30, 2.10, 1.80, 1.50, 1.30, 0.80, 1.20, 1.80, 2.00, 2.40, 2.60, 2.70, 3.30, 3.40, 3.20 },
             { 4.30, 4.20, 3.90, 3.80, 3.50, 3.40, 3.20, 3.80, 3.60, 3.40, 3.50, 3.40, 3.30, 3.00, 2.90, 2.70, 2.70, 2.40, 2.20, 1.80, 1.70, 1.20, 0.80, 1.40, 1.80, 2.00, 2.20, 2.60, 3.00, 3.10, 3.40 },
             { 4.50, 4.40, 4.10, 4.00, 3.70, 3.60, 3.40, 3.30, 3.80, 3.60, 3.30, 3.70, 3.20, 3.50, 3.30, 3.10, 2.90, 2.60, 2.70, 2.30, 2.10, 1.80, 1.40, 0.80, 1.20, 1.60, 1.80, 2.10, 2.60, 2.70, 3.00 },
             { 4.60, 4.50, 4.30, 4.10, 3.90, 3.70, 3.60, 3.40, 3.20, 3.80, 3.50, 3.30, 3.40, 3.30, 3.20, 3.40, 3.10, 2.90, 2.70, 2.60, 2.40, 2.00, 1.80, 1.20, 0.80, 1.30, 1.50, 1.80, 2.50, 2.70, 2.70 },
             { 4.80, 4.60, 4.40, 4.40, 4.00, 3.90, 3.70, 3.60, 3.40, 3.20, 3.70, 3.50, 3.70, 3.50, 3.40, 3.20, 3.40, 3.20, 3.00, 2.70, 2.80, 2.40, 2.00, 1.60, 1.30, 0.80, 1.10, 1.50, 2.20, 2.30, 2.70 },
             { 4.80, 4.70, 4.50, 4.40, 4.10, 4.00, 3.80, 3.60, 3.50, 3.30, 3.80, 3.60, 3.20, 3.60, 3.50, 3.30, 3.10, 3.30, 3.20, 2.80, 2.70, 2.60, 2.20, 1.80, 1.50, 1.10, 0.80, 1.30, 2.00, 2.20, 2.50 },
             { 5.00, 4.90, 4.60, 4.50, 4.30, 4.10, 3.90, 3.80, 3.60, 3.50, 3.20, 3.10, 3.50, 3.30, 3.80, 3.60, 3.40, 3.20, 3.10, 3.20, 3.00, 2.70, 2.60, 2.10, 1.80, 1.50, 1.30, 0.80, 1.70, 1.80, 2.10 },
             { 5.30, 5.20, 4.90, 4.80, 4.60, 4.40, 4.20, 4.10, 3.90, 3.80, 3.50, 3.40, 3.10, 3.70, 3.60, 3.50, 3.30, 3.70, 3.60, 3.30, 3.10, 3.30, 3.00, 2.60, 2.50, 2.20, 2.00, 1.70, 0.80, 1.10, 1.40 },
             { 5.40, 5.20, 5.00, 4.90, 4.60, 4.50, 4.30, 4.20, 4.00, 3.90, 3.60, 3.50, 3.20, 3.80, 3.70, 3.60, 3.40, 3.30, 3.70, 3.40, 3.30, 3.40, 3.10, 2.70, 2.70, 2.30, 2.20, 1.80, 1.10, 0.80, 1.20 },
             { 5.50, 5.40, 5.10, 5.00, 4.80, 4.60, 4.40, 4.30, 4.10, 4.00, 3.70, 3.60, 3.30, 3.20, 3.20, 3.80, 3.60, 3.40, 3.30, 3.60, 3.50, 3.20, 3.40, 3.00, 2.70, 2.70, 2.50, 2.10, 1.40, 1.20, 0.80 }
            };

            IDictionary<int, string> dictStationFrom = new Dictionary<int, string>()
            {
            {0, "Sungai Buloh" },
            {1, "Kampung Selamat" },
            {2, "Kwasa Damansara" },
            {3, "Kwasan Sentral" },
            {4, "Kota Damansara" },
            {5, "Surian" },
            {6, "Mutiara Damansara" },
            {7, "Bandar Utama" },
            {8, "Taman Tun Dr Ismail" },
            {9, "Phileo Damansara" },
            {10, "Pusat Bandar Damansara" },
            {11, "Semantan" },
            {12, "Muzium Negara" },
            {13, "Pasar Seni" },
            {14, "Merdeka" },
            {15, "Bukit Bintang" },
            {16, "Tun Razak Exchange" },
            {17, "Cochrane" },
            {18, "Maluri" },
            {19, "Taman Pertama" },
            {20, "Taman Midah" },
            {21, "Taman Mutiara" },
            {22, "Taman Connaught" },
            {23, "Taman Suntex" },
            {24, "Sri Raya" },
            {25, "Bandar Tun Hussein Onn" },
            {26, "Batu Sebelas Cheras" },
            {27, "Bukit Dukung" },
            {28, "Sungai Jernih" },
            {29, "Stadium Kajang" },
            {30, "Kajang" }
            };

            IDictionary<int, string> dictStationTo = new Dictionary<int, string>()
            {
            {0, "Sungai Buloh" },
            {1, "Kampung Selamat" },
            {2, "Kwasa Damansara" },
            {3, "Kwasan Sentral" },
            {4, "Kota Damansara" },
            {5, "Surian" },
            {6, "Mutiara Damansara" },
            {7, "Bandar Utama" },
            {8, "Taman Tun Dr Ismail" },
            {9, "Phileo Damansara" },
            {10, "Pusat Bandar Damansara" },
            {11, "Semantan" },
            {12, "Muzium Negara" },
            {13, "Pasar Seni" },
            {14, "Merdeka" },
            {15, "Bukit Bintang" },
            {16, "Tun Razak Exchange" },
            {17, "Cochrane" },
            {18, "Maluri" },
            {19, "Taman Pertama" },
            {20, "Taman Midah" },
            {21, "Taman Mutiara" },
            {22, "Taman Connaught" },
            {23, "Taman Suntex" },
            {24, "Sri Raya" },
            {25, "Bandar Tun Hussein Onn" },
            {26, "Batu Sebelas Cheras" },
            {27, "Bukit Dukung" },
            {28, "Sungai Jernih" },
            {29, "Stadium Kajang" },
            {30, "Kajang" }
            };

                DateTime dateTime = DateTime.Now;
                ViewBag.Rate = rate;
                ViewBag.DictStationFrom = dictStationFrom;
                ViewBag.DictStationTo = dictStationTo;
                ViewBag.DateTime = dateTime.ToString("dd/M/yyyy hh:mm:ss tt");

                ViewBag.Username = Session["Username"];
                ViewBag.Email = Session["Email"];
                ViewBag.Nric = Session["Nric"];
                ViewBag.RegId = Session["RegId"];

                int from = int.Parse(routeRate.StationFrom);
                ViewBag.StationFrom = dictStationFrom[from];

                int to = int.Parse(routeRate.StationTo);
                ViewBag.StationTo = dictStationTo[to];

                int numTicket = int.Parse(routeRate.NumTicket);
                ViewBag.NumTicket = numTicket;

                string LCategory = routeRate.LevelCategory;
                ViewBag.LCategory = LCategory;

                ViewBag.LevelCategory = routeRate.LevelCategory;


                if (routeRate.LevelCategory == "0.50")
                {
                    ViewBag.LCategory = "Senior cetizen";
                }
                else if (routeRate.LevelCategory == "0.40")
                {

                    ViewBag.LCategory = "OKU";
                }
                else if (routeRate.LevelCategory == "0.60")
                {
                    ViewBag.LCategory = "Student";
                }
                else if (routeRate.LevelCategory == "1.00")
                {
                    ViewBag.LCategory = "Normal";
                }
                
                double category = double.Parse(routeRate.LevelCategory);
                ViewBag.DiscountPercent = (100 - (category * 100) + " %");
                    
                double charge = rate[from, to];
                ViewBag.Charge = charge * category * numTicket;
                    
                if (routeRate.tripType == "Round-trip")
                {
                    ViewBag.Charge = ViewBag.Charge * 2;
                }
                else if (routeRate.tripType == "One-way")

                ViewBag.Charge = ViewBag.Charge;
                
                return View("Result",routeRate);

                }
                else
                {
                    return View();
                }
           
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Result(Route route)
        {
            ViewBag.msg = "muhammadamirul5703@gmail.com";
            ViewBag.Username = Session["Username"];
            ViewBag.Nric = Session["Nric"];
            ViewBag.Email = Session["Email"];
            ViewBag.tripType = route.tripType;
            ViewBag.StationFrom = route.StationFrom;
            ViewBag.StationTo = route.StationTo;
            ViewBag.NumTicket = route.NumTicket;
            ViewBag.LCategory = route.LevelCategory;
            ViewBag.DiscountPercent = route.DiscountPercent;
            ViewBag.DateTime = route.DateTime;
            ViewBag.Charge = route.Charge;



            return View(route);
        }







        [HttpPost]
        public async Task<ActionResult> thanks (Route routeRate)
        {


            ViewBag.Username = Session["Username"];
            ViewBag.Nric = Session["Nric"];
            ViewBag.Email = Session["Email"];
            ViewBag.tripType = routeRate.tripType;
            ViewBag.RegId = Session["RegId"];
            ViewBag.StationFrom = routeRate.StationFrom;
            ViewBag.StationTo = routeRate.StationTo;
            ViewBag.NumTicket = routeRate.NumTicket;
            ViewBag.LCategory = routeRate.LevelCategory;
            ViewBag.DiscountPercent = routeRate.DiscountPercent;
            ViewBag.DateTime = routeRate.DateTime;
            ViewBag.Charge = routeRate.Charge;

           //--------------------------------------------------Save Database-----------------------------------------------//

            SqlConnection conn = new SqlConnection(ConfigurationManager.
               ConnectionStrings["MrtContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand("InsertItem", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.Parameters.AddWithValue("@tripType", routeRate.tripType);
            cmd.Parameters.AddWithValue("@StationFrom", ViewBag.StationFrom);
            cmd.Parameters.AddWithValue("@StationTo", ViewBag.StationTo);
            cmd.Parameters.AddWithValue("@DateTime", ViewBag.DateTime);
            cmd.Parameters.AddWithValue("@LevelCategory", ViewBag.LCategory);
            cmd.Parameters.AddWithValue("@NumTicket", ViewBag.NumTicket);
            cmd.Parameters.AddWithValue("@DiscountPercent", ViewBag.DiscountPercent);
            cmd.Parameters.AddWithValue("@Charge", ViewBag.Charge);
            cmd.Parameters.AddWithValue("@UserID", ViewBag.RegId);
            cmd.Parameters.AddWithValue("@Email", ViewBag.Email);
            cmd.Parameters.AddWithValue("@Nric", ViewBag.Nric);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            catch
            {
                return View();
            }
            finally
            {
                conn.Close();
            }



            //---------------------------------------------------Send Email-------------------------------------------------------//


            try
            {

                if (ModelState.IsValid)
                {
                    var body = "<center><h2> Your Purchase Details </h2> </br>" +
                "<table style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " +
                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Name </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.Username + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> NRIC / Passport No </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.Nric + "<td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Email </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.Email + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Trip Type </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.tripType + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> From </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.StationFrom + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> To </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.StationTo + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Passanger category </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.LCategory + "</td>" +
                    "</tr> " +

                     "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Discount price </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.DiscountPercent + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Number of Tickets </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.NumTicket + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Date and Time Purchase </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + ViewBag.DateTime + "</td>" +
                    "</tr> " +

                    "<tr> " +
                        "<td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> Total Price </td><td style='border-collapse: collapse;border: 1px solid black;padding:8px;'> " + "Rm " + ViewBag.Charge + "</td>" +
                    "</tr> " +
                "</table></center>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(ViewBag.Email));  // replace with valid value 
                    message.From = new MailAddress("address");  // replace with valid value
                    message.Subject = "Your email subject";
                    message.Body = string.Format(body, ViewBag.Username, ViewBag.Email);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "username",  // replace with valid value
                            Password = "password"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "host";
                        smtp.Port = 123;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);

                        ViewBag.Message = "Thank You "  + @ViewBag.Username + ", Purchasing are now complete";
                        ViewBag.Message1 = "Please check your email!";
                        return View("ConfirmEmail");
                    }
                }

            } catch
            {
                ViewBag.Message = "Your email fail to send " + @ViewBag.Username + " please try again later";
               
            }


            return View("thanks", routeRate);

        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies["RegId"].Value = string.Empty;
            Response.Cookies["RegId"].Expires = DateTime.Now.AddMonths(-10);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ConfirmEmail()
        {
            return View();
        }



        public ActionResult Detail1(int? id)
        {

            if (id == null)
            {
                Register register = db.Registers.Find(id);
                return View(id);
            }

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}








    