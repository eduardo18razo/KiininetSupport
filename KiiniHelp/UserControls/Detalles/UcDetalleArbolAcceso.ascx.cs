using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleArbolAcceso : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
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

        public int IdArbolAcceso
        {
            get { return int.Parse(hfIdArbolAcceso.Value); }
            set
            {
                try
                {
                    hfIdArbolAcceso.Value = value.ToString();
                    ArbolAcceso arbol = _servicioArbolAcceso.ObtenerArbolAcceso(value);
                    if (arbol != null)
                    {
                        lblTitulo.Text = arbol.InventarioArbolAcceso.First().Descripcion;
                        lblCategoria.Text = arbol.Area.Descripcion;
                        //lblTipoUsuario.Text = arbol.TipoUsuario.Descripcion;

                        lblTipoUsuarioOpcion.Text = arbol.TipoUsuario.Descripcion;
                        lblTipificacion.Text = arbol.Tipificacion;
                        lblFormulario.Text = arbol.InventarioArbolAcceso.First().Mascara != null ? arbol.InventarioArbolAcceso.First().Mascara.Descripcion : string.Empty;
                        lblActivo.Text = arbol.Habilitado ? "Si" : "No";
                        lblPublico.Text = arbol.Publico ? "Si" : "No";
                        lblTituloOpcion.Text = arbol.InventarioArbolAcceso.First().Descripcion;

                        lblCategoriaOpcion.Text = arbol.Area.Descripcion;
                        lblNivel1.Text = arbol.Nivel1 != null ? arbol.Nivel1.Descripcion : string.Empty;
                        lblNivel2.Text = arbol.Nivel2 != null ? arbol.Nivel2.Descripcion : string.Empty;
                        lblNivel3.Text = arbol.Nivel3 != null ? arbol.Nivel3.Descripcion : string.Empty;
                        lblNivel4.Text = arbol.Nivel4 != null ? arbol.Nivel4.Descripcion : string.Empty;
                        lblNivel5.Text = arbol.Nivel5 != null ? arbol.Nivel5.Descripcion : string.Empty;
                        lblNivel6.Text = arbol.Nivel6 != null ? arbol.Nivel6.Descripcion : string.Empty;
                        lblNivel7.Text = arbol.Nivel7 != null ? arbol.Nivel7.Descripcion : string.Empty;

                        rptUsuarios.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptUsuarios.DataBind();

                        rptContenido.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptContenido.DataBind();

                        rptOperacion.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptOperacion.DataBind();

                        rptDesarrollo.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptDesarrollo.DataBind();

                        rptAtencion.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptAtencion.DataBind();

                        rptConsulta.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptConsulta.DataBind();

                        rptCategoria.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptCategoria.DataBind();

                        rptUniversal.DataSource = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal).Select(s => s.GrupoUsuario).Distinct().ToList();
                        rptUniversal.DataBind();

                        lblSlaTotal.Text = arbol.InventarioArbolAcceso.First().Sla != null ? arbol.InventarioArbolAcceso.First().Sla.TiempoHoraProceso.ToString() : string.Empty;
                        lblPrioridad.Text = arbol.Impacto != null ? arbol.Impacto.Prioridad.Descripcion : string.Empty;
                        lblImpacto.Text = arbol.Impacto != null ? arbol.Impacto.Urgencia.Descripcion : string.Empty;

                        rptInformeCategoria.DataSource = arbol.TiempoInformeArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría).Distinct().ToList();
                        rptInformeCategoria.DataBind();

                        rptInformeContenido.DataSource = arbol.TiempoInformeArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).Distinct().ToList();
                        rptInformeContenido.DataBind();

                        rptInformeDesarrollo.DataSource = arbol.TiempoInformeArbol.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).Distinct().ToList();
                        rptInformeDesarrollo.DataBind();

                        lblEncuesta.Text = arbol.InventarioArbolAcceso.First().Encuesta != null ? arbol.InventarioArbolAcceso.First().Encuesta.Descripcion : string.Empty;
                        lblActivoEncuesta.Text = arbol.InventarioArbolAcceso.First().Encuesta != null ? "Si" : "No";
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
        }
    }
}