using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCRAPConveyor.Facturacion.Model
{
    public class ET_MENSAJES
    {
        protected string _T;
        protected string _ID;
        protected string _NUM;
        protected string _MESSAGE;

        public string TYPE
        {
            get { return _T; }
            set { _T = value; }
        }
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string NUMBER
        {
            get { return _NUM; }
            set { _NUM = value; }
        }
        public string MESSAGE
        {
            get { return _MESSAGE; }
            set { _MESSAGE = value; }
        }
    }
}
