//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyecto_SENA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Empresas
    {
        public int Id_Empresa { get; set; }
        public string Razon_Social { get; set; }
        public string Nit { get; set; }
        public string Direccion { get; set; }
        public string Nombre_Jefe { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public Nullable<int> Id_Aprendiz { get; set; }
    
        public virtual Tbl_Aprendices Tbl_Aprendices { get; set; }
    }
}