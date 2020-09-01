﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_SENA.Models;

namespace Proyecto_SENA.Controllers
{
    [Authorize]
    public class Tbl_VisitasController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Visitas
        public ActionResult Index()
        {

            if (Session["Rol"].ToString() != "1" && Session["Rol"].ToString() != "2")
            {
                return RedirectToAction("Index", "Home");
            }

            if (Session["Rol"].ToString() == "1")
            {
                var tbl_Visitas = db.Tbl_Visitas.Include(t => t.Tbl_Fichas).Include(t => t.Tbl_Instructores);
                return View(tbl_Visitas.ToList());
            }
            else
            {
                string Id_Usuario = Convert.ToString(Session["Id_Usuario"]);

                var instructor = (from i in db.Tbl_Instructores
                                  where i.Id_Usuario == Id_Usuario
                                  select i.Numero_Identificacion).First();

                var tbl_Visitas = from v in db.Tbl_Visitas
                                  where v.Id_Instructor == instructor
                                  select v;
                return View(tbl_Visitas.ToList());
            }
        }

        // GET: Tbl_Visitas/Details/5
        public ActionResult Details(int? id)
        {

            if (Session["Rol"].ToString() != "1" && Session["Rol"].ToString() != "2")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Visitas tbl_Visitas = db.Tbl_Visitas.Find(id);
            if (tbl_Visitas == null)
            {
                return HttpNotFound();
            }

            int? ficha = tbl_Visitas.Numero_Ficha;
            return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
        }

        // GET: Tbl_Visitas/Create
        public ActionResult Create()
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Numero_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha");
            ViewBag.Id_Instructor = new SelectList(db.Tbl_Instructores, "Id_Instructor", "Nombres");

            //Necesio retornar los nombres y apellidos del instructor
            var lst = (from l in db.Tbl_Instructores
                       select new { identificacion = l.Numero_Identificacion, Nombres = l.Nombres + " " + l.Apellidos }).ToList();

            //Convierto la consulta a SelectListItem que es el tipo con el que trabaja el DropDownList
            List<SelectListItem> Id_Instructor = lst.ConvertAll(l =>
            {
                return new SelectListItem()
                {
                    Text = l.Nombres,
                    Value = l.identificacion.ToString()
                };
            });
            //genero el viewbag
            ViewBag.Id_Instructor = Id_Instructor;
            return View();
        }

        // POST: Tbl_Visitas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Visita,Id_Instructor,Numero_Ficha")] Tbl_Visitas tbl_Visitas)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Visitas.Add(tbl_Visitas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Numero_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Id_Ficha", tbl_Visitas.Numero_Ficha);
            ViewBag.Id_Instructor = new SelectList(db.Tbl_Instructores, "Id_Instructor", "Numero_Identificacion", tbl_Visitas.Id_Instructor);
            return View(tbl_Visitas);
        }

        // GET: Tbl_Visitas/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Visitas tbl_Visitas = db.Tbl_Visitas.Find(id);
            if (tbl_Visitas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Numero_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha", tbl_Visitas.Numero_Ficha);
            ViewBag.Id_Instructor = new SelectList(db.Tbl_Instructores, "Numero_Identificacion", "Nombres");

            //Necesio retornar los nombres y apellidos del instructor
            var lst = (from l in db.Tbl_Instructores
                       select new { identificacion = l.Numero_Identificacion, Nombres = l.Nombres + " " + l.Apellidos }).ToList();

            //Convierto la consulta a SelectListItem que es el tipo con el que trabaja el DropDownList
            List<SelectListItem> Id_Instructor = lst.ConvertAll(l =>
            {
                return new SelectListItem()
                {
                    Text = l.Nombres,
                    Value = l.identificacion.ToString()
                };
            });
            //genero el viewbag
            ViewBag.Id_Instructor = Id_Instructor;
            return View(tbl_Visitas);
        }

        // POST: Tbl_Visitas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Visita,Id_Instructor,Numero_Ficha")] Tbl_Visitas tbl_Visitas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Visitas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Numero_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Id_Ficha", tbl_Visitas.Numero_Ficha);
            ViewBag.Id_Instructor = new SelectList(db.Tbl_Instructores, "Id_Instructor", "Numero_Identificacion", tbl_Visitas.Id_Instructor);
            return View(tbl_Visitas);
        }

        // GET: Tbl_Visitas/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Visitas tbl_Visitas = db.Tbl_Visitas.Find(id);
            if (tbl_Visitas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Visitas);
        }

        // POST: Tbl_Visitas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Visitas tbl_Visitas = db.Tbl_Visitas.Find(id);
            db.Tbl_Visitas.Remove(tbl_Visitas);
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
