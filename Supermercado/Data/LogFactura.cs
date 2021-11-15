using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class LogFactura : Log
    {
        public int idFactura { get; set; }
        public int numeroCaja { get; set; }
        public float total { get; set; }
        public List<DetalleFacturaLog> detelle { get; set; }
    }

    public class DetalleFacturaLog 
    { 
        public long EAN { get; set; }
        public int cantidad { get; set; }
        public long subTotal { get; set; }
    }
}
