<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmUsuarios.aspx.cs" Inherits="KiiniHelp.Users.Administracion.Usuarios.FrmUsuarios" %>

<%@ Register Src="~/UserControls/Seleccion/AsociarGrupoUsuario.ascx" TagPrefix="uc" TagName="AsociarGrupoUsuario" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaOrganizacion.ascx" TagPrefix="uc" TagName="UcConsultaOrganizacion" %>
<%@ Register Src="~/UserControls/Consultas/UcConsultaUbicaciones.ascx" TagPrefix="uc" TagName="UcConsultaUbicaciones" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc" TagName="UcAltaPuesto" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Agregar Usuarios</title>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <header class="" id="panelAlertaGeneral" runat="server" visible="False">
                        <div class="alert alert-danger">
                            <div>
                                <div class="float-left">
                                    <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                </div>
                                <div class="float-left">
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
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h4>Agregar de Usuarios</h4>
                        </div>
                        <div class="panel-body">
                            <div class="well">
                                <div class="form-inline verical-center center-content-div">
                                    <asp:Label ID="Label1" runat="server" Text="Tipo Usuario" class="col-sm-2 col-sm-offset-3 control-label"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="clearfix clear-fix"></div>
                            </div>
                            <div class="well center-content-div" runat="server" id="divDatos" visible="False">
                                <asp:Button type="button" class="btn btn-primary btn-lg " Text="Datos Generales" ID="btnModalDatosGenerales" data-toggle="modal" data-target="#modalDatosGenerales" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                                <asp:Button type="button" class="btn btn-primary btn-lg disabled" Text="Organización" ID="btnModalOrganizacion" data-toggle="modal" data-target="#modalOrganizacion" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                                <asp:Button type="button" class="btn btn-primary btn-lg disabled" Text="Ubicación" ID="btnModalUbicacion" data-toggle="modal" data-target="#modalUbicacion" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                                <asp:Button type="button" class="btn btn-primary btn-lg disabled" Text="Roles" ID="btnModalRoles" data-toggle="modal" data-target="#modalRoles" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                                <asp:Button type="button" class="btn btn-primary btn-lg disabled" Text="Grupos" ID="btnModalGrupos" data-toggle="modal" data-target="#modalGrupos" data-backdrop="static" data-keyboard="false" runat="server"></asp:Button>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <asp:Button CssClass="btn btn-lg btn-danger float-right margin-left-25" ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_OnClick"></asp:Button>
                            <asp:Button CssClass="btn btn-lg btn-success float-right" ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_OnClick"></asp:Button>

                            <div class="clearfix clear-fix"></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--DATOS GENERALES--%>
            <div class="modal fade" id="modalDatosGenerales" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="upDatosGenerales" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-header" id="panelAlertaModalDg" runat="server" visible="False">
                                    <div class="alert alert-danger">
                                        <div>
                                            <div class="float-left">
                                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                            </div>
                                            <div class="float-left">
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
                                                    <div class="col-sm-4 no-padding-left">
                                                        <asp:TextBox ID="txtAp" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true"  MaxLength="100" OnTextChanged="txtAp_OnTextChanged" />
                                                    </div>
                                                    <div class="col-sm-4 no-padding-left">
                                                        <asp:TextBox ID="txtAm" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" MaxLength="100" />
                                                    </div>
                                                    <div class="col-sm-4 no-padding-left">
                                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" onkeypress="return ValidaCampo(this,1)" AutoPostBack="true" MaxLength="100"  OnTextChanged="txtAp_OnTextChanged" />
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <asp:Label runat="server" Text="usuario" class="col-sm-4 control-label izquierda" />
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <div class="col-sm-4 no-padding-left">
                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control texto-normal text-no-transform" onkeypress="return ValidaCampo(this,14)" MaxLength="100"/>
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <asp:Label ID="Label2" runat="server" Text="Puesto" class="col-sm-4 control-label izquierda" />
                                                </div>
                                                <div class="form-group col-sm-12">
                                                    <div class="col-sm-4 no-padding-left">
                                                        <asp:DropDownList runat="server" ID="ddlPuesto" CssClass="DropSelect" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Agregar" ID="btnAddPuesto" OnClick="btnAddPuesto_OnClick" />
                                                    </div>
                                                </div>
                                                <div class="form-group col-sm-12 margen-arriba">
                                                    <div class="col-sm-3 no-padding-left">
                                                        <asp:CheckBox runat="server" Text="VIP" ID="chkVip" />
                                                    </div>
                                                    <div class="col-sm-3 no-padding-left">
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
                                                                    <div class="col-xs-6 col-md-3 ocultar">
                                                                        <asp:Label runat="server" ID="lblTipotelefono" Text='<%# Eval("TipoTelefono.Id") %>'></asp:Label>
                                                                    </div>
                                                                    <div class="col-xs-5 col-md-3">
                                                                        <asp:Label runat="server"><%# Eval("TipoTelefono.Descripcion") %></asp:Label>
                                                                    </div>
                                                                    <div class="col-xs-5 col-md-3">
                                                                        <asp:TextBox runat="server" ID="txtNumero" Text='<%# Eval("Numero") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                                                                    </div>
                                                                    <div class="col-xs-4 col-md-3" runat="server" visible='<%# Eval("TipoTelefono.Extension") %>'>
                                                                        <asp:TextBox runat="server" ID="txtExtension" Text='<%# Eval("Extension") %>' CssClass="form-control" onkeypress="return ValidaCampo(this,2)" MaxLength="40" />
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
                                                                        <asp:TextBox runat="server" ID="txtCorreo" Text='<%# Eval("Correo") %>' CssClass="form-control text-lowercase" onkeypress="return ValidaCampo(this,13)" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel-footer text-center">
                                        <asp:Button ID="btnAceptarDatosGenerales" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnAceptarDatosGenerales_OnClick" />
                                        <asp:Button ID="btnCerrarDatosGenerales" runat="server" CssClass="btn btn-danger" Text="Cancelar" data-dismiss="modal" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <%--ORGANIZACION--%>
            <div class="modal fade" id="modalOrganizacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <div class="modal-dialog modal-lg widht75Perc">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="upOrganizacion" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <%--<uc:AltaOrganizacion runat="server" ID="ucOrganizacion" FromModal="True" />--%>
                                <uc:UcConsultaOrganizacion runat="server" ID="UcConsultaOrganizacion" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <%--UBICACIONES--%>
            <div class="modal fade" id="modalUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <div class="modal-dialog modal-lg widht75Perc">
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
            <div class="modal fade" id="modalRoles" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="upRoles" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-header" id="panelAlertaRoles" runat="server" visible="false">
                                    <div class="alert alert-danger">
                                        <div>
                                            <div class="float-left">
                                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                                            </div>
                                            <div class="float-left">
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
                                    <div class="panel-footer text-center">
                                        <asp:Button ID="btnAceptarRoles" runat="server" CssClass="btn btn-success" Text="Aceptar" OnClick="btnCerrarRoles_OnClick" />
                                        <asp:Button ID="btnCerrarRoles" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCerrarRoles_OnClick" data-dismiss="modal" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <%--GRUPOS--%>
            <div class="modal fade" id="modalGrupos" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <asp:UpdatePanel ID="upGrupos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-dialog modal-lg">
                            <div class="modal-content">
                                <uc:AsociarGrupoUsuario runat="server" ID="AsociarGrupoUsuario" />
                                <div class="modal-footer">
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
                                <uc:UcAltaPuesto runat="server" id="UcAltaPuesto" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
