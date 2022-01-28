using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCRAPConveyor.Web.origen
{
    public class sp_GetList_BasculasFactura_Result
    {
        public long id { get; set; }
        public Nullable<int> boleto { get; set; }
        public string placas { get; set; }
        public string chofer { get; set; }
        public string usuario { get; set; }
        public string empresa { get; set; }
        public string transportista { get; set; }
        public Nullable<System.DateTime> fechaHoraEntrada { get; set; }
        public Nullable<decimal> pesoEntrada { get; set; }
        public Nullable<System.DateTime> fechaHoraSalida { get; set; }
        public Nullable<decimal> pesoSalida { get; set; }
        public Nullable<decimal> pesoTara { get; set; }
        public Nullable<int> bascula { get; set; }
        public string tagCamion { get; set; }
        public Nullable<System.DateTime> fechaCreacion { get; set; }
        public Nullable<bool> factura { get; set; }
        public string numFactura { get; set; }
        public string producto { get; set; }
        public Nullable<bool> documento { get; set; }
        public string numDocumento { get; set; }
        public Nullable<System.DateTime> fechaFactura { get; set; }
        public Nullable<System.DateTime> fechaDocumento { get; set; }
        public string tipoMaterial { get; set; }
        public string tolva { get; set; }
        public string descSAP { get; set; }
        public string cortina { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
        public Nullable<int> idLinea { get; set; }
        public string material { get; set; }
        public Nullable<int> material2 { get; set; }
        public string Linea { get; set; }
    }
}