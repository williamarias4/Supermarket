using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class ProductoFresco : Producto
    {
        public ProductoFresco(Producto _p, int _idFresco, int _PLU, float _Peso)
        {
            this.id = _p.id;
            this.idArea = _p.idArea;
            this.ean = _p.ean;
            this.descripcion = _p.descripcion;
            this.precio = _p.precio;
            this.cantidad = _p.cantidad;
            this.IdFresco = _idFresco;
            this.PLU = _PLU;
            this.Peso = _Peso;
        }
        public ProductoFresco() { }
        public int IdFresco { get; set; }
        public int PLU { get; set; }
        public float Peso { get; set; }
    }
}
