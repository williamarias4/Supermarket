using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class Factura
    {
        public long idFactura { get; set; }
        public int numeroCaja { get; set; }
        public float total { get; set; }
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string fecha { get; set; }
        public List<DetalleFactura> detalle { get; set; }


    }
}
