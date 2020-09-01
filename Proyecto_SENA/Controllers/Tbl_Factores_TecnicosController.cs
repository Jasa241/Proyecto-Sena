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
    public class Tbl_Factores_TecnicosController : Controller
    {
        private practicas3Entities db = new practicas3Entities();
        // GET: Tbl_Factores_Tecnicos/Details/5
        public ActionResult Details(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Factores_Tecnicos tbl_Factores_Tecnicos = db.Tbl_Factores_Tecnicos.Find(id);
            if (tbl_Factores_Tecnicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Factores_Tecnicos);
        }

        // GET: Tbl_Factores_Tecnicos/Create
        public ActionResult Create(int id, int etapa)
        {
            Tbl_Factores_Tecnicos tbl_Factores_Tecnicos = new Tbl_Factores_Tecnicos();
            tbl_Factores_Tecnicos.Id_Aprendiz = id;
            tbl_Factores_Tecnicos.Tipo_Informe = etapa;
            return View(tbl_Factores_Tecnicos);
        }

        // POST: Tbl_Factores_Tecnicos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Factores_Tecnicos,Tipo_Informe,Transferencia_Conocimiento,Valoracion_Conocimiento,Mejora_Continua,Valoracion_Mejora,Fortalecimiento_Ocupacional,Valoracion_Fortalecimiento,Oportunidad_Calidad,Valoracion_Oportunidad,Responsabilidad_Ambiental,Valoracion_Ambiental,Administracion_Recursos,Valoracion_Administracion,Seguridad_Ocupacional,Valoracion_Seguridad,Documentacion_Etapa_Productiva,Valoracion_Etapa_Productiva,Observacion_Jefe,Observacion_Aprendiz,Id_Aprendiz")] Tbl_Factores_Tecnicos tbl_Factores_Tecnicos)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Factores_Tecnicos.Id_Aprendiz select f.Id_Ficha).First();
                db.Tbl_Factores_Tecnicos.Add(tbl_Factores_Tecnicos);
                db.SaveChanges();
                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
            }
            else { ViewBag.Validar = "Llene todos los campos"; }
            return View(tbl_Factores_Tecnicos);
        }

        // GET: Tbl_Factores_Tecnicos/Edit/5
        public ActionResult Edit(int? id, int? etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Factores_Tecnicos tbl_Factores_Tecnicos = db.Tbl_Factores_Tecnicos.Find(id);
            tbl_Factores_Tecnicos.Id_Aprendiz = tbl_Factores_Tecnicos.Id_Aprendiz;
            tbl_Factores_Tecnicos.Tipo_Informe = etapa;
            if (tbl_Factores_Tecnicos == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Factores_Tecnicos);
        }

        // POST: Tbl_Factores_Tecnicos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Factores_Tecnicos,Tipo_Informe,Periodo_Evaluado,Transferencia_Conocimiento,Valoracion_Conocimiento,Mejora_Continua,Valoracion_Mejora,Fortalecimiento_Ocupacional,Valoracion_Fortalecimiento,Oportunidad_Calidad,Valoracion_Oportunidad,Responsabilidad_Ambiental,Valoracion_Ambiental,Administracion_Recursos,Valoracion_Administracion,Seguridad_Ocupacional,Valoracion_Seguridad,Documentacion_Etapa_Productiva,Valoracion_Etapa_Productiva,Observacion_Jefe,Observacion_Aprendiz,Id_Aprendiz")] Tbl_Factores_Tecnicos tbl_Factores_Tecnicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Factores_Tecnicos).State = EntityState.Modified;
                db.SaveChanges();
                return Back(tbl_Factores_Tecnicos.Id_Aprendiz,tbl_Factores_Tecnicos.Tipo_Informe);
            }
            return View(tbl_Factores_Tecnicos);
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
