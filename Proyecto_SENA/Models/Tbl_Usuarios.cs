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
    
    public partial class Tbl_Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Usuarios()
        {
            this.Tbl_Instructores = new HashSet<Tbl_Instructores>();
        }
    
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public Nullable<int> Id_Rol { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Instructores> Tbl_Instructores { get; set; }
        public virtual Tbl_Roles Tbl_Roles { get; set; }
    }
}