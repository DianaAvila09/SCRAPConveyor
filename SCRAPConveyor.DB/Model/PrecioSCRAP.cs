//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCRAPConveyor.DB.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PrecioSCRAP
    {
        public long id { get; set; }
        public string tipo { get; set; }
        public string cliente { get; set; }
        public string mes { get; set; }
        public string año { get; set; }
        public Nullable<decimal> precio { get; set; }
        public Nullable<System.DateTime> fechaCreacion { get; set; }
        public Nullable<bool> activo { get; set; }
        public string moneda { get; set; }
    }
}
