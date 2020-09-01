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
    public class Tbl_Evaluacion_Etapa_ProductivaController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Evaluacion_Etapa_Productiva/Details/5
        public ActionResult Details(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva = db.Tbl_Evaluacion_Etapa_Productiva.Find(id);
            if (tbl_Evaluacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // GET: Tbl_Evaluacion_Etapa_Productiva/Create
        public ActionResult Create(int id)
        {
            Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva = new Tbl_Evaluacion_Etapa_Productiva();
            tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz = id;
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // POST: Tbl_Evaluacion_Etapa_Productiva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Evaluacion,Juicio_Evaluacion,Reconocimientos,Id_Aprendiz")] Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz select f.Id_Ficha).First();
                db.Tbl_Evaluacion_Etapa_Productiva.Add(tbl_Evaluacion_Etapa_Productiva);
                db.SaveChanges();
                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
            }
            else { ViewBag.Validar = "Llene todos los campos"; }
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // GET: Tbl_Evaluacion_Etapa_Productiva/Edit/5
        public ActionResult Edit(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva = db.Tbl_Evaluacion_Etapa_Productiva.Find(id);
            tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz = tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz;
            ViewBag.etapa = etapa;
            if (tbl_Evaluacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // POST: Tbl_Evaluacion_Etapa_Productiva/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Evaluacion,Juicio_Evaluacion,Reconocimientos,Id_Aprendiz")] Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva)
        {
            int? etapa = ViewBag.etapa;
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Evaluacion_Etapa_Productiva).State = EntityState.Modified;
                db.SaveChanges();
                return Back(tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz, etapa);
            }
            return View(tbl_Evaluacion_Etapa_Productiva);
        }
        public ActionResult Back(int? Id_Aprendiz, int? etapa)
        {
            return RedirectToAction("Details", "Tbl_Aprendices", new { id = Id_Aprendiz, etapa });
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
