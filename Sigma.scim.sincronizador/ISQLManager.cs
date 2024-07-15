using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace ThorSqlCore
{
    public interface ISQLManager
    {
        string EjecutaComando(string Comando, ListDictionary Parametros);
        DataTable EjecutaConsulta(string Comando, ListDictionary Parametros);
        List<T> EjecutaConsultaEntity<T>(string Comando, ListDictionary Parametros);
    }
}