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
    
    public partial class Tbl_Fichas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Fichas()
        {
            this.Tbl_Aprendices = new HashSet<Tbl_Aprendices>();
            this.Tbl_Visitas = new HashSet<Tbl_Visitas>();
        }
    
        public int Id_Ficha { get; set; }
        public Nullable<int> Numero_Ficha { get; set; }
        public Nullable<int> Id_Programa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Aprendices> Tbl_Aprendices { get; set; }
        public virtual Tbl_Programas Tbl_Programas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Visitas> Tbl_Visitas { get; set; }
    }
}