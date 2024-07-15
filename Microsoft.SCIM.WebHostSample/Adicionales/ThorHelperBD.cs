using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Text;
using System.Linq;
using ThorSqlCore;

namespace Microsoft.SCIM.WebHostSample.Adicionales
{
    public static class ThorHelperBD
    {

        public static bool InsertaEntidad<T>(T valor)
        {
            SQLManager _dbejecutor = new SQLManager(ThorConfig.strConnection);
            _dbejecutor.TipoCommando = System.Data.CommandType.StoredProcedure;
            bool _Result = true;
            ListDictionary _params = new ListDictionary();
            if (valor is Core2EnterpriseUser)
            {
                Core2EnterpriseUser _user = valor as Core2EnterpriseUser;
                _params.Add("@upn", _user.UserName);
                _params.Add("@nombre", _user.DisplayName);
                _params.Add("@mail", _user.ElectronicMailAddresses.First<ElectronicMailAddress>().Value);
                _params.Add("@obj", System.Text.Json.JsonSerializer.Serialize (_user ) );
                _dbejecutor.EjecutaComando("Thor_Users_InsertaActualizaUsuario", _params );
            }
            else if (valor is Core2Group)
            {
                Core2Group _grupo = valor as Core2Group;
                _params.Add("@guid_grupo", _grupo.Identifier);
                _params.Add("@nombre", _grupo.DisplayName);
                _dbejecutor.EjecutaComando("Thor_Groups_InsertaActualizaGrupo", _params);
            }
            return true;
        }
    }
}


