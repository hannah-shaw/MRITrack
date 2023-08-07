using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MRITrack.Models;

namespace MRITrack.Controllers
{
    public class CommentsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Appointments);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Comments/Create
        [Authorize(Roles = "Patient,Admin")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var loggedInUser = db.Users.FirstOrDefault(u => u.UserId == userId);
            var loggedInUserAppointmentList = db.Appointments.Where(u => u.UserId == loggedInUser.Id).ToList();
            ViewBag.AppointmentId = new SelectList(loggedInUserAppointmentList, "Id", "DoctorId");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient,Admin")]
        public ActionResult Create([Bind(Include = "Id,Star,AppointmentId")] Comments comments)
        {
            var userId = User.Identity.GetUserId();
            var loggedInUser = db.Users.FirstOrDefault(u => u.UserId == userId);
            var loggedInUserAppointmentList = db.Appointments.Where(u => u.UserId == loggedInUser.Id).ToList();
            var commentIds = new List<int>();

            foreach (var appointment in loggedInUserAppointmentList)
            {
                var comment = db.Comments.FirstOrDefault(u => u.AppointmentId == appointment.Id);

                if (comment != null)
                {
                    commentIds.Add(comment.Id);
                }
            }

            if (ModelState.IsValid && loggedInUserAppointmentList.Any(a => a.Id == comments.AppointmentId))
            {
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentId = new SelectList(loggedInUserAppointmentList, "Id", "DoctorId", comments.AppointmentId);
            return View(comments);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "Id", "Time", comments.AppointmentId);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Star,AppointmentId")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentId = new SelectList(db.Appointments, "Id", "Time", comments.AppointmentId);
            return View(comments);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.Comments.Find(id);
            db.Comments.Remove(comments);
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
