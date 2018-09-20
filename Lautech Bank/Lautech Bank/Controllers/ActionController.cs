using Lautech_Bank.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Lautech_Bank.Controllers
{
    public class ActionController : Controller
    {
        lautechBankEntities1 db = new lautechBankEntities1();      

        // GET: /Action/

        public ActionResult Index()
        {           

            return View();
        }

        // POST: /Action/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(userdetail model)
        {
            userdetail ud = new userdetail();
            ud.AccNo = model.AccNo;
            //ud.
            return View();
        }


        public ActionResult Register()
        {

            return View();
        }


        // GET: /Action/Home        users
        public ActionResult Home() {
            var bal = db.Account_balance.Where(s => s.AccNo == 100000003);
            return View(bal.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult Home(string amount)
        {            
            int Amount = Convert.ToInt32(amount);
            var query = db.Account_balance.Where(s => s.AccNo == 100000003 && s.amount >= Amount);
            
            return View();
        }

     

    }
}
