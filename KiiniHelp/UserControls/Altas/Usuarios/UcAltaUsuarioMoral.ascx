<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioMoral.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioMoral" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaUbicaciones.ascx" TagPrefix="uc" TagName="UcConsultaUbicaciones" %>
<%@ Register Src="~/UserControls/Seleccion/AsociarGrupoUsuario.ascx" TagPrefix="uc" TagName="AsociarGrupoUsuario" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc" TagName="UcAltaPuesto" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaOrganizacion.ascx" TagPrefix="uc" TagName="UcConsultaOrganizacion" %>
<asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="" id="panelAlertaGeneral" runat="server" visible="False">
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
                        <asp:Repeater runat="server" ID="rptErrorGeneral">
                            <ItemTemplate>
                                <%# Eval("Detalle")  %>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <asp:HiddenField runat="server" ID="hfIdUsuario" />
                <asp:HiddenField runat="server" ID="hfAlta" />
                <asp:HiddenField runat="server" ID="hfEsMoral" />

                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4>Agregar de Usuario Moral</h4>
                    </div>
                    <div class="panel-body">
                        <div class="well" runat="server" Visible="False">
                            <div class="form-inline center-content-div">
                                <div class="form-group" style="width: 42%">
                                    <label class="col-md-4">Tipo Usuario</label>
                                    <div class="col-md-8">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="well center-content-div" runat="server" id="divDatos" visible="False">
                            <asp:Button type="button" class="btn btn-primary " Text="Datos Generales" ID="btnModalDatosGenerales" data-toggle="modal" data-target="#modalDatosGenerales" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                            <asp:Button type="button" class="btn btn-primary disabled" Text="Organización" ID="btnModalOrganizacion" data-toggle="modal" data-target="#modalOrganizacion" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                            <asp:Button type="button" class="btn btn-primary disabled" Text="Ubicación" ID="btnModalUbicacion" data-toggle="modal" data-target="#modalUbicacion" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                            <asp:Button type="button" class="btn btn-primary disabled" Text="Roles" ID="btnModalRoles" data-toggle="modal" data-target="#modalRoles" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                            <asp:Button type="button" class="btn btn-primary disabled" Text="Grupos" ID="btnModalGrupos" data-toggle="modal" data-target="#modalGrupos" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                        </div>
                    </div>
                    <div class="panel-footer" style="text-align: center">
                        <asp:Button CssClass="btn btn-success" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_OnClick" />
                        <asp:Button CssClass="btn btn-danger" ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_OnClick" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <%--DATOS GENERALES--%>
        <div class="modal fade" id="modalDatosGenerales" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" style="overflow: auto">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <asp:UpdatePanel ID="upDatosGenerales" runat="server">
                        <ContentTemplate>
                            <div class="modal-header" id="panelAlertaModalDg" runat="server" visible="False">
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
                                    <asp:Repeater runat="server" ID="rptErrorDg">
                                        <ItemTemplate>
                                            <%# Container.DataItem %>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Datos generales
                                </div>
                                <div class="panel-body">
                                    <%--DATOS GENERALES--%>
                                    <div class="panel-body">
                                        <div class="form-inline">
                                            <div class="form-group col-sm-12">
                                                <asp:Label ID="Label4" runat="server" Text="Apellido Paterno" class="col-sm-4 control-label izquierda" />
                                                <asp:Label ID="Label5" runat="server" Text="Apellido Materno" class="col-sm-4 control-label izquierda" />
                                                <asp:Label ID="Label6" runat="server" Text="Nombre" class="col-sm-4 control-label izquierda" />
                                            </div>
                                            <div class="form-group col-sm-12">
                                                <div class="col-sm-4" style="padding-left: 0">
                                                    <asp:TextBox ID="txtAp" runat="server" CssClass="form-control obligatorio" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                                </div>
                                                <div class="col-sm-4" style="padding-left: 0">
                                                    <asp:TextBox ID="txtAm" runat="server" CssClass="form-control obligatorio" onkeypress="return ValidaCampo(this,1)" MaxLength="32" />
                                                </div>
                                                <div class="col-sm-4" style="padding-left: 0">
                                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control obligatorio" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="32" OnTextChanged="txtAp_OnTextChanged" />
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-12">
                                                <asp:Label runat="server" Text="usuario" class="col-sm-4 control-label izquierda" />
                                            </div>
                                            <div class="form-group col-sm-12">
                                                <div class="col-sm-4" style="padding-left: 0">
                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control texto-normal obligatorio" onkeypress="return ValidaCampo(this,14)" OnTextChanged="txtAp_OnTextChanged" MaxLength="12" Style=" text-transform: none" AutoPostBack="True" />
                                                </div>
                                            </div>
                                            <div class="form-group col-sm-12">
                                                <asp:Label ID="Label2" runat="server" Text="Puesto" class="col-sm-4 control-label izquierda" />
                                            </div>
                                            <div class="form-group col-sm-12" runat="server" ID="divPuesto">
                                                <div class="col-sm-4" style="padding-left: 0">
                                                    <asp:DropDownList runat="server" ID="ddlPuesto" CssClass="DropSelect" />
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Agregar" ID="btnAddPuesto" OnClick="btnAddPuesto_OnClick" />
                                                </div>
                                            </div>

                                            <div class="form-group col-sm-12 margen-arriba">
                                                <div class="col-sm-3" style="padding-left: 0">
                                                    <asp:CheckBox runat="server" Text="VIP" ID="chkVip" />
                                                </div>
                                                <div class="col-sm-3" style="padding-left: 0">
                                                    <asp:CheckBox runat="server" Text="Directorio Activo " ID="chkDirectoriActivo" />
                                                </div>
                                                <div class="form-group margen-arriba">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--TELEFONOS--%>
                                    <div class="well">
                                        <div class="panel panel-primary">
                                            <div class="panel-heading">
                                                <div class="row">
                                                    <div class="col-xs-6 col-sm-3">Telefonos</div>
                                                    <div class="col-xs-6 col-sm-3">
                                                        Numero telefono
                                                    </div>
                                                    <div class="col-xs-6 col-sm-3">
                                                        Extensiones
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="panel-body">
                                                <asp:Repeater ID="rptTelefonos" runat="server">
                                                    <ItemTemplate>
                                                        <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                            <div class="row">
                                                                <div class="col-xs-6 col-md-3" style="display: none">
                                                                    <asp:Label runat="server" ID="lblTipotelefono" Text='<%# Eval("TipoTelefono.Id") %>'></asp:Label>
                                                                </div>
                                                                <div class="col-xs-5 col-md-3">
                                                                    <asp:Label runat="server"><%# Eval("TipoTelefono.Descripcion") %></asp:Label>
                                                                </div>
                                                                <div class="col-xs-5 col-md-3">
                                                                    <asp:TextBox runat="server" ID="txtNumero" Text='<%# Eval("Numero") %>' CssClass=<%# bool.Parse(Eval("Obligatorio").ToString()) ? "form-control obligatorio"  : "form-control"  %> onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                                </div>
                                                                <div class="col-xs-4 col-md-3" runat="server" visible='<%# Eval("TipoTelefono.Extension") %>'>
                                                                    <asp:TextBox runat="server" ID="txtExtension" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,15)" MaxLength="40" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                    <%--CORREOS--%>
                                    <div class="well">
                                        <div class="panel panel-primary">
                                            <div class="panel-heading">
                                                Correos
                                            </div>
                                            <div class="panel-body">
                                                <asp:Repeater ID="rptCorreos" runat="server">
                                                    <ItemTemplate>
                                                        <div style="border-radius: 20px; margin-bottom: 5px; height: auto">
                                                            <div class="row">
                                                                <div class="col-xs-8 col-md-6">
                                                                    <asp:TextBox runat="server" ID="txtCorreo" Text='<%# Eval("Correo") %>' CssClass=<%# bool.Parse(Eval("Obligatorio").ToString()) ? "form-control obligatorio"  : "form-control"  %> Style="text-transform: lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel-footer" style="text-align: center">
                                    <asp:Button ID="btnAceptarDatosGenerales" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptarDatosGenerales_OnClick" />
                                    <asp:Button ID="btnCerrarDatosGenerales" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCerrarDatosGenerales_OnClick" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--ORGANIZACION--%>
        <div class="modal fade" id="modalOrganizacion" data-backdrop-limit="1" tabindex="-1" role="dialog" aria-labelledby="upload-avatar-title" aria-hidden="true" style="overflow: hidden">
            <div class="modal-dialog modal-lg" style="width: 100%">
                <div class="modal-content">
                    <asp:UpdatePanel ID="upOrganizacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc:UcConsultaOrganizacion runat="server" ID="UcConsultaOrganizacion" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--UBICACIONES--%>
        <div class="modal fade" id="modalUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden">
            <div class="modal-dialog modal-lg" style="width: 100%">
                <div class="modal-content">
                    <asp:UpdatePanel ID="upUbicacion" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc:UcConsultaUbicaciones runat="server" ID="UcConsultaUbicaciones" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--ROLES--%>
        <div class="modal fade" id="modalRoles" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: hidden">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <asp:UpdatePanel ID="upRoles" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-header" id="panelAlertaRoles" runat="server" visible="false">
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
                                    <asp:Repeater runat="server" ID="rptErrorRoles">
                                        <ItemTemplate>
                                            <ul>
                                                <li><%# Container.DataItem %></li>
                                            </ul>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    Asignacion de Roles                                
                                </div>
                                <div class="panel-body">
                                    <asp:CheckBoxList runat="server" ID="chklbxRoles" OnSelectedIndexChanged="chkKbxRoles_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                                <div class="panel-footer" style="text-align: center">
                                    <asp:Button ID="btnAceptarRoles" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptarRoles_OnClick" />
                                    <asp:Button ID="btnCerrarRoles" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCerrarRoles_OnClick" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--GRUPOS--%>
        <div class="modal fade" id="modalGrupos" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="overflow: auto">
            <asp:UpdatePanel ID="upGrupos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <uc:AsociarGrupoUsuario runat="server" ID="AsociarGrupoUsuario" />
                            <div class="panel-footer" style="text-align: center">
                                <asp:Button runat="server" CssClass="btn btn-success" ID="btnAceptarGrupos" Text="Aceptar" OnClick="btnAceptarGrupos_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrarGrupos" Text="Cerrar" OnClick="btnCerrarGrupos_OnClick" />
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <%--AREA--%>
        <div class="modal fade" id="modalAreas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
            <asp:UpdatePanel ID="upModalAltaAreas" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog modal-md">
                        <div class="modal-content">
                            <uc:UcAltaPuesto runat="server" ID="ucAltaPuesto" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
