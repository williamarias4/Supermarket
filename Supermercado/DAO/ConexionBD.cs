using System;
using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;


namespace Supermercado.DAO
{
    public class ConexionBD
    {
        private static string connectionString = @"Data Source=(DESCRIPTION=
    (ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(Host=localhost )(PORT=1521)))
    (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE))); User ID=SUPER; PASSWORD=superUsuario;";
        protected Oracle.ManagedDataAccess.Client.OracleConnection GetConnection()
        {    
            return new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
        }
    }
}
