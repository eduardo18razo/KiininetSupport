using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleGrupoUsuario : UserControl
    {
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

        public int IdUsuario
        {
            set
            {
                List<UsuarioGrupo> grupos = new ServiceGrupoUsuarioClient().ObtenerGruposDeUsuario(value);
                List<HelperAsignacionRol> lstRoles = grupos.Select(s => new { s.IdRol, s.Rol.Descripcion }).Distinct().Select(typeAnonymous => new HelperAsignacionRol
                {
                    IdRol = typeAnonymous.IdRol,
                    DescripcionRol = typeAnonymous.Descripcion,
                    Grupos = new List<HelperAsignacionGrupoUsuarios>()
                }).ToList();
                foreach (UsuarioGrupo usuarioGrupo in grupos)
                {

                    HelperAsignacionRol rolActivo = lstRoles.Single(s => s.IdRol == usuarioGrupo.IdRol);
                    HelperAsignacionGrupoUsuarios grupoToAdd;
                    if (rolActivo.Grupos.Any(s => s.IdGrupo == usuarioGrupo.IdGrupoUsuario))
                    {
                        grupoToAdd = rolActivo.Grupos[rolActivo.Grupos.FindIndex(s => s.IdGrupo == usuarioGrupo.IdGrupoUsuario)];
                    }
                    else
                        grupoToAdd = new HelperAsignacionGrupoUsuarios
                        {
                            IdGrupo = usuarioGrupo.IdGrupoUsuario,
                            DescripcionGrupo = usuarioGrupo.GrupoUsuario.Descripcion,
                            SubGrupos = usuarioGrupo.SubGrupoUsuario == null ? null :  new List<HelperSubGurpoUsuario> { new HelperSubGurpoUsuario { Id = (int)usuarioGrupo.IdSubGrupoUsuario } },
                        };
                    if (usuarioGrupo.IdSubGrupoUsuario != null)
                    {
                        if (!grupoToAdd.SubGrupos.Any(a => a.Id == usuarioGrupo.IdSubGrupoUsuario))
                        {
                            if (grupoToAdd.SubGrupos == null)
                                grupoToAdd.SubGrupos = new List<HelperSubGurpoUsuario>();
                            grupoToAdd.SubGrupos = new List<HelperSubGurpoUsuario>();
                            HelperSubGurpoUsuario subGpoToAdd = new HelperSubGurpoUsuario
                            {
                                Id = (int)usuarioGrupo.IdSubGrupoUsuario,
                                Descripcion = usuarioGrupo.SubGrupoUsuario.SubRol.Descripcion
                            };
                            grupoToAdd.SubGrupos.Add(subGpoToAdd);
                        }
                    }
                    rolActivo.Grupos.Add(grupoToAdd);



                }
                MostrarGruposSeleccionados(lstRoles);
            }
        }
        private void MostrarGruposSeleccionados(List<HelperAsignacionRol> gruposAsignados)
        {
            try
            {
                rptRoles.DataSource = gruposAsignados;
                rptRoles.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
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
        protected void rptRoles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                Repeater rpt = (Repeater)e.Item.FindControl("rptGrupos");
                rpt.DataSource = ((HelperAsignacionRol)e.Item.DataItem).Grupos;
                rpt.DataBind();
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
        protected void rptGrupos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                Repeater rpt = (Repeater)e.Item.FindControl("rptSubGrupos");
                rpt.DataSource = ((HelperAsignacionGrupoUsuarios)e.Item.DataItem).SubGrupos;
                rpt.DataBind();
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