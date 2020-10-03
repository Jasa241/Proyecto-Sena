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
        /// <summary>Se crea un ViewBag con la etapa en la que se encuentra, para continuar 
        /// con el correcto direccionamiento</summary>
        /// <param name="id">Resive el id que contiene la informacion del registro</param>
        /// <returns>El modelo con la informacion correspondiente al id</returns>
        public ActionResult Details(int? id, int etapa)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva = db.Tbl_Planeacion_Etapa_Productiva.Find(id);
            if (tbl_Planeacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // GET: Tbl_Planeacion_Etapa_Productiva/Create
        /// <param name="id">Resive el Id del aprendiz, para ser guardado en el modelo
        /// </param>
        /// <returns>Se devuelve la vista mas el modelo</returns>
        public ActionResult Create(int? id)
        {
            Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva = new Tbl_Planeacion_Etapa_Productiva();
            tbl_Planeacion_Etapa_Productiva.Id_Aprendiz = id;
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // POST: Tbl_Planeacion_Etapa_Productiva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se crea el registro
        /// se obtiene la ficha del aprendiz, para redireccionar con el numero de la ficha y cargar la 
        /// informacion correctamente
        /// </summary>
        /// <param name="tbl_Planeacion_Etapa_Productiva">es el objeto del tipo de la tabla el cual tiene la informacion
        /// suministrada por el usuario
        /// </param>
        /// <returns>Redirecciona segun el rol que realizo la accion</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Planeacion,Actividades_A_Desarrollar,Evidencias_Aprendizaje,Fecha,Lugar,Observaciones,Id_Aprendiz")] Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Planeacion_Etapa_Productiva.Id_Aprendiz select f.Id_Ficha).First();
                db.Tbl_Planeacion_Etapa_Productiva.Add(tbl_Planeacion_Etapa_Productiva);
                db.SaveChanges();

                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
            }
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // GET: Tbl_Planeacion_Etapa_Productiva/Edit/5
        /// <summary>
        /// Se crea el modelo con la informacion obtenida atraves del id, y se da valor al Id_Aprendiz,
        /// ya que este campo no se puede cambiar
        /// </summary>
        /// <param name="id">Obtiene el Id de la empresa para mostrar la informacion</param>
        /// <returns>Devuelve la vista mas el modelo</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva = db.Tbl_Planeacion_Etapa_Productiva.Find(id);
            tbl_Planeacion_Etapa_Productiva.Id_Aprendiz = tbl_Planeacion_Etapa_Productiva.Id_Aprendiz;
            if (tbl_Planeacion_Etapa_Productiva == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Planeacion_Etapa_Productiva);
        }

        // POST: Tbl_Planeacion_Etapa_Productiva/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se edita el registro
        /// se obtiene la ficha del aprendiz, para redireccionar con el numero de la ficha y cargar la 
        /// informacion correctamente
        /// </summary>
        /// <param name="tbl_Planeacion_Etapa_Productiva">es el objeto del tipo de la tabla el cual tiene la informacion
        /// suministrada por el usuario</param>
        /// <returns>Se redirecciona al index de aprendices, mas la ficha</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Planeacion,Actividades_A_Desarrollar,Evidencias_Aprendizaje,Fecha,Lugar,Observaciones,Id_Aprendiz")] Tbl_Planeacion_Etapa_Productiva tbl_Planeacion_Etapa_Productiva)
        {
            if (ModelState.IsValid && tbl_Planeacion_Etapa_Productiva.Actividades_A_Desarrollar != null && tbl_Planeacion_Etapa_Productiva.Evidencias_Aprendizaje != null && tbl_Planeacion_Etapa_Productiva.Observaciones != null)
            {
                db.Entry(tbl_Planeacion_Etapa_Productiva).State = EntityState.Modified;
                db.SaveChanges();
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Planeacion_Etapa_Productiva.Id_Aprendiz select f.Id_Ficha).First();

                if (Session["Rol"].ToString() != "1" || Session["Rol"].ToString() != "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
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
