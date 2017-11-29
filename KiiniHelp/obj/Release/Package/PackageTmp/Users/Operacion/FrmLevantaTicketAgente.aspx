<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmLevantaTicketAgente.aspx.cs" Inherits="KiiniHelp.Users.Operacion.FrmLevantaTicketAgente" %>

<%@ Register Src="~/UserControls/Detalles/UcMensajeValidacion.ascx" TagPrefix="uc1" TagName="UcMensajeValidacion" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPreticket.ascx" TagPrefix="uc1" TagName="UcAltaPreticket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3>Levantar Ticket Por Usuario</h3>
                </div>
                <div class="panel-body">
                    <div class="panel-primary">
                        <div class="panel-heading">
                            <h3>Buscar Usuario</h3>
                        </div>
                        <div class="panel-body">
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Correo electrónico, teléfono, nombre de usuario" CssClass="col-sm-4" />
                                        <div class="col-sm-5">
                                            <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" Style="text-transform: none" />
                                        </div>
                                        <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-success col-sm-1" OnClick="btnBuscar_OnClick" />
                                    </div>
                                </div>
                                <div class="form-horizontal">
                                    <asp:RadioButtonList runat="server" ID="rbtnLstUsuarios" OnSelectedIndexChanged="rbtnLstUsuarios_OnSelectedIndexChanged" AutoPostBack="True" />
                                </div>
                                <div class="form-horizontal" runat="server" visible="False" id="divArbol">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <asp:Label Width="16%" runat="server" CssClass="col-sm-3 control-label">Medio de comunicación</asp:Label>
                                            <asp:Label Width="16%" runat="server" CssClass="col-sm-3 control-label">Area</asp:Label>
                                            <asp:Label Width="16%" runat="server" CssClass="col-sm-3 control-label">Tipo de Servicio</asp:Label>
                                        </div>

                                        <div class="form-group">
                                            <asp:DropDownList runat="server" Width="16%" ID="ddlCanal" CssClass="DropSelect" AutoPostBack="true" />
                                            <asp:DropDownList runat="server" Width="16%" ID="ddlArea" CssClass="DropSelect" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true" />
                                            <asp:DropDownList runat="server" Width="16%" CssClass="DropSelect" ID="ddlTipoArbol" OnSelectedIndexChanged="ddlTipoArbol_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                        </div>
                                        <div class="form-group">

                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 1</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 2</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 3</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 4</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 5</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 6</asp:Label>
                                            <asp:Label Width="14%" runat="server" CssClass="col-sm-3 control-label">SubMenu/Opcion 7</asp:Label>
                                        </div>
                                        <div class="form-group">

                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel1" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel2" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel3" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel4" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel5" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel6" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                            <asp:DropDownList runat="server" Width="14%" CssClass="DropSelect" ID="ddlNivel7" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer">
                                <asp:Button runat="server" CssClass="btn btn-success" ID="btnLevantar" Text="Levantar ticket" OnClick="btnLevantar_OnClick" Visible="False" />
                                <asp:Button runat="server" CssClass="btn btn-success" ID="btnNotificacion" Text="Notificar" OnClick="btnNotificacion_OnClick" Visible="False" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    
    <div class="modal fade" id="modalPreTicket" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <uc1:UcAltaPreticket runat="server" id="ucAltaPreticket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalValidaUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <uc1:UcMensajeValidacion runat="server" id="ucMensajeValidacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    

    <%--<div class="modal fade" id="modalPreTicket" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upUser" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="width: 1250px; height: 940px; overflow: hidden">
                    <div class="modal-content">
                        jiajidajoijdsoijsdaoi
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc1:UcAltaPreticket runat="server" id="ucAltaPreticket" />--%>


        <%--<div class="modal fade" id="modalPreTicket" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
            <asp:UpdatePanel ID="upConfirmacion" runat="server">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h1>Generación de Ticket</h1>
                                </div>
                                <div class="panel panel-body">



                                    
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Se ha generado correctamente el ticket No.:" />
                                        <asp:TextBox runat="server" ID="lblNoTicket" CssClass="form-control" ReadOnly="True" />
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Con clave:" ID="lblDescRandom" />
                                        <asp:TextBox runat="server" ID="lblRandom" CssClass="form-control" ReadOnly="True" />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button runat="server" CssClass="btn btn-danger" ID="btnCerrar" Text="Cerrar" />
                                </div>

                            </div>

                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>--%>
    <%--</div>--%>
</asp:Content>
