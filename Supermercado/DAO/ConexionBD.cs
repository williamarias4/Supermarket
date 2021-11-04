using System;
using Oracle.ManagedDataAccess.Client;

namespace Supermercado.DAO
{
    public class ConexionBD
    {
        protected OracleConnection GetConnection()
        {
            return new OracleConnection("DATA SOURCE = XE; PASSWORD = superUsuario; USER ID = SUPER;");
        }
    }
}
