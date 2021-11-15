using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class LogMovimiento : Log
    {
        public string tabla { get; set; }
        public string tipoTransaccion { get; set; }
        public string descripcion { get; set; }
    }
}
