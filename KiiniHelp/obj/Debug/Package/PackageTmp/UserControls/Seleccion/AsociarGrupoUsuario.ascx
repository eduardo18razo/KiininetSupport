<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsociarGrupoUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.AsociarGrupoUsuario" %>

<%@ Register Src="~/UserControls/Altas/UcAltaGrupoUsuario.ascx" TagPrefix="uc" TagName="UcAltaGrupoUsuario" %>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel ID="upGrupos" runat="server">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfTipoUsuario" />
                <asp:HiddenField runat="server" ID="hfAsignacionAutomatica" />
                <header class="modal-header" id="panelAlertaGrupos" runat="server" visible="false">
                    <div class="alert alert-danger">
                        <div>
                            <div style="float: left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div style="float: left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix" />
                        </div>
                        <hr />
                        <asp:Repeater runat="server" ID="rptErrorGrupos">
                            <ItemTemplate>
                                <ul>
                                    <li><%# Container.DataItem %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <div class="well">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Asignacion de Grupos
                        </div>
                        <div class="panel-body">
                            <div class="panel">
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="col-sm-offset-1">
                                            <asp:CheckBox runat="server" ID="chkAsignarGruposSistema" AutoPostBack="True" OnCheckedChanged="chkAsignarGruposSistema_OnCheckedChanged" Text="Asignar grupos del sistema" Visible="False" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoAdministrador" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Administrador" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoAdministrador" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="1" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar nuevo" OnClick="OnClickAltaGrupo" CommandArgument="1" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoAcceso" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Acceso" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoAcceso" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="2" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="2" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoEspConsulta" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Especial de Consulta" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoEspecialConsulta" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="3" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="3" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoRespAtencion" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable de Atención" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoResponsableAtencion" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="4" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="4" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoRespMtto" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable de Información Publicada" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoResponsableMantenimiento" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="5" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="5" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoRespOperacion" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable de Operación" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoResponsableOperacion" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="6" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="6" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divGrupoRespDesarrollo" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable de Desarrollo" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlGrupoResponsableDesarrollo" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="7" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="7" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divDuenoServicio" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Dueño del Servicio" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlDuenoServicio" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="13" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="13" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divdivContactCenter" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Contact Center" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlContactCenter" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="14" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="14" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>


                                    <div class="form-group" runat="server" id="divUbicacionEmpleado" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable Mtto Ubicacion Empledo" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlUbicacionEmpleado" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="8" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="8" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>

                                    <div class="form-group" runat="server" id="divOrganizacionEmpleado" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable Mtto Organizacion Empleado" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlOrganizacionEmpleado" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="9" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="9" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>

                                    <div class="form-group" runat="server" id="divUsuarioEmpleado" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable Mtto Usuario Empleado" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlUsuarioEmpleado" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="10" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="10" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>

                                    <div class="form-group" runat="server" id="divUsuarioCliente" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable Mtto Usuario Cliente" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlUsuarioCliente" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="11" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="11" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>

                                    <div class="form-group" runat="server" id="divUsuarioProveedor" visible="False">
                                        <div class="col-sm-offset-1">
                                            <asp:Label runat="server" Text="Responsable Mtto Usuario Proveedor" class="col-sm-3 control-label"></asp:Label>
                                            <asp:DropDownList runat="server" Width="250px" ID="ddlUsuarioProveedor" CssClass="DropSelect" AutoPostBack="True" />
                                            <asp:Button runat="server" CssClass="btn btn-success btn-xs" Text="Asignar" OnClick="OnClickAsignarGrupo" CommandArgument="12" />
                                            <asp:Button runat="server" CssClass="btn btn-primary btn-xs" Text="Agregar" OnClick="OnClickAltaGrupo" CommandArgument="12" data-toggle="modal" data-target="#modalAltaGrupoUsuario" data-backdrop="static" data-keyboard="false" />
                                        </div>
                                    </div>

                                    <br />
                                    <br />
                                </div>

                            </div>
                            <br />
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Grupos asignados</h3>
                                </div>
                                <div class="panel-body">
                                    <asp:Repeater runat="server" ID="rptUsuarioGrupo">
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class="row form-control" style="margin-top: 5px">
                                                <asp:Label runat="server" ID="lblIdTipoSubGrupo" Text='<%# Eval("grupoUsuario.TipoGrupo.Id") %>' Visible="False" />
                                                <asp:Label runat="server" Text='<%# Eval("grupoUsuario.TipoGrupo.Descripcion") %>' Style="width: 50%" />
                                                > 
                                                <asp:Label runat="server" ID="lblIdGrupoUsuario" Text='<%# Eval("GrupoUsuario.Id") %>' Visible="False" />
                                                <asp:Label runat="server" Text='<%# Eval("GrupoUsuario.Descripcion") %>' Style="width: 50%" />
                                                >
                                                <asp:Label runat="server" ID="lblIdSubGrupo" Text='<%# Eval("SubGrupoUsuario.Id") %>' Visible="False" />
                                                <asp:Label runat="server" Text='<%# Eval("SubGrupoUsuario.SubRol.Descripcion") %>' Style="width: 50%" />
                                                <asp:Button runat="server" CssClass="btn btn-primary btn-sm" ID="btnEliminar" OnClick="btnEliminar_OnClick" Text="Quitar"/>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer" style="text-align: center">
                            <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-success" Text="Limpiar" OnClick="btnLimpiar_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

<%--MODAL GRUPO USUARIO--%>
<div class="modal fade" id="modalAltaGrupoUsuarios" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upAltaGrupo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <uc:UcAltaGrupoUsuario runat="server" id="ucAltaGrupoUsuario" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<%--MODAL SELECCION DE ROL--%>
<div class="modal fade" id="modalSeleccionRol" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upSubRoles" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-md">
                <div class="modal-content">
                    <div class="modal-header" id="panelAlertaSeleccionRol" runat="server" visible="false">
                        <div class="alert alert-danger">
                            <div>
                                <div style="float: left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div style="float: left">
                                    <h3>Error</h3>
                                </div>
                                <div class="clearfix clear-fix" />
                            </div>
                            <asp:Repeater runat="server" ID="rptErrorSeleccionRol">
                                <ItemTemplate>
                                    <div class="row">
                                        <ul>
                                            <li><%# Container.DataItem %></li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <asp:Label runat="server" ID="lblTitleSubRoles"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <asp:HiddenField runat="server" ID="hfOperacion" />
                            <div>
                                <div class="form-group">
                                    <div class="form-group">
                                        <asp:CheckBoxList runat="server" ID="chklbxSubRoles" Checked="True" Visible="True" OnSelectedIndexChanged="chklbxSubRoles_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button ID="btnAsignarSubRoles" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAsignarSubRoles_OnClick" />
                        <asp:Button ID="btnCancelarSubRoles" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelarSubRoles_OnClick" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
