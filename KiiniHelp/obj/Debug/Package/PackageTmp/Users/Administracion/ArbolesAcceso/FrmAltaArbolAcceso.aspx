<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmAltaArbolAcceso.aspx.cs" Inherits="KiiniHelp.Users.Administracion.ArbolesAcceso.FrmAltaArbolAcceso" %>

<%@ Register Src="~/UserControls/Altas/AltaArea.ascx" TagPrefix="uc" TagName="AltaArea" %>
<%@ Register Src="~/UserControls/Altas/UcAltaNivelArbol.ascx" TagPrefix="uc" TagName="UcAltaNivelArbol" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upGeneral" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <header>
                        <div class="alert alert-danger" id="panelAlert" runat="server" visible="False">
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
                            <asp:Repeater runat="server" ID="rptHeaderError">
                                <ItemTemplate>
                                    <%# Container.DataItem %>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </header>
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Opciones de menú
                        </div>
                        <div class="panel-body">
                            <div class="well">
                                <div class="form-group">
                                    <div class="col-sm-offset-1">
                                        <asp:Label runat="server" for="ddlTipoUsuario" class="col-sm-3 control-label">Tipo de Usuario</asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                                        <%----%>
                                    </div>
                                </div>

                            </div>
                            <%--ARBOL DE ACCESO--%>
                            <div class="well center-block">
                                <asp:UpdatePanel ID="upArbolAcceso" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="panel panel-primary">
                                            <div class="panel-heading">
                                                Datos generales
                                            </div>
                                            <div class="panel-body">
                                                <asp:UpdatePanel ID="upSeleccionArea" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <div class="panel" style="background: transparent; border: 0; -webkit-box-shadow: none; -ms-box-shadow: none; box-shadow: none">
                                                            
                                                                <div class="form-group">
                                                                    <div class="col-sm-offset-1">
                                                                        <asp:Label runat="server" for="ddlNivel6" class="col-sm-3 control-label" Text="Area de Atención" />
                                                                        <asp:DropDownList runat="server" ID="ddlArea" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                                        <asp:Button runat="server" ID="btnAddArea" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAddArea_OnClick" Enabled="False"/>
                                                                    </div>
                                                                </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlTipoArbol" class="col-sm-3 control-label">Tipo de Servicio</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlTipoArbol" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlTipoArbol_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel1" class="col-sm-3 control-label">SubMenu/Opcion 1</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel1" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel1" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel2" class="col-sm-3 control-label">SubMenu/Opcion 2</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel2" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel2" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel3" class="col-sm-3 control-label">SubMenu/Opcion 3</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel3" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel3" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel4" class="col-sm-3 control-label">SubMenu/Opcion 4</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel4" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel4" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel5" class="col-sm-3 control-label">SubMenu/Opcion 5</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel5" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel5" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel6" class="col-sm-3 control-label">SubMenu/Opcion 6</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel6" Width="450px" CssClass="DropSelect" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel6" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="6" Enabled="False" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-offset-1">
                                                        <asp:Label runat="server" for="ddlNivel7" class="col-sm-3 control-label">SubMenu/Opcion 7</asp:Label>
                                                        <asp:DropDownList runat="server" ID="ddlNivel7" Width="450px" CssClass="DropSelect" AutoPostBack="True" AppendDataBoundItems="True" />
                                                        <asp:Button runat="server" ID="btnAgregarNivel7" Text="Agregar" CssClass="btn btn-primary btn-xs" OnClick="btnAgregarNivel_OnClick" CommandArgument="7" Enabled="False" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <div class="clearfix clear-fix"></div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--AREA--%>
            <div class="modal fade" id="modalAreas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <div class="modal-dialog ">
                            <div class="modal-content">
                                <uc:AltaArea runat="server" ID="AltaAreas" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%--MODAL CATALOGO NIVELES--%>
            <div class="modal fade" id="editNivel" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
                <asp:UpdatePanel ID="upCampus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-dialog modal-lg" style="width: 1100px; height: 750px; overflow-y: auto">
                            <div class="modal-content">
                                <uc:UcAltaNivelArbol runat="server" id="ucAltaNivelArbol" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
