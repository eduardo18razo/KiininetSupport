<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmLevantaTicketAgente.aspx.cs" Inherits="KiiniHelp.Users.Operacion.FrmLevantaTicketAgente" %>

<%@ Register Src="~/UserControls/Detalles/UcMensajeValidacion.ascx" TagPrefix="uc1" TagName="UcMensajeValidacion" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPreticket.ascx" TagPrefix="uc1" TagName="UcAltaPreticket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                / Levantar Ticket</h3>
            <hr />
            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <div class="module-inner">
                            <div class="row">
                                <asp:Label runat="server" Text="Correo electrónico, teléfono, nombre de usuario" CssClass="col-lg-2" />
                                <div class="col-lg-2">
                                    <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" Style="text-transform: none" />
                                </div>
                                <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-primary col-lg-1" OnClick="btnBuscar_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <div class="module-inner">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-2">
                                        <asp:RadioButtonList runat="server" ID="rbtnLstUsuarios" CssClass="col-lg-12" OnSelectedIndexChanged="rbtnLstUsuarios_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" runat="server" visible="False" id="divArbol">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <div class="module-inner">
                            <div class="row">

                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-lg-4">Medio de comunicación</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-4">Area</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-4">Tipo de Servicio</asp:Label>

                                </div>
                                <div class="form-group">
                                    <div class="col-lg-4">
                                        <asp:DropDownList runat="server" ID="ddlCanal" CssClass="form-control" />
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true" />
                                    </div>
                                    <div class="col-lg-4">
                                        <asp:DropDownList runat="server" ID="ddlTipoArbol" CssClass="form-control" OnSelectedIndexChanged="ddlTipoArbol_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                </div>

                            </div>
                            <div class="row margin-top">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 1</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 2</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 3</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 4</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 5</asp:Label>
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 6</asp:Label>
                                </div>
                                <div class="form-group">
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel1" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel2" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel3" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel4" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel5" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel6" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                            <div class="row margin-top">
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 7</asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-2">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel7" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <div class="module-inner">
                            <div class="row">
                                <div class="form-group">
                                    <div class="col-lg-2">
                                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnLevantar" Text="Levantar ticket" OnClick="btnLevantar_OnClick" Visible="False" />
                                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnNotificacion" Text="Notificar" OnClick="btnNotificacion_OnClick" Visible="False" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>


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
