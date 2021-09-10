using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCRAPConveyor.Facturacion.Model
{
    public class IT_MATERIALES
    {
        protected string _MATERIAL;
        protected string _DESCRIPCION;
        protected decimal _CANTIDAD;
        protected decimal _PRECIO;
        protected int _UNIDAD_PRECIO;
        protected string _MONEDA;

        public string MATERIAL
        {
            get { return _MATERIAL; }
            set { _MATERIAL = value; }
        }
        public decimal CANTIDAD
        {
            get { return _CANTIDAD; }
            set { _CANTIDAD = value; }
        }
        public decimal PRECIO
        {
            get { return _PRECIO; }
            set { _PRECIO = value; }
        }
        public int UNIDAD_PRECIO
        {
            get { return _UNIDAD_PRECIO; }
            set { _UNIDAD_PRECIO = value; }
        }
        public string DESCRIPCION
        {
            get { return _DESCRIPCION; }
            set { _DESCRIPCION = value; }
        }
        public string MONEDA
        {
            get { return _MONEDA; }
            set { _MONEDA = value; }
        }
    }
}
