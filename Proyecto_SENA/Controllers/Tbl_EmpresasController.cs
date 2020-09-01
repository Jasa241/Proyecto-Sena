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
                if(e.Id_Aprendiz == id)
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tbl_Empresas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Empresa,Razon_Social,Nit,Direccion,Nombre_Jefe,Cargo,Telefono,Correo,Id_Aprendiz")] Tbl_Empresas tbl_Empresas)
        {
            if (ModelState.IsValid && tbl_Empresas.Nit != null && tbl_Empresas.Nombre_Jefe != null && tbl_Empresas.Cargo != null && tbl_Empresas.Correo != null && tbl_Empresas.Telefono != null && tbl_Empresas.Direccion != null && tbl_Empresas.Razon_Social != null)
            {
                int ida = 0;
                int? ficha = 0;
                var aprendiz = from a in db.Tbl_Aprendices select a;

                foreach (var a in aprendiz)
                {
                    if (a.Numero_Identificacion == tbl_Empresas.Id_Aprendiz)
                        ida = a.Numero_Identificacion;
                        ficha = a.Id_Ficha;
                }
                tbl_Empresas.Id_Aprendiz = ida;
                db.Tbl_Empresas.Add(tbl_Empresas);
                db.SaveChanges();

                if (Session["Rol"].ToString() == "1" || Session["Rol"].ToString() == "2")
                    return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
                else
                    return RedirectToAction("Index", "Login");
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
        public ActionResult Edit( [Bind(Include = "Id_Empresa,Razon_Social,Nit,Direccion,Nombre_Jefe,Cargo,Telefono,Correo")] Tbl_Empresas tbl_Empresas)
        {
            if (ModelState.IsValid)
            {
                int? ida = 0;
                int? idf = 0;
                var empresa = from e in db.Tbl_Empresas where e.Id_Empresa == tbl_Empresas.Id_Empresa select e.Id_Aprendiz;

                foreach (var e in empresa)
                {
                    ida = e;
                }

                tbl_Empresas.Id_Aprendiz = ida;
                db.Entry(tbl_Empresas).State = EntityState.Modified;
                db.SaveChanges();

                var ficha = from f in db.Tbl_Aprendices where f.Numero_Identificacion == ida select f.Id_Ficha;
                foreach (var f in ficha)
                {
                    idf = f;
                }

                return RedirectToAction("Index", "Tbl_Aprendices", new { ficha = idf });
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
