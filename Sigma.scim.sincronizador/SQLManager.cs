using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ThorSqlCore
{
    public class SQLManager : ISQLManager
    {
        private string _Conexion { get; }
        public SQLManager(string conexion)
        {
            _Conexion = conexion;
        }

        public CommandType TipoCommando { get; set; } = CommandType.Text;
        public int TimeOut { get; set; }


        public string EjecutaComando(string Comando, ListDictionary Parametros)
        {
            SqlConnection _conn = new SqlConnection(_Conexion);
            string _sql = Comando;   //model.consulta;
            SqlCommand _cm = CreaComando(_sql, _conn, Parametros);
            try
            {
                _conn.Open();
                _cm.ExecuteNonQuery();
            }
            finally
            {
                _conn.Close();
            }
            return "";
        }

        public DataTable EjecutaConsulta(string Comando, ListDictionary Parametros)
        {
            DataTable _result = new DataTable();
            SqlConnection _conn = new SqlConnection(_Conexion);
            string _sql = Comando;
            SqlDataAdapter _da = new SqlDataAdapter(_sql, _conn);
            System.Data.DataTable _dt = new System.Data.DataTable();
            _da.Fill(_dt);
            return _dt;
        }

        public List<T> EjecutaConsultaEntity<T>(string Comando, ListDictionary Parametros)
        {
            List<T> _res;
            SqlConnection _conn = new SqlConnection(_Conexion);
            SqlCommand _cm = CreaComando(Comando, _conn, Parametros);
            try
            {
                _conn.Open();
                using (var result = _cm.ExecuteReader())
                {
                    _res = MapToList<T>(result);
                }
            }
            finally
            {
                _conn.Close();
                _conn.Dispose();
            }
            return _res;
        }


        private SqlCommand CreaComando(string comando, SqlConnection conn, ListDictionary Parametros)
        {
            SqlCommand _cm = new SqlCommand(comando, conn);
            if (TipoCommando != CommandType.Text)
                _cm.CommandType = TipoCommando;
            if (TimeOut != 0)
                _cm.CommandTimeout = TimeOut;
            if (Parametros != null)
            {
                foreach (string key in Parametros.Keys)
                {
                    _cm.Parameters.AddWithValue(key, Parametros[key]);
                }
            }
            return _cm;
        }

        private List<T> MapToList<T>(SqlDataReader dr)
        {
            var objList = new List<T>();
            var props = typeof(T).GetRuntimeProperties();

            var colMapping = dr.GetColumnSchema()
                .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
                .ToDictionary(key => key.ColumnName.ToLower());

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in props)
                    {
                        try
                        {
                            if (colMapping.ContainsKey(prop.Name.ToLower()))
                            {
                                var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
                                prop.SetValue(obj, val == DBNull.Value ? null : val);
                            }
                            else
                            {
                                prop.SetValue(obj, null);
                            }
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (System.Collections.Generic.KeyNotFoundException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            prop.SetValue(obj, null);
                        }
                    }
                    objList.Add(obj);
                }
            }
            return objList;
        }
    }
}
