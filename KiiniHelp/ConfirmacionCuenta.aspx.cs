using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp
{
    public partial class ConfirmacionCuenta : System.Web.UI.Page
    {
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
        {
            set
            {
                panelAlertaGeneral.Visible = value.Any();
                if (!panelAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value.Select(s => new { Detalle = s }).ToList();
                rptErrorGeneral.DataBind();
            }
        }

        private void SendNotificacion(int idUsuario, int idTelefono)
        {
            try
            {

                _servicioUsuarios.EnviaCodigoVerificacionSms(idUsuario, (int)BusinessVariables.EnumTipoLink.Confirmacion, idTelefono);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ValidaCaptura()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (txtContrasena.Text.Trim() == string.Empty)
                    sb.AppendLine("<li>Contraseña es campo obligatorio</li>");
                if (txtContrasena.Text.Trim() != txtConfirmaContrasena.Text.Trim())
                    sb.AppendLine("<li>Las contraseñas no coinciden</li>");
                foreach (RepeaterItem item in rptConfirmacion.Items)
                {
                    TextBox txtcodigo = (TextBox)item.FindControl("txtCodigo");
                    TextBox txttelefono = (TextBox)item.FindControl("txtNumeroEdit");
                    if (txtcodigo.Text.Trim() == string.Empty)
                        sb.AppendLine(string.Format("<li>el numero de telefono {0} no ha sido confirmado</li>", txttelefono.Text));
                }
                if (rptPreguntas.Items.Count <= 0)
                    sb.AppendLine("<li>Debe especificar al menos una pregunta</li>");
                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Datos Generales</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void CargaTelefonosObligatorios(int idUsuario)
        {
            try
            {
                Usuario userData = _servicioUsuarios.ObtenerDetalleUsuario(idUsuario);
                rptConfirmacion.DataSource = userData.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular && w.Obligatorio && !w.Confirmado).OrderBy(o => o.Id).Take(1);
                rptConfirmacion.DataBind();            
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                if (!IsPostBack)
                {
                    Session["PreguntaReto"] = null;
                    if (Request.Params["confirmacionalta"] != null)
                    {
                        string[] values = Request.Params["confirmacionalta"].Split('_');
                        if (!_servicioUsuarios.ValidaConfirmacion(int.Parse(values[0]), values[1]))
                        {
                            Response.Redirect(ResolveUrl("~/Default.aspx"));
                        }
                        CargaTelefonosObligatorios(int.Parse(values[0]));
                    }
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnChangeNumber_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                RepeaterItem item = ((RepeaterItem)btn.NamingContainer);
                TextBox txtNumero = (TextBox)item.FindControl("txtNumeroEdit");
                Button btnSend = (Button)item.FindControl("btnSendNotification");
                switch (btn.CommandArgument)
                {
                    case "0":
                        txtNumero.ReadOnly = false;
                        btn.CssClass = "btn btn-sm btn-success";
                        btn.Text = "Aceptar";
                        btn.CommandArgument = "1";
                        btnSend.Enabled = false;
                        break;
                    case "1":
                        Label lblId = (Label)item.FindControl("lblId");
                        Label lblIdUsuario = (Label)item.FindControl("lblIdUsuario");
                        _servicioUsuarios.ActualizarTelefono(int.Parse(lblIdUsuario.Text), int.Parse(lblId.Text), txtNumero.Text);
                        txtNumero.ReadOnly = true;
                        btn.CssClass = "btn btn-sm btn-primary";
                        btn.Text = "Cambiar Numero";
                        btn.CommandArgument = "0";
                        btnSend.Enabled = true;
                        break;
                }

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnSendNotification_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                RepeaterItem item = ((RepeaterItem)btn.NamingContainer);
                Label lblId = (Label)item.FindControl("lblId");
                Label lblIdUsuario = (Label)item.FindControl("lblIdUsuario");
                SendNotificacion(int.Parse(lblIdUsuario.Text), int.Parse(lblId.Text));
                ((Button)sender).Enabled = false;
                tmpSendNotificacion.Enabled = true;

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnAddPregunta_OnClick();
                ValidaCaptura();
                new ServiceSecurityClient().ValidaPassword(txtContrasena.Text.Trim());
                string result = string.Empty;
                Dictionary<int, string> confirmaciones = new Dictionary<int, string>();
                foreach (RepeaterItem item in rptConfirmacion.Items)
                {
                    Label lblIdTelefono = (Label)item.FindControl("lblId");
                    Label lblIdUsuario = (Label)item.FindControl("lblIdUsuario");
                    TextBox txtcodigo = (TextBox)item.FindControl("txtCodigo");
                    result += _servicioUsuarios.ValidaCodigoVerificacionSms(int.Parse(lblIdUsuario.Text), (int)BusinessVariables.EnumTipoLink.Confirmacion, int.Parse(lblIdTelefono.Text), txtcodigo.Text);
                    confirmaciones.Add(int.Parse(lblIdTelefono.Text), txtcodigo.Text.Trim());
                }
                if (result.Trim() != string.Empty)
                    throw new Exception(result);

                _servicioUsuarios.ConfirmaCuenta(int.Parse(Request.Params["confirmacionalta"].Split('_')[0]), txtContrasena.Text.Trim(), confirmaciones, (List<PreguntaReto>)Session["PreguntaReto"], Request.Params["confirmacionalta"].Split('_')[1]);
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void btnAddPregunta_OnClick() // object sender, EventArgs e
        {
            try
            {

                if (txtPregunta.Text.Trim() == string.Empty)
                    throw new Exception("Especifique una pregunta");
                if (txtRespuesta.Text.Trim() == string.Empty)
                    throw new Exception("Especifique una respuesta");

                List<PreguntaReto> tmpPreguntas = ((List<PreguntaReto>)Session["PreguntaReto"]) ?? new List<PreguntaReto>();

                if (txtIdPregunta.Text.Trim() == string.Empty)
                    tmpPreguntas.Add(new PreguntaReto
                    {
                        Id = tmpPreguntas.Count + 1,
                        Pregunta = txtPregunta.Text.Trim(),
                        Respuesta = txtRespuesta.Text.Trim()
                    });
                else
                {
                    PreguntaReto pregunta = tmpPreguntas.SingleOrDefault(s => s.Id == Convert.ToInt32(txtIdPregunta.Text.Trim()));
                    if (pregunta != null)
                    {
                        pregunta.Pregunta = txtPregunta.Text.Trim();
                        pregunta.Respuesta = txtRespuesta.Text.Trim();
                    }
                }


                rptPreguntas.DataSource = tmpPreguntas;
                rptPreguntas.DataBind();
                Session["PreguntaReto"] = tmpPreguntas;
                txtIdPregunta.Text = string.Empty;
                txtPregunta.Text = string.Empty;
                txtRespuesta.Text = string.Empty;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                PreguntaReto pregunta = ((List<PreguntaReto>)Session["PreguntaReto"]).SingleOrDefault(s => s.Id == Convert.ToInt32(((LinkButton)sender).CommandArgument));
                if (pregunta != null)
                {
                    txtIdPregunta.Text = pregunta.Id.ToString();
                    txtPregunta.Text = pregunta.Pregunta;
                    txtRespuesta.Text = pregunta.Respuesta;
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        protected void tmpSendNotificacion_OnTick(object sender, EventArgs e)
        {
            try
            {
                tmpSendNotificacion.Enabled = false;
                foreach (RepeaterItem item in rptConfirmacion.Items)
                {
                    ((Button)item.FindControl("btnSendNotification")).Enabled = true;
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}