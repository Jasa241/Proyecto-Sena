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
        /// <summary>
        /// Valida si el usuario tiene acceso o no, si la respuesta es negativa se devuelve
        /// un mensaje
        /// </summary>
        /// <param name="NombreUsuario">Recibe el nombre de usuario suministrado</param>
        /// <param name="Contrasena">Recibe la contraseña suministrada</param>
        /// <returns>Redireccionamiento segun el tipo de rol que ingrese</returns>
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
                        /*Encriptacion de la contraseña*/
                        string resultado = string.Empty;
                        byte[] encriptar = System.Text.Encoding.Unicode.GetBytes(Contrasena);
                        resultado = Convert.ToBase64String(encriptar);

                        /*Se valida el usuario y  la contraseña en la DB*/
                        var login = (from l in db.Tbl_Usuarios
                                     where l.NombreUsuario == NombreUsuario &&
                                     l.Contrasena == resultado
                                     select l).First();
                        /*Se le asigna el rol a la variable de Session["Rol"], para comprobar el 
                         acceso a los diferentes controladores*/
                        Session["Rol"] = login.Id_Rol.ToString();

                        /*Se le asigna el nombre de usuario a la variable de Session["Id_Usuario"],
                         para eventualmente ser usada en el controller visitas*/
                        Session["Id_Usuario"] = login.NombreUsuario.ToString();

                        /*Se crea la cookie que almacena el inicio de sesion*/
                        FormsAuthentication.SetAuthCookie(login.NombreUsuario, false);

                        if (login.Id_Rol == 1 || login.Id_Rol == 2)
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("Create", "Tbl_Aprendices");
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