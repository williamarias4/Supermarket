using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class ProductoFresco : Producto
    {
        public int IdFresco { get; set; }
        public int PLU { get; set; }
        public float Peso { get; set; }
    }
}
