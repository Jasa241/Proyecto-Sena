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
        /// <summary>
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se obtiene la informacion del registro mediante el id
        /// </summary>
        /// <param name="id">es el id que tiene la informacion del registro</param>
        /// <param name="etapa">contiene la etapa en la que nos encontramos</param>
        /// <returns>devuelve la vista mas el modelo</returns>
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
            Tbl_Factores_Tecnicos tbl_Factores_Tecnicos = db.Tbl_Factores_Tecnicos.Find(id);
            if (tbl_Factores_Tecnicos == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Factores_Tecnicos);
        }

        // GET: Tbl_Factores_Tecnicos/Create
        /// <summary>
        ///  Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        ///  se crea un objeto del tipo de la tabla y se asigna el valor del id y la etapa
        ///  a los respectivos atributos
        /// </summary>
        /// <param name="id">es el id del aprendiz el cual se le va generar el factor tecnico</param>
        /// <param name="etapa">es la etapa en la cual se encuentra el aprendiz</param>
        /// <returns>devulve la vista mas el modelo</returns>
        public ActionResult Create(int id, int etapa)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
            Tbl_Factores_Tecnicos tbl_Factores_Tecnicos = new Tbl_Factores_Tecnicos();
            tbl_Factores_Tecnicos.Id_Aprendiz = id;
            tbl_Factores_Tecnicos.Tipo_Informe = etapa;
            return View(tbl_Factores_Tecnicos);
        }

        // POST: Tbl_Factores_Tecnicos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se crea el registro
        /// se obtiene la ficha para redireccionar correctamente
        /// </summary>
        /// <param name="tbl_Factores_Tecnicos">es el objeto del tipo de la tabla, el cual 
        /// contiene la informacion suministrada por el usuario</param>
        /// <returns>se redireciona al index de aprendices, con la ficha</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Factores_Tecnicos,Tipo_Informe,Transferencia_Conocimiento,Valoracion_Conocimiento,Mejora_Continua,Valoracion_Mejora,Fortalecimiento_Ocupacional,Valoracion_Fortalecimiento,Oportunidad_Calidad,Valoracion_Oportunidad,Responsabilidad_Ambiental,Valoracion_Ambiental,Administracion_Recursos,Valoracion_Administracion,Seguridad_Ocupacional,Valoracion_Seguridad,Documentacion_Etapa_Productiva,Valoracion_Etapa_Productiva,Observacion_Jefe,Observacion_Aprendiz,Id_Aprendiz")] Tbl_Factores_Tecnicos tbl_Factores_Tecnicos)
        {
            if (ModelState.IsValid)
            {
                var ficha = (from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Factores_Tecnicos.Id_Aprendiz select f.Id_Ficha).First();
                db.Tbl_Factores_Tecnicos.Add(tbl_Factores_Tecnicos);
                db.SaveChanges();
                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                
            }
            return View(tbl_Factores_Tecnicos);
        }

        // GET: Tbl_Factores_Tecnicos/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se crea un objeto del tipo de la tabla y se le asigna el id y la etapa en sus
        /// respectivos atributos
        /// </summary>
        /// <param name="id">es el id del aprendiz</param>
        /// <param name="etapa">es la etapa en la que se encuentra</param>
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
        /// <summary>
        /// se edita el registro
        /// </summary>
        /// <param name="tbl_Factores_Tecnicos">es un objeto del tipo de la clase, el cual obtiene
        /// los datos suministrados por el usuario</param>
        /// <returns>redireciona al details de aprendices, con el id y la etapa del aprendiz</returns>
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
