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
using Lautech_Bank.Models.Account;
using System.Data.SqlClient;

namespace Lautech_Bank.Controllers
{
    public class UserController : Controller
    {
        private Lautech_BankContext db = new Lautech_BankContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            Intrest();
            return View();
        }


        [HttpPost]
        public ActionResult Index([Bind(Include="Accno,password")] UserLogin model)
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
                {
                    Session["logedUser"] = model.Accno;
                    return RedirectToAction("Home");
                }
            }
            return View(model);
        }

        //
        // GET: /User/Create

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Register([Bind(Include="fName,lName,password,confirmpassword,Email,accountType")] registeration userLog)
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
                    while (accId.Length < 10)
                    {
                        accId = accId.Insert(0, "0");
                    }
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

        public ActionResult Logout() {
            Session["logedUser"] = null;
            return RedirectToAction("Index");
        }

        //=================================================After Logging in======================================================//

        public ActionResult Transaction()
        {
            if (Session["logedUser"] == null)
            {
                return RedirectToAction("Index");
            }
            String accno =  Session["logedUser"].ToString();
            var list = db.Transactions.Where(a => a.accNo.Equals(accno)).ToList();
            return View(list);
        }

        public ActionResult Home()
        {
            if (Session["logedUser"] != null)
            {
                String accno = Session["logedUser"].ToString();
                var bal = db.Userdetails.Where(a => a.accNo.Equals(accno)).FirstOrDefault();
                Session["balance"] = bal.amount;
                return View();
            }
             return RedirectToAction("Index");
        }


        
        [HttpPost]
        public ActionResult Home( [Bind (Include="withdraw")] Withdrawal WD)
        {
           
            if (ModelState.IsValid)
            {
               String accno =  Session["logedUser"].ToString();

                var res = db.Userdetails.Where(e => e.accNo.Equals(accno)).FirstOrDefault();
                
                try{
                  double wd =  Convert.ToDouble(WD.withdraw);
                  double currentBal = res.amount;
                  if (currentBal >= wd && wd > 0)
                  {
                      double newBal = Math.Round(currentBal - wd, 2);
                      res.amount = newBal;
                      db.Entry(res).State = EntityState.Modified;

                      Transactions tran = new Transactions();
                      tran.amount = -wd;
                      tran.accNo = accno;
                      tran.trans_date = DateTime.Now;
                      tran.trans_detail = "Withdraw";
                      db.Transactions.Add(tran);
                      db.SaveChanges();
                      ViewBag.message = "Transaction Successful.";
                      return View();
                  }
                  else {
                      ViewBag.message = "Transaction Failed. Insufficient Balance";
                      return View(WD);
                  }
                }
                catch{
                    ViewBag.message = "Transaction Failed. Please enter a valid value";
                        return View(WD);
                }                
            }
            ViewBag.message = "Please enter the amount";
            return View();
        }


        public ActionResult Deposit() {
            if (Session["logedUser"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Deposit([Bind(Include="withdraw")] Withdrawal WD)
        {
            if (ModelState.IsValid)
            {
                try
                {
                        double wd = Convert.ToDouble(WD.withdraw);
                        String accno = Session["logedUser"].ToString();
                        var res = db.Userdetails.Where(e => e.accNo.Equals(accno)).FirstOrDefault();
                        res.amount += wd;

                        db.Entry(res).State = EntityState.Modified;

                        Transactions tran = new Transactions();
                        tran.amount = wd;
                        tran.accNo = accno;
                        tran.trans_date = DateTime.Now;
                        tran.trans_detail = "Deposit";
                        db.Transactions.Add(tran);
                        db.SaveChanges();

                        ViewBag.message = "Transaction Successful.";
                        return View();                                      
                }
                catch
                {
                    ViewBag.message = "Transaction Failed. Please enter a valid value";
                    return View(WD);
                }                
            }
            ViewBag.message = "Please enter the amount";
            return View();
        }


        public ActionResult Loan() {
            Session["loan"] = Properties.Settings.Default.LoanIntrest;
            string accno = Session["logedUser"].ToString();
            var data = db.Loans.Where(a => a.accNo.Equals(accno)).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Loan([Bind(Include="amount,duration")] LoanClass lc)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    double amount = Convert.ToDouble(lc.amount);
                    int duration = Convert.ToInt32(lc.duration);

                    Loan ln = new Loan();
                    ln.accNo = Session["logedUser"].ToString();
                    ln.amount = amount;
                    ln.duration = duration;
                    ln.loanDate = DateTime.Now;
                    ln.Pending = true;

                    db.Loans.Add(ln);
                    db.SaveChanges();
                    ViewBag.success = "Loan request has been submitted.";

                    string accno = Session["logedUser"].ToString();
                    var data = db.Loans.Where(a => a.accNo.Equals(accno)).ToList();
                    return View(data);
                }
                catch {
                    ViewBag.message = "Enter valid values.";
                    return View(lc);
                }
            }
            @ViewBag.amount = "Enter amount";
            return View(lc);
        }

        //=================================================After Logging in======================================================//


        private int excess = 0;
        private int increment = 0;
        private int diff = 0;
        private DateTime currentDate;
        private DateTime startDate;        

        SqlConnection con = new SqlConnection(@"Data Source=(localdb)\v11.0; Initial Catalog=Lautech_BankContext-20180920125143; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename=|DataDirectory|Lautech_BankContext-20180920125143.mdf");

        public void Intrest()
        {
            if(Session["sd"] == null)
            {
                con.Open();                    
                SqlCommand cmd = new SqlCommand("select startdate from banklogics  where id = 1",con);
                Session["sd"] = cmd.ExecuteScalar();
                con.Close();
            }
            currentDate = DateTime.Now.Date;
            startDate = Convert.ToDateTime(Session["sd"].ToString());
            if (currentDate >= (startDate.AddDays(30)))
            {
                diff = (int)Math.Floor((currentDate.Subtract(startDate).TotalDays));
                increment = diff / 30;
                excess = diff % 30;
                increment *= Properties.Settings.Default.PercentageIntrest;
              
                con.Open();
                SqlCommand cmd = new SqlCommand("select accNo,sum(amount) from Transactions where trans > CONVERT(datetime, '"
                    + startDate + "' and dates < CONVERT(datetime, '" + startDate.AddDays(diff - excess) + "') group by name", con);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var res = db.Userdetails.Where(e => e.accNo.Equals(dr.GetValue(0).ToString())).FirstOrDefault();

                    res.amount = dr.GetDouble(1);

                    db.Entry(res).State = EntityState.Modified;

                    db.SaveChanges();

                }
                con.Close();

                var sd = db.BankLogics.Where(e => e.Id.Equals(1)).FirstOrDefault();
                sd.startdate = startDate;
                db.Entry(sd).State = EntityState.Modified;
                db.SaveChanges();

            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }   
}