using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Proyecto_SENA.Models;

namespace Proyecto_SENA.Controllers
{
    [Authorize]
    public class Tbl_InstructoresController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Instructores
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

            var tbl_Instructores = db.Tbl_Instructores.Include(t => t.Tbl_Usuarios);
            return View(tbl_Instructores.ToList());
        }

        // GET: Tbl_Instructores/Create
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <returns>Se devuelve la vista</returns>
        public ActionResult Create()
        {
            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Id_Usuario = List_Instructores();
            return View();
        }

        // POST: Tbl_Instructores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se valida que el modelo sea valido y se crea el nuevo registro en la DB
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <param name="tbl_Instructores">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Usuario")] Tbl_Instructores tbl_Instructores)
        {
            if (ModelState.IsValid)
            {
                db.Tbl_Instructores.Add(tbl_Instructores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = List_Instructores();
            return View(tbl_Instructores);
        }

        // GET: Tbl_Instructores/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
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
            Tbl_Instructores tbl_Instructores = db.Tbl_Instructores.Find(id);
            if (tbl_Instructores == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Usuario = List_Instructores();
            return View(tbl_Instructores);
        }

        // POST: Tbl_Instructores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// se valida que el modelo sea valido y se edita el registro en la DB
        /// </summary>
        /// <param name="tbl_Instructores">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Usuario")] Tbl_Instructores tbl_Instructores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Instructores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Usuario = List_Instructores();
            return View(tbl_Instructores);
        }

        // GET: Tbl_Instructores/Delete/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a eliminar</param>
        /// <returns>La vista con el modelo que contiene la informacion correspondiente al id</returns>
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
            Tbl_Instructores tbl_Instructores = db.Tbl_Instructores.Find(id);
            if (tbl_Instructores == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Instructores);
        }

        // POST: Tbl_Instructores/Delete/5
        /// <summary>
        /// Se muestar la vista con la informacion correspondiente al id
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a eliminar</param>
        /// <returns>Se devuelve la vista mas el modelo con la informacion del registro a eliminar</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Instructores tbl_Instructores = db.Tbl_Instructores.Find(id);
            db.Tbl_Instructores.Remove(tbl_Instructores);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Se crea una lista de tipo SelectListItem 
        /// se buscan los usuario que considan con el rol 2 (instructor)
        /// para mostrar solo los usuarios que pertenecen a los intructores
        /// </summary>
        /// <returns>se regresa la lista con los usuarios ya filtrados</returns>
        static List<SelectListItem> List_Instructores()
        {
            practicas3Entities db = new practicas3Entities();

            //Necesio retornar los nombres de usuario que tienen el rol instructor
            var lst = (from l in db.Tbl_Usuarios
                       where l.Id_Rol == 2
                       select new { id = l.NombreUsuario, Nombre_Usuario = l.NombreUsuario }).ToList();

            //Convierto la consulta a SelectListItem que es el tipo con el que trabaja el DropDownList
            List<SelectListItem> Id_Usuario = lst.ConvertAll(l =>
            {
                return new SelectListItem()
                {
                    Text = l.Nombre_Usuario,
                    Value = l.id.ToString()
                };
            });

            return Id_Usuario;
        }
    }
}
