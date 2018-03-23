<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketAgente.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Tickets.UcTicketAgente" %>


<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <section class="module">
            <div class="row">
                <div class="col-lg-8 col-md-7 col-sm-7">
                    <div class="module-inner">
                        <asp:Label runat="server" Text="Correo electrónico, teléfono, nombre de usuario" CssClass="col-sm-4" />
                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control text-no-transform" />
                        <asp:Button runat="server" ID="btnBuscar" Text="Buscar" CssClass="btn btn-success col-sm-1" OnClick="btnBuscar_OnClick" />
                    </div>
                </div>
                <div class="form-horizontal">
                    <asp:RadioButtonList runat="server" ID="rbtnLstUsuarios" OnSelectedIndexChanged="rbtnLstUsuarios_OnSelectedIndexChanged" AutoPostBack="True" />
                </div>
                <div class="form-horizontal" runat="server" visible="False" id="divArbol">
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

                    <div class="row">
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
                    <div class="row">
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="col-lg-2">SubMenu/Opcion 7</asp:Label>
                        </div>
                        <div class="form-group">
                            <div class="col-lg-2">
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivel7" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnLevantar" Text="Levantar ticket" OnClick="btnLevantar_OnClick" Visible="False" />
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnNotificacion" Text="Notificar" OnClick="btnNotificacion_OnClick" Visible="False" />
                </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
