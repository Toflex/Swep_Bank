using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lautech_Bank.Models.MyModels;
using Lautech_Bank.Models;
using System.Collections;

namespace Lautech_Bank.Controllers
{
    public class UserController : Controller
    {
        private Lautech_BankContext db = new Lautech_BankContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                List<Userdetail> ud = db.Userdetails.Where(e => e.accNo.Equals(model.Accno) && e.password.Equals(model.password)).ToList();
                if (ud.Count != 1)
                {
                    ViewBag.message = "Account number or password incorrect";
                    return View(model);
                }
                else
                    return RedirectToAction("Home");
            }
            return View(model);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(registeration userLog)
        {
            if (ModelState.IsValid)
            {         
                if(!userLog.confirmpassword.Equals(userLog.password)){
                     ViewBag.message = "Password does not match";
                    return View(userLog);
                }
                String accId;
            var user = db.Userdetails.OrderByDescending(t => t.accNo).FirstOrDefault();                
                if (user == null)
                {
                    accId = "0000000001";
                }
                else
                {                    
                    accId = (Convert.ToInt32(user.accNo)+1).ToString();
                }               
                var amount = 0.00;
                Userdetail userId = new Userdetail();
                userId.accNo = accId;
                userId.amount = amount;
                userId.fname = userLog.fName;
                userId.lName = userLog.lName;
                userId.Email = userLog.Email;
                userId.password = userLog.password;
                userId.accountType = userLog.accountType;

                db.Userdetails.Add(userId);
                db.SaveChanges();
                Session["logedUser"] = accId;
                return RedirectToAction("Index");
            }

            return View(userLog);
        }

        //=================================================After Logging in======================================================//

        public ActionResult Transaction()
        {
            return View(db.Transactions.ToList());
        }

        public ActionResult Home()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Home( [Bind (Include="withname")] Withdrawal WD)
        {
            if (ModelState.IsValid)
            {

                var res = db.Userdetails.Where(e => e.accNo.Equals(Session["logedUser"].ToString())).FirstOrDefault();
                
                try{
                  double wd =  Convert.ToDouble(WD.withdraw);
                  double currentBal = res.amount;
                  if (currentBal >= wd)
                  {
                      double newBal = Math.Round(currentBal - res.amount,2);
                      res.amount = newBal;
                      db.Entry(res).State = EntityState.Modified;

                      Transactions tran = new Transactions();
                      tran.amount = newBal.ToString();
                      tran.trans_date = DateTime.Now;
                      db.Transactions.Add(tran);
                      db.SaveChanges();
                      return View();
                  }
                }
                catch{
                    ViewBag.message = "Please enter a valid Value";
                        return View(WD);
                }                
            }
            return View(WD);
        }


        //=================================================After Logging in======================================================//


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }   
}