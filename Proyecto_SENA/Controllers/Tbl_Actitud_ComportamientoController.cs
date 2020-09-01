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
    public class Tbl_Actitud_ComportamientoController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Actitud_Comportamiento/Details/5
        public ActionResult Details(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento = db.Tbl_Actitud_Comportamiento.Find(id);
            if (tbl_Actitud_Comportamiento == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Actitud_Comportamiento);
        }

        // GET: Tbl_Actitud_Comportamiento/Create
        public ActionResult Create(int id, int etapa)
        {
            Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento = new Tbl_Actitud_Comportamiento();
            tbl_Actitud_Comportamiento.Id_Aprendiz = id;
            tbl_Actitud_Comportamiento.Tipo_Informe = etapa;
            return View(tbl_Actitud_Comportamiento);
        }

        // POST: Tbl_Actitud_Comportamiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Actitud,Tipo_Informe,Relaciones_Interpersonales,Valoracion_Relaciones,TrabajoEnEquipo,Valoracion_Trabajo,Solucion_De_Problemas,Valoracion_Solucion,Cumplimiento,Valoracion_Cumplimiento,Organizacion,Valoracion_Organizacion,Id_Aprendiz")] Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Actitud_Comportamiento.Id_Aprendiz select f.Id_Ficha).First();

                db.Tbl_Actitud_Comportamiento.Add(tbl_Actitud_Comportamiento);
                db.SaveChanges();

                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
            }
            else { ViewBag.Validar = "Llene todos los campos"; }
            return View(tbl_Actitud_Comportamiento);
        }

        // GET: Tbl_Actitud_Comportamiento/Edit/5
        public ActionResult Edit(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento = db.Tbl_Actitud_Comportamiento.Find(id);
            tbl_Actitud_Comportamiento.Tipo_Informe = etapa;
            tbl_Actitud_Comportamiento.Id_Aprendiz = tbl_Actitud_Comportamiento.Id_Aprendiz;
            if (tbl_Actitud_Comportamiento == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Actitud_Comportamiento);
        }

        // POST: Tbl_Actitud_Comportamiento/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Actitud,Tipo_Informe,Periodo_Evaluado,Relaciones_Interpersonales,Valoracion_Relaciones,TrabajoEnEquipo,Valoracion_Trabajo,Solucion_De_Problemas,Valoracion_Solucion,Cumplimiento,Valoracion_Cumplimiento,Organizacion,Valoracion_Organizacion,Id_Aprendiz")] Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Actitud_Comportamiento).State = EntityState.Modified;
                db.SaveChanges();
                return Back(tbl_Actitud_Comportamiento.Id_Aprendiz,tbl_Actitud_Comportamiento.Tipo_Informe);
            }
            return View(tbl_Actitud_Comportamiento);
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
