using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_SENA.Models;
using System.Web.Security;
using Microsoft.Ajax.Utilities;

namespace Proyecto_SENA.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            LogOut();
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(String NombreUsuario, String Contrasena)
        {
            try
            {
                if (NombreUsuario != "" && Contrasena != "")
                {
                    using (var db = new practicas3Entities())
                    {
                        string resultado = string.Empty;
                        byte[] encriptar = System.Text.Encoding.Unicode.GetBytes(Contrasena);
                        resultado = Convert.ToBase64String(encriptar);

                        var login = (from l in db.Tbl_Usuarios
                                     where l.NombreUsuario == NombreUsuario &&
                                     l.Contrasena == resultado
                                     select l).First();

                        Session["Rol"] = login.Id_Rol.ToString();
                        Session["Id_Usuario"] = login.NombreUsuario.ToString();

                        FormsAuthentication.SetAuthCookie(login.NombreUsuario, false);

                        if (login.Id_Rol == 1 || login.Id_Rol == 2)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Create", "Tbl_Aprendices");
                        }
                    }
                }
                else
                {
                    ViewBag.Validar = "Llene todos los campos";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Validar = "Usuario y/o contraseña equivocado";
                return View();
            }

            //Session["Rol"] = "1";
            //return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Login");
        }
    }
}