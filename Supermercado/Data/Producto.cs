using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class Producto
    {
        public Producto(int _id, int _idArea, long _ean, string _descripcion, float _precio, int _cantidad)
        {
            this.id = _id;
            this.idArea = _idArea;
            this.ean = _ean;
            this.descripcion = _descripcion;
            this.precio = _precio;
            this.cantidad = _cantidad;
        }
        public Producto() { }
        public int id { get; set; }
        public int idArea { get; set; }
        public long ean { get; set; }
        public string descripcion { get; set; }
        public float precio { get; set; }
        public int cantidad { get; set; }
        public string cantidadVendida { get; set; }
    }
}
