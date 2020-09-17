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
    public class Tbl_EmpresasController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Empresas/Details/5
        public ActionResult Details(int? id, int etapa)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ide = 0;
            var empresa = from e in db.Tbl_Empresas
                          select e;
            foreach (var e in empresa)
            {
                if (e.Id_Aprendiz == id)
                    ide = e.Id_Empresa;
            }
            Tbl_Empresas tbl_Empresas = db.Tbl_Empresas.Find(ide);

            if (tbl_Empresas == null)
            {
                return HttpNotFound();
            }
            ViewBag.etapa = etapa;
            return View(tbl_Empresas);
        }

        // GET: Tbl_Empresas/Create
        public ActionResult Create(int? Id_Aprendiz)
        {
            Tbl_Empresas tbl_Empresas = new Tbl_Empresas();
            tbl_Empresas.Id_Aprendiz = Id_Aprendiz;
            return View();
        }

        // POST: Tbl_Empresas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Razon_Social,Nit,Direccion,Nombre_Jefe,Cargo,Telefono,Correo,Id_Aprendiz")] Tbl_Empresas tbl_Empresas)
        {
            if (ModelState.IsValid && tbl_Empresas.Nit != null && tbl_Empresas.Nombre_Jefe != null && tbl_Empresas.Cargo != null && tbl_Empresas.Correo != null && tbl_Empresas.Telefono != null && tbl_Empresas.Direccion != null && tbl_Empresas.Razon_Social != null)
            {
                if (Metodos.Numeros(tbl_Empresas.Telefono) && Metodos.Cadena(tbl_Empresas.Nombre_Jefe) && Metodos.Cadena(tbl_Empresas.Cargo))
                {
                    int? ficha = 0;
                    var aprendiz = from a in db.Tbl_Aprendices select a;

                    foreach (var a in aprendiz)
                    {
                        if (a.Numero_Identificacion == tbl_Empresas.Id_Aprendiz)
                            ficha = a.Id_Ficha;
                    }

                    db.Tbl_Empresas.Add(tbl_Empresas);
                    db.SaveChanges();

                    if (Session["Rol"].ToString() == "1" || Session["Rol"].ToString() == "2")
                        return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                    else
                        return RedirectToAction("Index", "Login");
                }
                else { ViewBag.Validar = "Llene los campos con el formato correcto"; }
            }
            else { ViewBag.Validar = "Llene todos los campos"; }

            return View(tbl_Empresas);
        }

        // GET: Tbl_Empresas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Empresas tbl_Empresas = db.Tbl_Empresas.Find(id);
            tbl_Empresas.Id_Aprendiz = tbl_Empresas.Id_Aprendiz;
            if (tbl_Empresas == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Empresas);
        }

        // POST: Tbl_Empresas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Empresa,Razon_Social,Nit,Direccion,Nombre_Jefe,Cargo,Telefono,Correo,Id_Aprendiz")] Tbl_Empresas tbl_Empresas)
        {
            if (ModelState.IsValid)
            {
                if (Metodos.Numeros(tbl_Empresas.Telefono) && Metodos.Cadena(tbl_Empresas.Nombre_Jefe) && Metodos.Cadena(tbl_Empresas.Cargo))
                {
                   
                    int? Id_Ficha = 0;
                    
                    db.Entry(tbl_Empresas).State = EntityState.Modified;
                    db.SaveChanges();

                    var ficha = from f in db.Tbl_Aprendices where f.Numero_Identificacion == tbl_Empresas.Id_Aprendiz select f.Id_Ficha;
                    foreach (var f in ficha)
                    {
                        Id_Ficha = f;
                    }

                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha = Id_Ficha });
                }
                else { ViewBag.Validar = "Llene los campos con el formato correcto"; }
            }
            return View(tbl_Empresas);

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
