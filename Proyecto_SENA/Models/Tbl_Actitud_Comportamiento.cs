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
    
    public partial class Tbl_Actitud_Comportamiento
    {
        public int Id_Actitud { get; set; }
        public Nullable<int> Tipo_Informe { get; set; }
        public string Relaciones_Interpersonales { get; set; }
        public string Valoracion_Relaciones { get; set; }
        public string TrabajoEnEquipo { get; set; }
        public string Valoracion_Trabajo { get; set; }
        public string Solucion_De_Problemas { get; set; }
        public string Valoracion_Solucion { get; set; }
        public string Cumplimiento { get; set; }
        public string Valoracion_Cumplimiento { get; set; }
        public string Organizacion { get; set; }
        public string Valoracion_Organizacion { get; set; }
        public Nullable<int> Id_Aprendiz { get; set; }
    
        public virtual Tbl_Aprendices Tbl_Aprendices { get; set; }
    }
}
