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
    public class Tbl_InstructoresController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Instructores
        public ActionResult Index()
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            var tbl_Instructores = db.Tbl_Instructores.Include(t => t.Tbl_Usuarios);
            return View(tbl_Instructores.ToList());
        }

        // GET: Tbl_Instructores/Create
        public ActionResult Create()
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Id_Usuario = new SelectList(db.Tbl_Usuarios, "Id_Usuario", "NombreUsuario");

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
            //genero el viewbag
            ViewBag.Id_Usuario = Id_Usuario;

            return View();
        }

        // POST: Tbl_Instructores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Instructor,Numero_Identificacion,Nombres,Apellidos,Id_Usuario")] Tbl_Instructores tbl_Instructores)
        {
            if (ModelState.IsValid && tbl_Instructores.Nombres != null && tbl_Instructores.Apellidos != null)
            {
                try
                {
                    db.Tbl_Instructores.Add(tbl_Instructores);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch { ViewBag.Validacion = "Numero de identificacion ya existente"; }
            }
            else
            {
                ViewBag.Validacion = "Llene todos los campos";
            }
            ViewBag.Id_Usuario = new SelectList(db.Tbl_Usuarios, "Id_Usuario", "NombreUsuario");

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
            //genero el viewbag
            ViewBag.Id_Usuario = Id_Usuario;
            return View(tbl_Instructores);
        }

        // GET: Tbl_Instructores/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
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
            ViewBag.Id_Usuario = new SelectList(db.Tbl_Usuarios, "NombreUsuario", "NombreUsuario");

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
            //genero el viewbag
            ViewBag.Id_Usuario = Id_Usuario;
            return View(tbl_Instructores);
        }

        // POST: Tbl_Instructores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Instructor,Numero_Identificacion,Nombres,Apellidos,Id_Usuario")] Tbl_Instructores tbl_Instructores)
        {
            if (ModelState.IsValid && tbl_Instructores.Nombres != null && tbl_Instructores.Apellidos != null)
            {
                db.Entry(tbl_Instructores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Validacion = "Llene todos los campos";
            }
            ViewBag.Id_Usuario = new SelectList(db.Tbl_Usuarios, "NombreUsuario", "NombreUsuario");

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
            //genero el viewbag
            ViewBag.Id_Usuario = Id_Usuario;
            return View(tbl_Instructores);
        }

        // GET: Tbl_Instructores/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Session["Rol"].ToString() != "1")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
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
    }
}
