using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniHelp.UserControls.Genericos
{
    public partial class UcCatalogos : UserControl
    {
        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        readonly ServiceOrganizacionClient _servicioOrganizacion = new ServiceOrganizacionClient();
        private List<string> _lstError = new List<string>();

        public string Titulo
        {
            get { return lblTitleCatalogo.Text; }
            set { lblTitleCatalogo.Text = value; }
        }

        public int IdCatalogo
        {
            get { return Convert.ToInt32(hfCatalogo.Value); }
            set { hfCatalogo.Value = value.ToString(); }
        }
        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue); }
            set
            {
                foreach (ListItem usuario in ddlTipoUsuarioCatalogo.Items)
                {
                    usuario.Selected = int.Parse(usuario.Value) == value;
                    if (usuario.Selected) break;
                }
            }
        }

        public int IdPais { get; set; }
        public int IdCampus { get; set; }
        public int IdTorre { get; set; }
        public int IdPiso { get; set; }
        public int IdZona { get; set; }
        public int IdSubZona { get; set; }
        public int IdSiteRack { get; set; }

        public int IdHolding { get; set; }
        public int IdCompania { get; set; }
        public int IdDireccion { get; set; }
        public int IdSubDireccion { get; set; }
        public int IdGerencia { get; set; }
        public int IdSubGerencia { get; set; }

        public int IdJefatura { get; set; }



        private List<string> AlertaCatalogos
        {
            set
            {
                panelAlertaCatalogo.Visible = value.Any();
                if (!panelAlertaCatalogo.Visible) return;
                rptErrorCatalogo.DataSource = value;
                rptErrorCatalogo.DataBind();
            }
        }

        public void Update()
        {
            upCatlogos.Update();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuarioCatalogo, lstTipoUsuario);
            }
        }
        protected void btnGuardarCatalogo_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Metodos.ValidaCapturaCatalogo(txtDescripcionCatalogo.Text))
                {
                    Ubicacion ubicacion = new Ubicacion
                    {
                        IdTipoUsuario = IdTipoUsuario,
                        IdPais = IdPais
                    };
                    Organizacion organizacion = new Organizacion
                    {
                        IdTipoUsuario = IdTipoUsuario,
                        IdHolding = IdHolding
                    };
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 3:
                            ubicacion.IdCampus = IdCampus;
                            ubicacion.Torre = new Torre
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            break;
                        case 4:
                            ubicacion.IdCampus = IdCampus;
                            ubicacion.IdTorre = IdTorre;
                            ubicacion.Piso = new Piso
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            break;
                        case 5:
                            ubicacion.IdCampus = IdCampus;
                            ubicacion.IdTorre = IdTorre;
                            ubicacion.IdPiso = IdPiso;
                            ubicacion.Zona = new Zona
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };

                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            break;
                        case 6:
                            ubicacion.IdCampus = IdCampus;
                            ubicacion.IdTorre = IdTorre;
                            ubicacion.IdPiso = IdPiso;
                            ubicacion.IdZona = IdZona;
                            ubicacion.SubZona = new SubZona
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };

                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            break;
                        case 7:
                            ubicacion.IdCampus = IdCampus;
                            ubicacion.IdTorre = IdTorre;
                            ubicacion.IdPiso = IdPiso;
                            ubicacion.IdZona = IdZona;
                            ubicacion.IdSubZona = IdSubZona;
                            ubicacion.SiteRack = new SiteRack
                            {
                                IdTipoUsuario = IdTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            break;
                        case 8:
                            organizacion.Holding = new Holding
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 9:
                            organizacion.IdHolding = IdHolding;
                            organizacion.Compania = new Compania
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 10:
                            organizacion.IdHolding = IdHolding;
                            organizacion.IdCompania = IdCompania;
                            organizacion.Direccion = new Direccion
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 11:
                            organizacion.IdHolding = IdHolding;
                            organizacion.IdCompania = IdCompania;
                            organizacion.IdDireccion = IdDireccion;
                            organizacion.SubDireccion = new SubDireccion
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 12:
                            organizacion.IdHolding = IdHolding;
                            organizacion.IdCompania = IdCompania;
                            organizacion.IdDireccion = IdDireccion;
                            organizacion.IdSubDireccion = IdSubDireccion;
                            organizacion.Gerencia = new Gerencia
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 13:
                            organizacion.IdHolding = IdHolding;
                            organizacion.IdCompania = IdCompania;
                            organizacion.IdDireccion = IdDireccion;
                            organizacion.IdSubDireccion = IdSubDireccion;
                            organizacion.IdGerencia = IdGerencia;
                            organizacion.SubGerencia = new SubGerencia
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                        case 14:
                            organizacion.IdHolding = IdHolding;
                            organizacion.IdCompania = IdCompania;
                            organizacion.IdDireccion = IdDireccion;
                            organizacion.IdSubDireccion = IdSubGerencia;
                            organizacion.IdGerencia = IdGerencia;
                            organizacion.IdSubGerencia = IdSubGerencia;
                            organizacion.Jefatura = new Jefatura
                            {
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCatalogo.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            break;
                    }
                    LimpiaCatalogo();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCatalogoUbicacion\");", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCatalogos = _lstError;
            }
        }

        protected void btnCancelarCatalogo_OnClick(object sender, EventArgs e)
        {
            LimpiaCatalogo();
        }
        private void LimpiaCatalogo()
        {
            try
            {
                txtDescripcionCatalogo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}