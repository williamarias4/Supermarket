using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class LogCajero : Log
    {
        public int numeroCaja { get; set; }
        public long idFactura { get; set; }
    }
}
