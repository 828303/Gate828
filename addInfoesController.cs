using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using GateBoys.Models;

namespace GateBoys.Controllers
{
    public class addInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: addInfoes
        public ActionResult Index()
        {
            return View(db.addInfoes.ToList());
        }

        // GET: addInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            addInfo addInfo = db.addInfoes.Find(id);
            if (addInfo == null)
            {
                return HttpNotFound();
            }
            return View(addInfo);
        }

        // GET: addInfoes/Create
        public ActionResult Create(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.email = email;
            return View();
        }

        // POST: addInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MoreInfoId,addInfoOf,name,midName,surname,idNum,street_number,route,administrative_area_level_1,locality,country,postal_code,adress,phone,dateRegistered")] addInfo addInfo,string email)
        {
            var mail = User.Identity.GetUserName();
            addInfo.addInfoOf = email;
            addInfo.dateRegistered = DateTime.Now.ToString();
            addInfo.adress = addInfo.addressCMBN();
            if (ModelState.IsValid)
            {
                db.addInfoes.Add(addInfo);
                db.SaveChanges();
                FormsAuthentication.SignOut();
                return RedirectToAction("success", new { id= addInfo.MoreInfoId });
            }

            return View(addInfo);
        }

        public ActionResult success(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userInfo = db.addInfoes.ToList().Where(x => x.MoreInfoId == id).ToList();
            if (userInfo == null)
            {
                return HttpNotFound();
            }

            else if (userInfo.Count < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                foreach (var item in userInfo)
                {
                    ViewBag.naam = item.name+" "+item.midName;
                    ViewBag.snaam = item.surname;
                }
            }
            return View();
        }

        // GET: addInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            addInfo addInfo = db.addInfoes.Find(id);
            if (addInfo == null)
            {
                return HttpNotFound();
            }
            return View(addInfo);
        }

        // POST: addInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MoreInfoId,addInfoOf,name,midName,surname,idNum,street_number,route,administrative_area_level_1,locality,country,postal_code,adress,phone,dateRegistered")] addInfo addInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(addInfo);
        }

        // GET: addInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            addInfo addInfo = db.addInfoes.Find(id);
            if (addInfo == null)
            {
                return HttpNotFound();
            }
            return View(addInfo);
        }

        // POST: addInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            addInfo addInfo = db.addInfoes.Find(id);
            db.addInfoes.Remove(addInfo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
