using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.WebSockets;
using Antlr.Runtime;
using Proyecto_SENA.Models;

namespace Proyecto_SENA.Controllers
{
    [Authorize]
    public class Tbl_AprendicesController : Controller
    {
        private practicas3Entities db = new practicas3Entities();

        // GET: Tbl_Aprendices
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// se recibe la ficha para mostrar unicamente los aprendices pertenecientes a ella
        /// </summary>
        /// <returns>Se devuelve la vista mas el modelo correspondiente a la tabla</returns>
        public ActionResult Index(int? ficha)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }

            var Aprendices = (from Aprendiz in db.Tbl_Aprendices
                              where ficha == Aprendiz.Id_Ficha
                              select Aprendiz).ToList();

            return View(Aprendices);
        }

        // GET: Tbl_Aprendices/Details/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro</param>
        /// <returns>la vista con el modelo que contiene la informacion correspondiente al id</returns>
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
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }

            ViewBag.etapa = etapa;
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Create
        /// <summary>
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <returns>Se devuelve la vista</returns>
        public ActionResult Create()
        {
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha");
            return View();
        }

        // POST: Tbl_Aprendices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// se valida que el modelo sea valido y se crea el nuevo registro en la DB
        /// </summary>
        /// <param name="tbl_Aprendices">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al create del controller empresas</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Ficha,Telefono,Correo,Id_Empresa")] Tbl_Aprendices tbl_Aprendices)
        {
            if (ModelState.IsValid && tbl_Aprendices.Nombres != null && tbl_Aprendices.Apellidos != null && tbl_Aprendices.Telefono != null && tbl_Aprendices.Correo != null)
            {
                db.Tbl_Aprendices.Add(tbl_Aprendices);
                db.SaveChanges();
                return RedirectToAction("Create", "Tbl_Empresas", new { Id_Aprendiz = tbl_Aprendices.Numero_Identificacion});
            }
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha", tbl_Aprendices.Id_Ficha);
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Edit/5
        /// <summary>
        /// Se valida mediante la variable de Session["Rol"] que el usuario tenga permiso
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a editar</param>
        /// <returns>El modelo con la informacion correspondiente al id</returns>
        public ActionResult Edit(int? id)
        {
            if (Session["Rol"].ToString() == "3")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha");
            return View(tbl_Aprendices);
        }

        // POST: Tbl_Aprendices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// se valida que el modelo sea valido y se edita el registro en la DB
        /// Se crea un ViewBag que posteriormente sera un DropDownList en la vista
        /// </summary>
        /// <param name="tbl_Aprendices">Recibe el modelo del tipo correspondiente a la tabla,
        /// con la informacion suministrada</param>
        /// <returns>Redireciona al index del controller aprendices, con la ficha del aprendiz</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Numero_Identificacion,Nombres,Apellidos,Id_Ficha,Telefono,Correo,Id_Centro,Id_Empresa")] Tbl_Aprendices tbl_Aprendices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Aprendices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { ficha = tbl_Aprendices.Id_Ficha });
            }
            ViewBag.Id_Ficha = new SelectList(db.Tbl_Fichas, "Id_Ficha", "Numero_Ficha", tbl_Aprendices.Id_Ficha);
            return View(tbl_Aprendices);
        }

        // GET: Tbl_Aprendices/Delete/5
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
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            if (tbl_Aprendices == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Aprendices);
        }

        // POST: Tbl_Aprendices/Delete/5
        /// <summary>
        /// Se muestar la vista con la informacion correspondiente al id
        /// </summary>
        /// <param name="id">Resive el id que contiene la informacion del registro a eliminar</param>
        /// <returns>Se devuelve la vista mas el modelo con la informacion del registro a eliminar</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tbl_Aprendices tbl_Aprendices = db.Tbl_Aprendices.Find(id);
            db.Tbl_Aprendices.Remove(tbl_Aprendices);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// se consulta si el aprendiz ya tiene una empresa registrada, si es asi se redirecciona al
        /// details del controller empresa, si es no redireccona al create del controller empresa,
        /// ambas redireciones incluyen el id del aprendiz, en el caso de details se envia la etapa 
        /// tambien
        /// </summary>
        /// <param name="id">Es el id del aprendiz al cual se va asignar una empresa</param>
        /// <param name="etapa">es la etapa en la que nos encontramos (Parcial,Final)</param>
        /// <returns></returns>
        public ActionResult empresa(int? id, int? etapa)
        {
            int Id_Empresa;
            try
            {
                var Id_empresa = (from e in db.Tbl_Empresas
                           where e.Id_Aprendiz == id
                           select e).First();
                Id_Empresa = Id_empresa.Id_Empresa;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Empresas", new { Id_Aprendiz = id });
            }

            return RedirectToAction("Details", "Tbl_Empresas", new { id, etapa });
        }

        /// <summary>
        /// se consulta si el aprendiz ya tiene una planeacion registrada, si es asi se redirecciona
        /// al details del controller planeacion, si es no redireccona al create del controller 
        /// planeacion, ambas redireciones incluyen el id del aprendiz, en el caso de details 
        /// se envia la etapa tambien
        /// </summary>
        /// <param name="id">Es el id del aprendiz al cual se va asignar una empresa</param>
        /// <param name="etapa">es la etapa en la que nos encontramos (Parcial,Final)</param>
        /// <returns></returns>
        public ActionResult planeacion(int? id, int? etapa)
        {
            int Id_Planeacion;
            try
            {
                var Id_planeacion = (from p in db.Tbl_Planeacion_Etapa_Productiva
                           where p.Id_Aprendiz == id
                           select p).First();
                Id_Planeacion = Id_planeacion.Id_Planeacion;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Planeacion_Etapa_Productiva", new { id });
            }

            return RedirectToAction("Details", "Tbl_Planeacion_Etapa_Productiva", new { id = Id_Planeacion, etapa });
        }

        /// <summary>
        /// se consulta si el aprendiz ya tiene un factor actitud registrado, si es asi se
        /// redirecciona al details del controller factores_actitud, si es no redireccona
        /// al create del controller factores_actitud, ambas redireciones incluyen el id 
        /// del aprendiz, en el caso de details se envia la etapa tambien
        /// </summary>
        /// <param name="id">Es el id del aprendiz al cual se va asignar una empresa</param>
        /// <param name="etapa">es la etapa en la que nos encontramos (Parcial,Final)</param>
        /// <returns></returns>
        public ActionResult Factores_Actitud(int? id, int? etapa)
        {
            int Id_Actitud;
            try
            {
                var Id_Factores_Actitud = (from f in db.Tbl_Actitud_Comportamiento
                                           where f.Id_Aprendiz == id && f.Tipo_Informe == etapa
                                           select f).First();

                Id_Actitud = Id_Factores_Actitud.Id_Actitud;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Actitud_Comportamiento", new { id, etapa });
            }

            return RedirectToAction("Details", "Tbl_Actitud_Comportamiento", new { id = Id_Actitud, etapa });
        }
        /// <summary>
        /// se consulta si el aprendiz ya tiene un factor tecnico registrado, si es asi se
        /// redirecciona al details del controller factores_tecnico, si es no redireccona
        /// al create del controller factores_tecnico, ambas redireciones incluyen el id 
        /// del aprendiz, en el caso de details se envia la etapa tambien
        /// </summary>
        /// <param name="id">Es el id del aprendiz al cual se va asignar una empresa</param>
        /// <param name="etapa">es la etapa en la que nos encontramos (Parcial,Final)</param>
        /// <returns></returns>
        public ActionResult Factores_Tecnicos(int? id, int? etapa)
        {
            int Id_Tecnicos;
            try
            {
                var Id_Factores_Tecnicos = (from f in db.Tbl_Factores_Tecnicos
                                            where f.Id_Aprendiz == id && f.Tipo_Informe == etapa
                                            select f).First();

                Id_Tecnicos = Id_Factores_Tecnicos.Id_Factores_Tecnicos;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Factores_Tecnicos", new { id, etapa });
            }

            return RedirectToAction("Details", "Tbl_Factores_Tecnicos", new { id = Id_Tecnicos, etapa });
        }

        /// <summary>
        /// se consulta si el aprendiz ya tiene una evaluacion registrado, si es asi se
        /// redirecciona al details del controller evaluacion, si es no redireccona
        /// al create del controller evaluacion, ambas redireciones incluyen el id 
        /// del aprendiz, en el caso de details se envia la etapa tambien
        /// </summary>
        /// <param name="id">Es el id del aprendiz al cual se va asignar una empresa</param>
        /// <param name="etapa">es la etapa en la que nos encontramos (Parcial,Final)</param>
        /// <returns></returns>
        public ActionResult Evaluacion(int? id, int? etapa)
        {
            int Id_Evaluacion;
            try
            {
                var Id_Etapa_Productiva = (from f in db.Tbl_Evaluacion_Etapa_Productiva
                                           where f.Id_Aprendiz == id
                                           select f).First();

                Id_Evaluacion = Id_Etapa_Productiva.Id_Evaluacion;
            }
            catch
            {
                return RedirectToAction("Create", "Tbl_Evaluacion_Etapa_Productiva", new { id });
            }

            return RedirectToAction("Details", "Tbl_Evaluacion_Etapa_Productiva", new { id = Id_Evaluacion, etapa });
        }
        public ActionResult Back(int? ficha)
        {

            return RedirectToAction("Index", "Tbl_Aprendices", new { ficha });
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
