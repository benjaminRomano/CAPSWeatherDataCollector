using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CAPSDataWall.Models;

namespace CAPSDataWall.Controllers
{
    public class Class1Controller : Controller
    {
        private CAPSDataWallContext db = new CAPSDataWallContext();

        // GET: Class1
        public async Task<ActionResult> Index()
        {
            return View(await db.Class1.ToListAsync());
        }

        // GET: Class1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = await db.Class1.FindAsync(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // GET: Class1/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Class1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,name")] Class1 class1)
        {
            if (ModelState.IsValid)
            {
                db.Class1.Add(class1);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(class1);
        }

        // GET: Class1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = await db.Class1.FindAsync(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // POST: Class1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,name")] Class1 class1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(class1).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(class1);
        }

        // GET: Class1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class1 class1 = await db.Class1.FindAsync(id);
            if (class1 == null)
            {
                return HttpNotFound();
            }
            return View(class1);
        }

        // POST: Class1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Class1 class1 = await db.Class1.FindAsync(id);
            db.Class1.Remove(class1);
            await db.SaveChangesAsync();
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
