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
    public class Tbl_FichasController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Fichas
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea el modelo necesario para mostrar la informacion
        /// </summary>
        /// <returns>Se devuelve la vista mas el modelo correspondiente a la tabla</returns>
        public ActionResult Index()
        {
            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }
            var tbl_Fichas = db.Tbl_Fichas.Include(t => t.Tbl_Programas);
            return View(tbl_Fichas.ToList());
        }

        // GET: Tbl_Fichas/Create
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag para posterior mente ser un DropDownList en la vista
        /// </summary>
        /// <returns>Se devuelve la vista</returns>
        public ActionResult Create()
        {
            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Id_Programa = new SelectList(db.Tbl_Programas, "Id_Programa", "Nombre");
            return View();
        }

        // POST: Tbl_Fichas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se valida que el modelo sea valido y se crea el nuevo registro en la DB
        /// Se crea un ViewBag para posterior mente ser un DropDownList en la vista
        /// </summary>
        /// <param name="tbl_Fichas">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Ficha,Numero_Ficha,Id_Programa")] Tbl_Fichas tbl_Fichas)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Fichas.Add(tbl_Fichas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Programa = new SelectList(db.Tbl_Programas, "Id_Programa", "Nombre", tbl_Fichas.Id_Programa);
            return View(tbl_Fichas);
        }

        // GET: Tbl_Fichas/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag para posterior mente ser un DropDownList en la vista
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
            Tbl_Fichas tbl_Fichas = db.Tbl_Fichas.Find(id);
            if (tbl_Fichas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Programa = new SelectList(db.Tbl_Programas, "Id_Programa", "Nombre", tbl_Fichas.Id_Programa);
            return View(tbl_Fichas);
        }

        // POST: Tbl_Fichas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se valida que el modelo sea valido y se edita el registro en la DB
        /// Se crea un ViewBag para posterior mente ser un DropDownList en la vista
        /// </summary>
        /// <param name="tbl_Fichas">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Ficha,Numero_Ficha,Id_Programa")] Tbl_Fichas tbl_Fichas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Fichas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Programa = new SelectList(db.Tbl_Programas, "Id_Programa", "Nombre", tbl_Fichas.Id_Programa);
            return View(tbl_Fichas);
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
