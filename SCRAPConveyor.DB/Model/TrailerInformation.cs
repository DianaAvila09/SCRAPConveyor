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
    
    public partial class TrailerInformation
    {
        public long id { get; set; }
        public Nullable<bool> releaseMaterial { get; set; }
        public Nullable<bool> materialOnTheWay { get; set; }
        public Nullable<bool> mixedMaterial { get; set; }
        public Nullable<bool> reqTrailerExchange { get; set; }
        public Nullable<int> fillingLevel { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> trailerNumber { get; set; }
        public string tolvaNumber { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public string material { get; set; }
        public Nullable<bool> TrailerFull { get; set; }
        public Nullable<bool> doorClosed { get; set; }
        public string materialType { get; set; }
    }
}
