using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Proyecto_SENA.Controllers
{
    public class Metodos
    {
        public static bool Cadena(string Text)
        {
            Regex cadena = new Regex(@"[A-Za-z]{3,}$");

            if(Text != null)
                if (cadena.IsMatch(Text))
                    return true;

            return false;
        }

        public static bool Numeros(string Number)
        {
            Regex numeros = new Regex(@"\d{7,}$");

            if (Number != null)
                if (numeros.IsMatch(Number))
                    return true;

            return false;
        }
    }
}