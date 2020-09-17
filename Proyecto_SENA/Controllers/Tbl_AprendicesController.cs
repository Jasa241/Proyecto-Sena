using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.WebSockets;
using Antlr.Runtime;
using Proyecto_SENA.Models;

namespace Proyecto_SENA.Controllers
{
    [Authorize]
    public class Tbl_AprendicesController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Aprendices
        public ActionResult Index(int? ficha)
        {
            if (Session["Rol"].ToString() != "1" && Session["Rol"].ToString() != "2")
            {
                return RedirectToAction("Index", "Home");
            }

            var Aprendices = (from Aprendiz in db.Tbl_Aprendices
                              where ficha == Aprendiz.Id_Ficha
                              select Aprendiz).ToList();
            return View(Aprendices);
        }

        // GET: Tbl_Aprendices/Details/5
        public ActionResult Details(int? id, int etapa)
        {

            if (Session["Rol"].ToString() != "1" && Session["Rol"].ToString() != "2")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }

            ViewBag.etapa = etapa;
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Create
        public ActionResult Create()
        {
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha");
            return View();
        }

        // POST: Tbl_Aprendices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Ficha,Telefono,Correo,Id_Empresa")] Tbl_Aprendices tbl_Aprendices)
        {
            if (ModelState.IsValid && tbl_Aprendices.Nombres != null && tbl_Aprendices.Apellidos != null && tbl_Aprendices.Telefono != null && tbl_Aprendices.Correo != null)
            {
                if (Metodos.Numeros(Convert.ToString(tbl_Aprendices.Numero_Identificacion)) && Metodos.Cadena(tbl_Aprendices.Nombres) && Metodos.Cadena(tbl_Aprendices.Apellidos) && Metodos.Numeros(tbl_Aprendices.Telefono))
                {
                    db.Tbl_Aprendices.Add(tbl_Aprendices);
                    db.SaveChanges();
                    return RedirectToAction("Create", "Tbl_Empresas");
                }
                else { ViewBag.Validar = "Llene los campos con el formato correcto"; }
            }
            else { ViewBag.Validar = "Llene todos los campos"; }

            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha", tbl_Aprendices.Id_Ficha);
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Edit/5
        public ActionResult Edit(int? id)
        {

            if (Session["Rol"].ToString() != "1" && Session["Rol"].ToString() != "2")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha");
            return View(tbl_Aprendices);
        }

        // POST: Tbl_Aprendices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Ficha,Telefono,Correo,Id_Centro,Id_Empresa")] Tbl_Aprendices tbl_Aprendices)
        {
            if (ModelState.IsValid)
            {
                if (Metodos.Cadena(tbl_Aprendices.Nombres) && Metodos.Cadena(tbl_Aprendices.Apellidos) && Metodos.Numeros(tbl_Aprendices.Telefono))
                {
                    db.Entry(tbl_Aprendices).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { ficha = tbl_Aprendices.Id_Ficha });
                }
                else { ViewBag.Validar = "Llene los campos con el formato correcto"; }
            }
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha", tbl_Aprendices.Id_Ficha);
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Delete/5
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
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Aprendices);
        }

        // POST: Tbl_Aprendices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            db.Tbl_Aprendices.Remove(tbl_Aprendices);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult empresa(int? id, int? etapa)
        {
            try
            {
                var idA = (from e in db.Tbl_Empresas
                           where e.Id_Aprendiz == id
                           select e).First();
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Empresas");
            }

            return RedirectToAction("Details", "Tbl_Empresas", new { id, etapa });
        }
        public ActionResult planeacion(int? id, int? etapa)
        {
            try
            {
                var idA = (from p in db.Tbl_Planeacion_Etapa_Productiva
                           where p.Id_Aprendiz == id
                           select p).First();
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Planeacion_Etapa_Productiva");
            }

            return RedirectToAction("Details", "Tbl_Planeacion_Etapa_Productiva", new { id, etapa });
        }
        public ActionResult Factores_Actitud(int? id, int? etapa)
        {
            int Id_Actitud;
            try
            {
                var Id_Factores_Actitud = (from f in db.Tbl_Actitud_Comportamiento
                                           where f.Id_Aprendiz == id && f.Tipo_Informe == etapa
                                           select f).First();

                Id_Actitud = Id_Factores_Actitud.Id_Actitud;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Actitud_Comportamiento", new { id, etapa });
            }

            return RedirectToAction("Details", "Tbl_Actitud_Comportamiento", new { id = Id_Actitud, etapa });
        }
        public ActionResult Factores_Tecnicos(int? id, int? etapa)
        {
            int Id_Tecnicos;
            try
            {
                var Id_Factores_Tecnicos = (from f in db.Tbl_Factores_Tecnicos
                                            where f.Id_Aprendiz == id && f.Tipo_Informe == etapa
                                            select f).First();

                Id_Tecnicos = Id_Factores_Tecnicos.Id_Factores_Tecnicos;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Factores_Tecnicos", new { id, etapa });
            }

            return RedirectToAction("Details", "Tbl_Factores_Tecnicos", new { id = Id_Tecnicos, etapa });
        }
        public ActionResult Evaluacion(int? id, int? etapa)
        {
            int Id_Evaluacion;
            try
            {
                var Id_Etapa_Productiva = (from f in db.Tbl_Evaluacion_Etapa_Productiva
                                           where f.Id_Aprendiz == id
                                           select f).First();

                Id_Evaluacion = Id_Etapa_Productiva.Id_Evaluacion;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Evaluacion_Etapa_Productiva", new { id });
            }

            return RedirectToAction("Details", "Tbl_Evaluacion_Etapa_Productiva", new { id = Id_Evaluacion, etapa });
        }
        public ActionResult Back(int? ficha)
        {

            return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
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
