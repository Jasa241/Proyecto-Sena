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
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// </summary>
        /// <returns>Se devuelve la vista mas el modelo correspondiente a la tabla</returns>
        public ActionResult Index()
        {
            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Tbl_Programas.ToList());
        }

        // GET: Tbl_Programas/Create
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// </summary>
        /// <returns>Se devuelve la vista</returns>
        public ActionResult Create()
        {
            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Tbl_Programas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se valida que el modelo sea valido y se crea el nuevo registro en la DB
        /// </summary>
        /// <param name="tbl_Programas">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Programa,Nombre")] Tbl_Programas tbl_Programas)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Programas.Add(tbl_Programas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Programas);
        }

        // GET: Tbl_Programas/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a editar</param>
        /// <returns>El modelo con la informacion correspondiente al id</returns>
        public ActionResult Edit(int? id)
        {
            if (Session["Rol"].ToString() != "1")
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
        /// <summary>
        /// se valida que el modelo sea valido y se edita el registro en la DB
        /// </summary>
        /// <param name="tbl_Programas">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Programa,Nombre")] Tbl_Programas tbl_Programas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Programas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
