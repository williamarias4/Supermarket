using System;
using Oracle.ManagedDataAccess.Client;

namespace Supermercado.DAO
{
    public class CRUD:ConexionBD
    {
        public void insertarProducto(int _idArea, int _ean, string _descripcion, int _precio, int _cantidad)
        {
            OracleConnection connection = GetConnection();
            string query = string.Format("INSERT INTO producto (pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) VALUES (producto_sq.nextval,'{0}','{1}','{2}','{3}','{4}');", _idArea, _ean, _descripcion, _precio, _cantidad);
            OracleCommand accion = new OracleCommand(query, connection);
            accion.CommandTimeout = 60;
            connection.Open();
            //Ejecuta el query
            accion.ExecuteReader();
            //Cierra conexion
            connection.Close();
        }
    }
}
