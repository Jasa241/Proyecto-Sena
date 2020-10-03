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
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que almacena la etapa en la que se encuentra, para redireccionar
        /// correctamente
        /// </summary>
        /// <param name="id">Es el id con el cual se trae la informacion de la tabla</param>
        /// <param name="etapa">Determina en que etapa del proceso se encuentra el usuario</param>
        /// <returns>La vista con el model que contiene la informacion de la tabla</returns>
        public ActionResult Details(int? id, int? etapa)
        {
            if (Session["Rol"].ToString() == "3")
                return RedirectToAction("Index", "Home");
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
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso 
        /// se crea un objeto del tipo de la tabla, y se le asigna los valores de id y etapa, en
        /// los respectivos atributos
        /// </summary>
        /// <param name="id">Contiene el id del aprendiz</param>
        /// <param name="etapa">Contiene la etapa en la cual se registra la informacion</param>
        /// <returns>devuelve la vista con el modelo</returns>
        public ActionResult Create(int id, int etapa)
        {
            if (Session["Rol"].ToString() == "3")
                return RedirectToAction("Index", "Home");
            Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento = new Tbl_Actitud_Comportamiento();
            tbl_Actitud_Comportamiento.Id_Aprendiz = id;
            tbl_Actitud_Comportamiento.Tipo_Informe = etapa;
            return View(tbl_Actitud_Comportamiento);
        }

        // POST: Tbl_Actitud_Comportamiento/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se crea el nuevo registro
        /// se obtiene la ficha del aprendiz, para redireccionar correctamente
        /// </summary>
        /// <param name="tbl_Actitud_Comportamiento">es un objeto del tipo de la tabla el cual
        /// obtiene la informacion suministrada por el usuario</param>
        /// <returns>redirecciona al index de aprendices, con la ficha</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Actitud,Tipo_Informe,Relaciones_Interpersonales,Valoracion_Relaciones,TrabajoEnEquipo,Valoracion_Trabajo,Solucion_De_Problemas,Valoracion_Solucion,Cumplimiento,Valoracion_Cumplimiento,Organizacion,Valoracion_Organizacion,Id_Aprendiz")] Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Actitud_Comportamiento.Id_Aprendiz select f.Id_Ficha).First();

                db.Tbl_Actitud_Comportamiento.Add(tbl_Actitud_Comportamiento);
                db.SaveChanges();
                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });

            }
            return View(tbl_Actitud_Comportamiento);
        }

        // GET: Tbl_Actitud_Comportamiento/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se obtiene la informacion del registro por medio del id 
        /// se crea un objeto del tipo de la tabla y se le asigan el id del aprendiz y la etapa,
        /// esto porque estos datos no se pueden modificar
        /// </summary>
        /// <param name="id">contiene el id para traer toda la informacion</param>
        /// <param name="etapa">Contiene la etapa en la que nos encontramos</param>
        /// <returns>devulve la vista mas el modelo</returns>
        public ActionResult Edit(int? id, int? etapa)
        {
            if (Session["Rol"].ToString() == "3")
                return RedirectToAction("Index", "Home");
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
        /// <summary>
        /// se edita el registro
        /// se obtiene la ficha para redireccionar correctamente
        /// </summary>
        /// <param name="tbl_Actitud_Comportamiento">obtiene la info suministrada por el usuario</param>
        /// <returns>redirecciona al index de aprendices con la ficha</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Actitud,Tipo_Informe,Periodo_Evaluado,Relaciones_Interpersonales,Valoracion_Relaciones,TrabajoEnEquipo,Valoracion_Trabajo,Solucion_De_Problemas,Valoracion_Solucion,Cumplimiento,Valoracion_Cumplimiento,Organizacion,Valoracion_Organizacion,Id_Aprendiz")] Tbl_Actitud_Comportamiento tbl_Actitud_Comportamiento)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Actitud_Comportamiento.Id_Aprendiz select f.Id_Ficha).First();

                db.Entry(tbl_Actitud_Comportamiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
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
