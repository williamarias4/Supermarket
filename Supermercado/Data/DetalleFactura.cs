using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class DetalleFactura
    {
        public int id { get; set; }
        public int idArea { get; set; }
        public long ean { get; set; }
        public string descripcion { get; set; }
        public float precio { get; set; }
        public int cantidad { get; set; }
        public float subtotal { get; set; }
    }
}
