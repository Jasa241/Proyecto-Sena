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
    public class Tbl_UsuariosController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Usuarios
        public ActionResult Index()
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            var tbl_Usuarios = db.Tbl_Usuarios.Include(t => t.Tbl_Roles);
            return View(tbl_Usuarios.ToList());
        }

        // GET: Tbl_Usuarios/Create
        public ActionResult Create()
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Id_Rol = new SelectList(db.Tbl_Roles, "Id_Rol", "Rol");
            return View();
        }

        // POST: Tbl_Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreUsuario,Contrasena,Id_Rol")] Tbl_Usuarios tbl_Usuarios)
        {
            if (ModelState.IsValid && tbl_Usuarios.NombreUsuario != null && tbl_Usuarios.Contrasena != null)
            {
                if (Metodos.Cadena(tbl_Usuarios.NombreUsuario))
                {
                    try
                    {
                        db.Tbl_Usuarios.Add(tbl_Usuarios);
                        tbl_Usuarios.Contrasena = Encriptar(tbl_Usuarios.Contrasena);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ViewBag.Validacion = "Nombre de usuario existente";
                    }
                }
                else { ViewBag.Validacion = "Llene los campos con el formato correcto"; }
            }
            else
            {
                ViewBag.Validacion = "Llene todos los campos";
            }

            ViewBag.Id_Rol = new SelectList(db.Tbl_Roles, "Id_Rol", "Rol", tbl_Usuarios.Id_Rol);
            return View(tbl_Usuarios);
        }

        // GET: Tbl_Usuarios/Edit/5
        public ActionResult Edit(string id)
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Usuarios tbl_Usuarios = db.Tbl_Usuarios.Find(id);
            tbl_Usuarios.NombreUsuario = id;
            tbl_Usuarios.Id_Rol = tbl_Usuarios.Id_Rol;
            tbl_Usuarios.Contrasena = Desencriptar(tbl_Usuarios.Contrasena);
            if (tbl_Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Usuarios);
        }

        // POST: Tbl_Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NombreUsuario,Contrasena,Id_Rol")] Tbl_Usuarios tbl_Usuarios)
        {

            if (ModelState.IsValid && tbl_Usuarios.Contrasena != null)
            {
                db.Entry(tbl_Usuarios).State = EntityState.Modified;
                tbl_Usuarios.Contrasena = Encriptar(tbl_Usuarios.Contrasena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Validacion = "Llene todos los campos";
            }
            return View(tbl_Usuarios);
        }

        // GET: Tbl_Usuarios/Delete/5
        public ActionResult Delete(string id)
        {

            if (Session["Rol"].ToString() != "1")
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Usuarios tbl_Usuarios = db.Tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Usuarios);
        }

        // POST: Tbl_Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tbl_Usuarios tbl_Usuarios = db.Tbl_Usuarios.Find(id);
            db.Tbl_Usuarios.Remove(tbl_Usuarios);
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

        string Encriptar(string pass)
        {
            string resultado = string.Empty;
            byte[] encriptar = System.Text.Encoding.Unicode.GetBytes(pass);
            resultado = Convert.ToBase64String(encriptar);

            return resultado;
        }

        string Desencriptar(string pass)
        {
            string resultado = string.Empty;
            byte[] desencriptar = Convert.FromBase64String(pass);
            resultado = System.Text.Encoding.Unicode.GetString(desencriptar);

            return resultado;
        }
    }
}
