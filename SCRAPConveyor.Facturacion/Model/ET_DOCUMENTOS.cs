using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCRAPConveyor.Facturacion.Model
{
    public class ET_DOCUMENTOS
    {
        protected string _DOCUMENTO;
        protected string _POSICION;
        protected string _FACTURA;
        protected string _MATERIAL;
        protected string _DESC_MAT;

        public string DOCUMENTO
        {
            get { return _DOCUMENTO; }
            set { _DOCUMENTO = value; }
        }
        public string POSICION
        {
            get { return _POSICION; }
            set { _POSICION = value; }
        }
        public string FACTURA
        {
            get { return _FACTURA; }
            set { _FACTURA = value; }
        }
        public string MATERIAL
        {
            get { return _MATERIAL; }
            set { _MATERIAL = value; }
        }
        public string DESC_MAT
        {
            get { return _DESC_MAT; }
            set { _DESC_MAT = value; }
        }
    }
}
