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
    
    public partial class TypeMaterial
    {
        public long id { get; set; }
        public string typeMaterial1 { get; set; }
        public string material { get; set; }
        public string code { get; set; }
        public Nullable<int> receta { get; set; }
        public string materialSAP { get; set; }
    }
}
