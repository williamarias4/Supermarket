using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Supermercado.Data;

namespace Supermercado.DAO
{
    public class CRUD : ConexionBD
    {
        public void insertarProducto(int _idArea, long _ean, string _descripcion, int _precio, int _cantidad)
        {
            try
            {
                OracleConnection connection = GetConnection();
                string query = string.Format("INSERT INTO producto (pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) VALUES (producto_sq.nextval,'{0}','{1}','{2}','{3}','{4}');", _idArea, _ean, _descripcion, _precio, _cantidad);
                query = "INSERT INTO super.producto (pk_idproducto, fk_idarea, ean, descripcion, precio, cantidad) VALUES (18, 1, 3693693693693,'aceite',1800, 30)";
                OracleCommand accion = new OracleCommand(query, connection);
                accion.CommandTimeout = 60;
                connection.Open();
                //Ejecuta el query
                accion.ExecuteNonQuery();
                //Cierra conexion
                connection.Close();
            }
            catch (Exception ex)
            {

                
            }
            
        }
    }
}
