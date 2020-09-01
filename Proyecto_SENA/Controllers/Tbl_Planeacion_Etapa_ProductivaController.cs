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
    public class Tbl_Planeacion_Etapa_ProductivaController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Planeacion_Etapa_Productiva/Details/5
        public ActionResult Details(int? id, int etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int idp = 0;
            var planeacion = from p in db.Tbl_Planeacion_Etapa_Productiva
                          select p;
            foreach (var p in planeacion)
            {
                if (p.Id_Aprendiz == id)
                    idp = p.Id_Planeacion;
            }
            Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva = db.Tbl_Planeacion_Etapa_Productiva.Find(idp);
            if (tbl_Planeacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // GET: Tbl_Planeacion_Etapa_Productiva/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Planeacion_Etapa_Productiva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Planeacion,Actividades_A_Desarrollar,Evidencias_Aprendizaje,Fecha,Lugar,Observaciones,Id_Aprendiz")] Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva)
        {
            if (ModelState.IsValid && tbl_Planeacion_Etapa_Productiva.Actividades_A_Desarrollar != null && tbl_Planeacion_Etapa_Productiva.Evidencias_Aprendizaje != null && tbl_Planeacion_Etapa_Productiva.Lugar != null && tbl_Planeacion_Etapa_Productiva.Observaciones != null && tbl_Planeacion_Etapa_Productiva.Id_Aprendiz != null)
            {
                int ida = 0;
                int? ficha = 0;
                var aprendiz = from a in db.Tbl_Aprendices select a;

                foreach (var a in aprendiz)
                {
                    if (a.Numero_Identificacion ==tbl_Planeacion_Etapa_Productiva.Id_Aprendiz)
                        ida = a.Numero_Identificacion;
                        ficha = a.Id_Ficha;
                }
                tbl_Planeacion_Etapa_Productiva.Id_Aprendiz = ida;
                db.Tbl_Planeacion_Etapa_Productiva.Add(tbl_Planeacion_Etapa_Productiva);
                db.SaveChanges();

                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
            }
            else { ViewBag.Validar = "Llene todos los campos"; }

            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // GET: Tbl_Planeacion_Etapa_Productiva/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva = db.Tbl_Planeacion_Etapa_Productiva.Find(id);
            if (tbl_Planeacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // POST: Tbl_Planeacion_Etapa_Productiva/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Planeacion,Actividades_A_Desarrollar,Evidencias_Aprendizaje,Fecha,Lugar,Observaciones,Id_Aprendiz")] Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva)
        {
            if (ModelState.IsValid)
            {
                int? ida = 0;
                int? idf = 0;
                var planeacion = from p in db.Tbl_Planeacion_Etapa_Productiva where p.Id_Planeacion == tbl_Planeacion_Etapa_Productiva.Id_Planeacion select p.Id_Aprendiz;

                foreach (var p in planeacion)
                {
                    ida = p;
                }

                tbl_Planeacion_Etapa_Productiva.Id_Aprendiz = ida;
                db.Entry(tbl_Planeacion_Etapa_Productiva).State = EntityState.Modified;
                db.SaveChanges();

                var ficha = from f in db.Tbl_Aprendices where f.Numero_Identificacion == ida select f.Id_Ficha;
                foreach (var f in ficha)
                {
                    idf = f;
                }

                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha = idf });
            }
           
            return View(tbl_Planeacion_Etapa_Productiva);
        }
        public ActionResult Back(int id, int etapa)
        {
            return RedirectToAction("Details", "Tbl_Aprendices", new { id, etapa });
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
