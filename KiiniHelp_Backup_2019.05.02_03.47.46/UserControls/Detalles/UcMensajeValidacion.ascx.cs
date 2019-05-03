using System;
using System.Web.UI;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcMensajeValidacion : UserControl, IControllerModal
    {
        public string Mensaje
        {
            set { lblMensaje.Text = value; }
        }
        public int IdUsuarioValidar
        {
            set { ucDetalleUsuario.IdUsuario = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ucDetalleUsuario.OnCancelarModal += ucDetalleUsuario_OnCancelarModal;
            }
            catch (Exception)
            {
                
            }
        }

        void ucDetalleUsuario_OnCancelarModal()
        {
            try
            {
                if (OnCancelarModal != null)
                    OnCancelarModal();
            }
            catch (Exception)
            {
            }
        }

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnCancelarModal != null)
                    OnCancelarModal();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}