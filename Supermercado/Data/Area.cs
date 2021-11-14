using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Supermercado.Data
{
    public class Area
    {
        public Area(int _idArea, string _descripcion)
        {
            this.idArea = _idArea;
            this.descripcion = _descripcion;
        }
        public Area() { }
        public int idArea { get; set; }
        public string descripcion { get; set; }
    }
}
