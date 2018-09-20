using Lautech_Bank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lautech_Bank.Controllers
{
    public class AccountController : Controller
    {
        lautechBankEntities1 db = new lautechBankEntities1();
        // GET: /Account/

        public ActionResult Index()
        {
            //var data = db.userdetails.ToList();
            return View();
        }

        [HttpPost]
        
        public ActionResult Index(UserLogin model)
        {
            if(ModelState.IsValid){
                userdetail ud = new userdetail();
                var result = db.userdetails.Where(e => e.AccNo.ToString() == model.Accno && e.password == model.password);
                
                if (result == null)
                {
                    ViewBag.error = "Account number or password incorrect";
                    return View(model);
                }
                else {
                    return View(model);
                }                
             }
                
            return View(model);
                
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }


      

        //
        // GET: /Account/Create

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Create

        [HttpPost]
        public ActionResult Register(registeration model)
        {

            if (ModelState.IsValid)
            {
                if (!model.password.Equals(model.confirmpassword))
                {
                    ViewBag.error = "Password does not match";
                    return View(model);
                }

                var account = db.userdetails.Max(e => e.AccNo);
                if (account == null)
                { account = 100000003; }
                userdetail ud = new userdetail();
                ud.fNmae = model.fNmae;
                ud.lName = model.lName;
                ud.password = model.password;
                ud.AccNo = account + 1;

                this.db.userdetails.Add(ud);
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                return View(model);
            }
        }

        //
        // GET: /Account/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Account/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Account/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
