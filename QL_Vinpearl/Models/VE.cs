//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QL_Vinpearl.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VE()
        {
            this.CTHD = new HashSet<CTHD>();
        }
    
        public string maVe { get; set; }
        public string maDV { get; set; }
        public Nullable<bool> loaiVe { get; set; }
        public Nullable<decimal> giaTien { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHD> CTHD { get; set; }
        public virtual DICHVU DICHVU { get; set; }
    }
}
