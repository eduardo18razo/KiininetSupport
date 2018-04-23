using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceEncuesta;
using KiiniHelp.ServiceSistemaTipoEncuesta;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Encuestas
{
    public partial class UcAltaEncuesta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        readonly ServiceTipoEncuestaClient _serviceSistemaTipoEncuesta = new ServiceTipoEncuestaClient();
        readonly ServiceEncuestaClient _servicioEncuesta = new ServiceEncuestaClient();
        private List<string> _lstError = new List<string>();
        private List<string> Alerta
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        private void LimpiarEncuesta()
        {
            try
            {
                Session["Encuesta"] = new Encuesta { EncuestaPregunta = new List<EncuestaPregunta>() };
                ddlTipoEncuesta.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                txtTitulo.Text = string.Empty;
                txtTituloCliente.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                LlenaPreguntas();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ObtenerEncuesta()
        {
            try
            {
                Encuesta encuesta = _servicioEncuesta.ObtenerEncuestaById(IdEncuesta);
                Session["Encuesta"] = encuesta;
                ddlTipoEncuesta.SelectedValue = encuesta.IdTipoEncuesta.ToString();
                txtTitulo.Text = encuesta.Titulo;
                txtTituloCliente.Text = encuesta.TituloCliente;
                txtDescripcion.Text = encuesta.Descripcion;
                LlenaPreguntas();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LlenaPreguntas()
        {
            try
            {
                Encuesta tmpEncuesta = ((Encuesta)Session["Encuesta"]);
                rptPreguntas.DataSource = tmpEncuesta.EncuestaPregunta;
                rptPreguntas.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<EncuestaPregunta> ObtenerPreguntas()
        {
            List<EncuestaPregunta> result = null;
            try
            {
                result = new List<EncuestaPregunta>();
                foreach (RepeaterItem item in rptPreguntas.Items)
                {
                    result.Add(new EncuestaPregunta
                    {
                        Pregunta = ((TextBox)item.FindControl("txtPregunta")).Text.Trim(),
                        Ponderacion = decimal.Parse(((TextBox)item.FindControl("txtPonderacion")).Text == string.Empty ? "0" : ((TextBox)item.FindControl("txtPonderacion")).Text)
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }

        private decimal ObtieneTotalPonderacion()
        {
            decimal result;
            try
            {
                result = ObtenerPreguntas().Sum(s => s.Ponderacion);
                if (result > 100)
                    throw new Exception("Recuerda que la ponderación debe sumar 100.");
                hfTotalPonderacion.Value = result.ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }

        public int IdEncuesta
        {
            get { return int.Parse(hfIdEncuesta.Value); }
            set
            {
                hfIdEncuesta.Value = value.ToString();
                ObtenerEncuesta();
            }
        }

        public int IdTipoEncuesta
        {
            get { return int.Parse(ddlTipoEncuesta.SelectedValue); }
            set { ddlTipoEncuesta.SelectedValue = value.ToString(); }
        }
        public bool Alta
        {
            get { return bool.Parse(hfAlta.Value); }
            set
            {
                hfAlta.Value = value.ToString();
                ddlTipoEncuesta.Enabled = value;
                if (!value)
                {
                    foreach (RepeaterItem item in rptPreguntas.Items)
                    {

                        ((TextBox)item.FindControl("txtPregunta")).Enabled = false;
                        ((TextBox)item.FindControl("txtPonderacion")).Enabled = false;
                        ((LinkButton)item.FindControl("btnSubir")).Enabled = false;
                        ((LinkButton)item.FindControl("btnBajar")).Enabled = false;
                        ((LinkButton)rptPreguntas.Controls[rptPreguntas.Controls.Count - 1].Controls[0].FindControl("btnagregarPregunta")).Enabled = false;
                    }
                }
                else
                {
                    if (IdEncuesta == 0)
                    {
                        Session["Encuesta"] = new Encuesta { EncuestaPregunta = new List<EncuestaPregunta>() };
                        LlenaPreguntas();
                    }
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Metodos.LlenaComboCatalogo(ddlTipoEncuesta, _serviceSistemaTipoEncuesta.ObtenerTiposEncuesta(true));
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnAddPregunta_OnClick(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rptPreguntas.Items)
                {
                    if (((TextBox)item.FindControl("txtPregunta")).Text == string.Empty)
                        throw new Exception("Debe especificar una pregunta.");
                    if (((TextBox)item.FindControl("txtPonderacion")).Text == string.Empty)
                        throw new Exception("Debe especificar ponderación para la pregunta.");
                }

                Encuesta tmpEncuesta = ((Encuesta)Session["Encuesta"]);

                if (tmpEncuesta.EncuestaPregunta == null)
                    tmpEncuesta.EncuestaPregunta = new List<EncuestaPregunta>();
                else
                    tmpEncuesta.EncuestaPregunta = ObtenerPreguntas();

                if (int.Parse(ddlTipoEncuesta.SelectedValue) == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore && tmpEncuesta.EncuestaPregunta.Count >= 1)
                    throw new Exception("Solo puede agregar una pregunta a este tipo de encuesta.");

                ObtieneTotalPonderacion();

                tmpEncuesta.EncuestaPregunta.Add(new EncuestaPregunta());
                Session["Encuesta"] = tmpEncuesta;
                LlenaPreguntas();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarEncuesta();
                if (OnCancelarModal != null)
                    OnCancelarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void rptPreguntas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Footer)
                {
                    ((TextBox)e.Item.FindControl("txtTotal")).Text = ObtieneTotalPonderacion().ToString();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void txtPonderacion_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)rptPreguntas.Controls[rptPreguntas.Controls.Count - 1].Controls[0].FindControl("txtTotal")).Text = ObtieneTotalPonderacion().ToString();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnSubir_OnClick(object sender, EventArgs e)
        {
            try
            {
                Encuesta tmpMascara = ((Encuesta)Session["Encuesta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                TextBox txtPregunta = (TextBox)item.FindControl("txtPregunta");
                TextBox txtPonderacion = (TextBox)item.FindControl("txtPonderacion");
                decimal ponderacion = decimal.Parse(txtPonderacion.Text);
                EncuestaPregunta campoEditar = txtPregunta.Text.Trim() == string.Empty ? tmpMascara.EncuestaPregunta.SingleOrDefault(s => s.Pregunta == null && s.Ponderacion == ponderacion) : tmpMascara.EncuestaPregunta.SingleOrDefault(s => s.Pregunta == txtPregunta.Text && s.Ponderacion == ponderacion);
                if (campoEditar == null) return;
                int indexActual = tmpMascara.EncuestaPregunta.IndexOf(campoEditar);
                if (indexActual == 0) return;
                tmpMascara.EncuestaPregunta.Remove(campoEditar);
                tmpMascara.EncuestaPregunta.Insert(indexActual - 1, campoEditar);
                Session["Encuesta"] = tmpMascara;
                LlenaPreguntas();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnBajar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Encuesta tmpMascara = ((Encuesta)Session["Encuesta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                TextBox txtPregunta = (TextBox)item.FindControl("txtPregunta");
                TextBox txtPonderacion = (TextBox)item.FindControl("txtPonderacion");
                decimal ponderacion = decimal.Parse(txtPonderacion.Text);
                EncuestaPregunta campoEditar = txtPregunta.Text.Trim() == string.Empty ? tmpMascara.EncuestaPregunta.SingleOrDefault(s => s.Pregunta == null && s.Ponderacion == ponderacion) : tmpMascara.EncuestaPregunta.SingleOrDefault(s => s.Pregunta == txtPregunta.Text && s.Ponderacion == ponderacion);
                if (campoEditar == null) return;
                int indexActual = tmpMascara.EncuestaPregunta.IndexOf(campoEditar);
                if (indexActual == tmpMascara.EncuestaPregunta.Count - 1) return;
                tmpMascara.EncuestaPregunta.Remove(campoEditar);
                tmpMascara.EncuestaPregunta.Insert(indexActual + 1, campoEditar);
                Session["Encuesta"] = tmpMascara;
                LlenaPreguntas();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void ddlTipoEncuesta_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                divPreguntas.Visible = int.Parse(ddlTipoEncuesta.SelectedValue) != (int)BusinessVariables.EnumTipoEncuesta.PromotorScore;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            try
            {
                Encuesta previewEncuesta = ((Encuesta)Session["Encuesta"]);
                previewEncuesta.IdTipoEncuesta = int.Parse(ddlTipoEncuesta.SelectedValue);
                previewEncuesta.Titulo = txtTitulo.Text;
                previewEncuesta.TituloCliente = txtTituloCliente.Text;
                previewEncuesta.Descripcion = txtDescripcion.Text;

                if (int.Parse(ddlTipoEncuesta.SelectedValue) != (int)BusinessVariables.EnumTipoEncuesta.PromotorScore)
                {
                    List<EncuestaPregunta> preguntas = ObtenerPreguntas();
                    preguntas.RemoveAll(w => w.Pregunta == string.Empty);
                    previewEncuesta.EncuestaPregunta = preguntas;
                }

                else
                {
                    previewEncuesta.EncuestaPregunta = new List<EncuestaPregunta>
                        {
                            new EncuestaPregunta
                            {
                                Pregunta = "¿Cuán probable es que recomiende el producto o servicio a un familiar o amigo?",
                                Ponderacion = 100
                            }
                        };
                }
                Session["PreviewEncuesta"] = previewEncuesta;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + "/Users/Administracion/Encuestas/FrmPreview.aspx','_blank');", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnGuardarEncuesta_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoEncuesta.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un tipo para la encuesta");
                if (txtTitulo.Text == string.Empty)
                    throw new Exception("Especifique un titúlo para la encuesta");
                if (txtTituloCliente.Text == string.Empty)
                    throw new Exception("Especifique un titúlo para el cliente.");
                if (txtTituloCliente.Text == string.Empty)
                    throw new Exception("Especifique una Descripción.");
                if (int.Parse(ddlTipoEncuesta.SelectedValue) != (int)BusinessVariables.EnumTipoEncuesta.PromotorScore)
                    if (ObtieneTotalPonderacion() < 100 || ObtieneTotalPonderacion() > 100)
                        throw new Exception("La ponderación total debe sumar 100.");
                if (Alta)
                {
                    Encuesta nuevaEncuesta = ((Encuesta)Session["Encuesta"]);
                    nuevaEncuesta.TipoEncuesta = null;
                    nuevaEncuesta.Id = 0;
                    nuevaEncuesta.IdTipoEncuesta = int.Parse(ddlTipoEncuesta.SelectedValue);
                    nuevaEncuesta.Titulo = txtTitulo.Text;
                    nuevaEncuesta.TituloCliente = txtTituloCliente.Text;
                    //Todo: Cambiar al txt 
                    nuevaEncuesta.Descripcion = txtDescripcion.Text;
                    if (int.Parse(ddlTipoEncuesta.SelectedValue) != (int)BusinessVariables.EnumTipoEncuesta.PromotorScore)
                    {
                        List<EncuestaPregunta> preguntas = ObtenerPreguntas();
                        preguntas.RemoveAll(w => w.Pregunta == string.Empty);
                        nuevaEncuesta.EncuestaPregunta = preguntas;
                        //nuevaEncuesta.EncuestaPregunta = ObtenerPreguntas();
                    }
                    else
                    {
                        nuevaEncuesta.EncuestaPregunta = new List<EncuestaPregunta>
                        {
                            new EncuestaPregunta
                            {
                                Pregunta = "¿Cuán probable es que recomiende el producto o servicio a un familiar o amigo?",
                                Ponderacion = 100
                            }
                        };
                    }
                    nuevaEncuesta.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                    _servicioEncuesta.GuardarEncuesta(nuevaEncuesta);
                }
                else
                {
                    lblOperacion.Text = "Editar Encuesta";
                    Encuesta encuestaActualizar = ((Encuesta)Session["Encuesta"]);
                    encuestaActualizar.TipoEncuesta = null;
                    encuestaActualizar.IdTipoEncuesta = int.Parse(ddlTipoEncuesta.SelectedValue);
                    encuestaActualizar.Titulo = txtTitulo.Text;
                    encuestaActualizar.TituloCliente = txtTituloCliente.Text;
                    encuestaActualizar.Descripcion = txtDescripcion.Text;
                    if (int.Parse(ddlTipoEncuesta.SelectedValue) != (int)BusinessVariables.EnumTipoEncuesta.PromotorScore)
                        encuestaActualizar.EncuestaPregunta = ObtenerPreguntas();
                    else
                    {
                        encuestaActualizar.EncuestaPregunta = new List<EncuestaPregunta>
                        {
                            new EncuestaPregunta
                            {
                                Pregunta =
                                    "¿Cuán probable es que recomiende el producto o servicio a un familiar o amigo?",
                                Ponderacion = 100
                            }
                        };
                    }
                    encuestaActualizar.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                    _servicioEncuesta.ActualizarEncuesta(encuestaActualizar);
                }

                LimpiarEncuesta();
                if (OnAceptarModal != null)
                    OnAceptarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnLimpiarEncuesta_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarEncuesta();
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarEncuesta();
                if (OnCancelarModal != null)
                    OnCancelarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }
    }
}