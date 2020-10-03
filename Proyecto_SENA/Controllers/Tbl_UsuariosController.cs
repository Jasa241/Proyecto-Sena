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

            var tbl_Usuarios = db.Tbl_Usuarios.Include(t => t.Tbl_Roles);
            return View(tbl_Usuarios.ToList());
        }

        // GET: Tbl_Usuarios/Create
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que posteriormente se convierte en un DropDownList en la vista
        /// </summary>
        /// <returns>Se devuelve la vista</returns>
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
        /// <summary>
        /// se valida que el modelo sea valido y se crea el nuevo registro en la DB
        /// Se crea un ViewBag que posteriormente se convierte en un DropDownList en la vista
        /// </summary>
        /// <param name="tbl_Usuarios">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreUsuario,Contrasena,Id_Rol")] Tbl_Usuarios tbl_Usuarios)
        {
            ViewBag.Id_Rol = new SelectList(db.Tbl_Roles, "Id_Rol", "Rol", tbl_Usuarios.Id_Rol);

            if (ModelState.IsValid)
            {
                try
                {
                    db.Tbl_Usuarios.Add(tbl_Usuarios);
                    tbl_Usuarios.Contrasena = Encriptar(tbl_Usuarios.Contrasena);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ViewBag.Validacion = "Usuario existente";
                    return View(tbl_Usuarios);
                }

            }

            return View(tbl_Usuarios);
        }

        // GET: Tbl_Usuarios/Edit/5.
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un objeto del tipo de la tabla, para asignar informacion que no puede ser 
        /// modificada
        /// Se desencripta
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a editar</param>
        /// <returns>El modelo con la informacion correspondiente al id</returns>
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
        /// <summary>
        /// se valida que el modelo sea valido y se edita el registro en la DB
        /// Se encripta la contraseña antes de hacer el edit en la DB
        /// </summary>
        /// <param name="tbl_Usuarios">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NombreUsuario,Contrasena,Id_Rol")] Tbl_Usuarios tbl_Usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Usuarios).State = EntityState.Modified;
                tbl_Usuarios.Contrasena = Encriptar(tbl_Usuarios.Contrasena);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Usuarios);
        }

        // GET: Tbl_Usuarios/Delete/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a eliminar</param>
        /// <returns>La vista con el modelo que contiene la informacion correspondiente al id</returns>
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
        /// <summary>
        /// Se muestar la vista con la informacion correspondiente al id
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a eliminar</param>
        /// <returns>Se devuelve la vista mas el modelo con la informacion del registro a eliminar</returns>
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

        /// <summary>
        /// Encriptamos la clave
        /// </summary>
        /// <param name="pass">Recibe la clave</param>
        /// <returns>Devuelve la clave encriptada</returns>
        string Encriptar(string pass)
        {
            string resultado = string.Empty;
            byte[] encriptar = System.Text.Encoding.Unicode.GetBytes(pass);
            resultado = Convert.ToBase64String(encriptar);

            return resultado;
        }

        /// <summary>
        /// Desencriptamos la clave
        /// </summary>
        /// <param name="pass">Recibe la clave encriptada</param>
        /// <returns>Devuelve la clave desencriptada</returns>
        string Desencriptar(string pass)
        {
            string resultado = string.Empty;
            byte[] desencriptar = Convert.FromBase64String(pass);
            resultado = System.Text.Encoding.Unicode.GetString(desencriptar);

            return resultado;
        }
    }
}
