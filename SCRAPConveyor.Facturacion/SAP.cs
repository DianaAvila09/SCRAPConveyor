using SAP.Middleware.Connector;
using SCRAPConveyor.Facturacion.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SCRAPConveyor.Facturacion
{
    class SAP
    {
        protected String Entorno;
        public SAP() {
            Entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
        }
        public Tuple<List<ET_MENSAJES>, string> ZFIFM_CREAR_PED_SCRAP(string I_SALES_ORG, string I_DIS_CHL, string I_DIVISION, string I_SOLD_TO, string I_PO_NUMBER, List<IT_MATERIALES> MATERIALES)
        {
            string strfecha = DateTime.Now.ToShortDateString();
            DateTime datevalue;
            DateTime.TryParse(strfecha, out datevalue);
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            IRfcFunction Bapi = SapRfcRepository.CreateFunction("ZFIFM_CREAR_PED_SCRAP");
            if (I_SALES_ORG != "") Bapi.SetValue("I_SALES_ORG", I_SALES_ORG);
            if (I_DIS_CHL != "") Bapi.SetValue("I_DIS_CHL", I_DIS_CHL);
            if (I_DIVISION != "") Bapi.SetValue("I_DIVISION", I_DIVISION);
            if (I_SOLD_TO != "") Bapi.SetValue("I_SOLD_TO", I_SOLD_TO);
            if (I_PO_NUMBER != "") Bapi.SetValue("I_PO_NUMBER", I_PO_NUMBER);

            IRfcTable IT_MATERIALES = Bapi.GetTable("IT_MATERIALES");
            foreach (IT_MATERIALES item in MATERIALES)
            {
                IT_MATERIALES.Append();
                IT_MATERIALES.SetValue("MATERIAL", item.MATERIAL);
                IT_MATERIALES.SetValue("DESCRIPCION", item.DESCRIPCION);
                IT_MATERIALES.SetValue("CANTIDAD", item.CANTIDAD);
                IT_MATERIALES.SetValue("PRECIO", item.PRECIO);
                IT_MATERIALES.SetValue("UNIDAD_PRECIO", item.UNIDAD_PRECIO);
                IT_MATERIALES.SetValue("MONEDA", item.MONEDA);
            }

            Bapi.Invoke(SapRfcDestination);

            IRfcTable registros = Bapi.GetTable("ET_MENSAJES");
            String E_DOCUMENTO = Bapi.GetString("E_DOCUMENTO");
            List<ET_MENSAJES> mensajes = new List<ET_MENSAJES>();
            foreach (var item in registros.ToList())
            {
                mensajes.Add(new ET_MENSAJES() { TYPE = item.GetString("TYPE"), NUMBER = item.GetString("NUMBER"), ID = item.GetString("ID"), MESSAGE = item.GetString("MESSAGE") });
            }
            return Tuple.Create(mensajes, E_DOCUMENTO);
        }

        public Tuple<List<ET_MENSAJES>,List<ET_DOCUMENTOS>> ZFIFM_CREAR_FRA_SCRAP(string I_PLANTA, string IT_ORD_VTA)
        {
            string strfecha = DateTime.Now.ToShortDateString();
            DateTime datevalue;
            DateTime.TryParse(strfecha, out datevalue);
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            IRfcFunction Bapi = SapRfcRepository.CreateFunction("ZFIFM_CREAR_FRA_SCRAP");
            if (I_PLANTA != "") Bapi.SetValue("I_PLANTA", I_PLANTA);
            if (IT_ORD_VTA != "") Bapi.SetValue("IT_ORD_VTA", IT_ORD_VTA);

            Bapi.Invoke(SapRfcDestination);

            IRfcTable ET_MENSAJES = Bapi.GetTable("ET_MENSAJES");
            IRfcTable ET_DOCUMENTOS = Bapi.GetTable("ET_DOCUMENTOS");
            List<ET_MENSAJES> mensajes = new List<ET_MENSAJES>();
            foreach (var item in ET_MENSAJES.ToList())
            {
                mensajes.Add(new ET_MENSAJES() { TYPE = item.GetString("TYPE"), NUMBER = item.GetString("NUMBER"), ID = item.GetString("ID"), MESSAGE = item.GetString("MESSAGE") });
            }
            List<ET_DOCUMENTOS> documentos = new List<ET_DOCUMENTOS>();
            foreach (var item in ET_DOCUMENTOS.ToList())
            {
                documentos.Add(new ET_DOCUMENTOS() { DOCUMENTO = item.GetString("DOCUMENTO"), POSICION = item.GetString("POSICION"), FACTURA = item.GetString("FACTURA"), MATERIAL = item.GetString("MATERIAL"), DESC_MAT = item.GetString("DESC_MAT")  });
            }
            return Tuple.Create(mensajes, documentos);
        }
    }
}
