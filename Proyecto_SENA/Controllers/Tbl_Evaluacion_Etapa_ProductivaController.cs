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
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que contiene la etapa para poder redireccionar correctamente 
        /// </summary>
        /// <param name="id">es el id que contiene la informacion del registro</param>
        /// <param name="etapa">contiene la etapa en la que se encuentra</param>
        /// <returns>se devuelve la vista mas el modelo</returns>
        public ActionResult Details(int? id, int? etapa)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
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
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se crea un objeto del tipo de la tabla y se asigna el id en el respectivo atributo
        /// </summary>
        /// <param name="id">es el id del aprendiz</param>
        /// <returns>se devuelve la vista mas el modelo</returns>
        public ActionResult Create(int id)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
            Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva = new Tbl_Evaluacion_Etapa_Productiva();
            tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz = id;
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // POST: Tbl_Evaluacion_Etapa_Productiva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se crea el registro
        /// se obtiene la ficha para poder redireccionar correctamente 
        /// </summary>
        /// <param name="tbl_Evaluacion_Etapa_Productiva">es un objeto que obtiene la informacion
        /// suministrada por el usuario</param>
        /// <returns>redirecciona al index de aprendices con la ficha</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Evaluacion,Juicio_Evaluacion,Reconocimientos,Id_Aprendiz")] Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz select f.Id_Ficha).First();
                db.Tbl_Evaluacion_Etapa_Productiva.Add(tbl_Evaluacion_Etapa_Productiva);
                db.SaveChanges();
                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
            }
            return View(tbl_Evaluacion_Etapa_Productiva);
        }

        // GET: Tbl_Evaluacion_Etapa_Productiva/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se obtiene la informacion con el id, y se crea un objeto del tipo de la tabla,
        /// se establece el id del aprendiz ya que este no puede ser modificado
        /// Se crea un ViewBag que contiene la etapa, para posteriormente poder direccionar correctamente
        /// </summary>
        /// <param name="id">es el id que contiene la informacion del registro</param>
        /// <param name="etapa">contiene la etapa en la que se encuentra</param>
        /// <returns>devuelve la vista mas el modelo</returns>
        public ActionResult Edit(int? id, int? etapa)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
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
        /// <summary>
        /// se edita el registro
        /// </summary>
        /// <param name="tbl_Evaluacion_Etapa_Productiva">es un objeto del tipo de la tabla
        /// que obtiene la informacion suministrada por el usuario</param>
        /// <returns>redirecciona al details de aprendices con el id del aprendiz y la etapa</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Evaluacion,Juicio_Evaluacion,Reconocimientos,Id_Aprendiz")] Tbl_Evaluacion_Etapa_Productiva tbl_Evaluacion_Etapa_Productiva)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Evaluacion_Etapa_Productiva.Id_Aprendiz select f.Id_Ficha).First();
                db.Entry(tbl_Evaluacion_Etapa_Productiva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
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
