using System;
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
    public class Tbl_ProgramasController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Programas
        public ActionResult Index()
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Tbl_Programas.ToList());
        }

        // GET: Tbl_Programas/Create
        public ActionResult Create()
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Tbl_Programas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Programa,Nombre")] Tbl_Programas tbl_Programas)
        {
            if (ModelState.IsValid && tbl_Programas.Nombre != null)
            {
                db.Tbl_Programas.Add(tbl_Programas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Validacion = "Llene todos los campos";
            }

            return View(tbl_Programas);
        }

        // GET: Tbl_Programas/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Programas tbl_Programas = db.Tbl_Programas.Find(id);
            if (tbl_Programas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Programas);
        }

        // POST: Tbl_Programas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Programa,Nombre")] Tbl_Programas tbl_Programas)
        {
            if (ModelState.IsValid && tbl_Programas.Nombre != null)
            {
                db.Entry(tbl_Programas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Validacion = "Llene todos los datos";
            }
            return View(tbl_Programas);
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
