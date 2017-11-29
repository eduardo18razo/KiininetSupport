using System.Collections.Generic;
using System.Linq;
using KiiniHelp.ServiceSistemaTipoTelefono;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Agentes
{
    public class BusquedaAgentes
    {
        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
        private readonly ServiceTipoTelefonoClient _servicioTipoTelefono = new ServiceTipoTelefonoClient();

        public BusquedaAgentes() { }
        public List<Usuario> GetUsers()
        {
            List<Usuario> itemsList = _servicioUsuario.ObtenerUsuarios(null);
            return itemsList;
        }

        public List<TipoTelefono> GetPhoneType()
        {
            List<TipoTelefono> itemsList = _servicioTipoTelefono.ObtenerTiposTelefono(false).Where(w=>w.Id == (int)BusinessVariables.EnumTipoTelefono.Celular).ToList();
            return itemsList;
        }
    }
}