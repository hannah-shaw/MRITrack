using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MRITrack.Models;

namespace MRITrack.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private Model1 db = new Model1();
        private ApplicationDbContext identityDB = new ApplicationDbContext();

        // GET: Appointments
        [AllowAnonymous]
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.Doctors).Include(a => a.Users);
            return View(appointments.ToList());
        }

        // GET: Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "FirstName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Time,UserId,DoctorId")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "FirstName", appointments.DoctorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", appointments.UserId);
            return View(appointments);
        }

        // GET: Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "FirstName", appointments.DoctorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", appointments.UserId);
            return View(appointments);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Time,UserId,DoctorId")] Appointments appointments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorId = new SelectList(db.Doctors, "Id", "FirstName", appointments.DoctorId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", appointments.UserId);
            return View(appointments);
        }

        [HttpPost]
        public String CreateAppointment()
        {
            var userId = User.Identity.GetUserId();
            var userQuery = db.Users.Where(u => u.UserId == userId);
            var user = userQuery.FirstOrDefault();

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(userId).FirstOrDefault();

            var appointment = new Appointments();
            if(roles == "Patient") {
                appointment = new Appointments
                {
                    UserId = user.Id,
                    DoctorId = int.Parse(Request.Form["DoctorID"]),
                    Date = DateTime.Now,
                    Time = Request.Form["AppointmentDate"]
                };
            }
            else
            {
                appointment = new Appointments
                {
                    UserId = int.Parse(Request.Form["PatientID"]),
                    DoctorId = int.Parse(Request.Form["DoctorID"]),
                    Date = DateTime.Now,
                    Time = Request.Form["AppointmentDate"]
                };
            }


           
            /*
            var vx = Request.Files["Attachment"].ContentLength;
            
            // Store the attachment in local storage.
            var Str1 = Request.Files[0].FileName.Split('.');
            var FileType = Str1[Str1.Length - 1];
            var FilePath =
                Server.MapPath("~/Uploads/") +
                string.Format(@"{0}", Guid.NewGuid()) +
                "." + FileType;
            Request.Files[0].SaveAs(FilePath);
            */
            var doctor = db.Appointments.Where(s => s.DoctorId == appointment.DoctorId).ToList();
            var dates = db.Appointments.Where(s => s.Time == appointment.Time).ToList();
            if (doctor.Intersect(dates).Any())
            {
                return "appointment conflict!";
            }

            if (ModelState.IsValid)
            {
                // Add the appointment into the database.
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return "Success";
                /*
                // Send confirmation email.
                var mail = new MailMessage();
                mail.To.Add(new MailAddress(Request.Form["EmailAddress"]));
                mail.From = new MailAddress("Monash-fit5032-2023-T3@outlook.com");

                mail.Subject = "Appointment Conformation";
                mail.Body =
                    "You made an appointment:\n" +
                    "User ID: " + Request.Form["PatientID"] + "\n" +
                    "Doctor ID: " + Request.Form["DoctorID"] + "\n" +
                    "Date: " + Request.Form["AppointmentDate"];
                mail.IsBodyHtml = false;

                var attachment = new System.Net.Mail.Attachment(FilePath);
                mail.Attachments.Add(attachment);

                var smtp = new SmtpClient();
                smtp.Host = "smtp.office365.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential
                    ("Monash-fit5032-2023-T3@outlook.com", "Monash2023#");

                smtp.Send(mail);
                return "Success";
                */
            }

            return "Database Unavailable.";
        }


        // GET: Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointments appointments = db.Appointments.Find(id);
            if (appointments == null)
            {
                return HttpNotFound();
            }
            return View(appointments);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointments appointments = db.Appointments.Find(id);
            db.Appointments.Remove(appointments);
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
