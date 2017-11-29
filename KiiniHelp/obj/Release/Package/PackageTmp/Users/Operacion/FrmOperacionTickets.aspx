<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmOperacionTickets.aspx.cs" Inherits="KiiniHelp.Users.Operacion.FrmOperacionTickets" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusAsignacion.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusAsignacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleTicket.ascx" TagPrefix="uc1" TagName="UcDetalleTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
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
                                <ul>
                                    <li><%# Container.DataItem %></li> 
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <asp:HiddenField runat="server" ID="hfTicketActivo"/>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3>Operacion de tickets</h3>
                    </div>
                    <div class="panel-body">
                        <div class="panel panel-primary">

                            <asp:Repeater runat="server" ID="rptTickets" OnItemDataBound="rptTickets_OnItemDataBound" OnItemCommand="rptTickets_OnItemCommand">
                                <HeaderTemplate>
                                    <div style="overflow-y: auto; min-height: 380px;">
                                        <div class="panel-heading" style="padding-bottom: 9px; margin: 0 0 20px; padding-left: 0">
                                            <asp:LinkButton CssClass="col-xs-1" runat="server" Text="Fecha y Hora" CommandName="DateTime" Width="10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Ticket" Width="8%" />
                                            <asp:Label CssClass="col-xs-2" runat="server" Text="Descripción" Width="14%" />
                                            <asp:Label CssClass="col-xs-2" runat="server" Text="Grupo Asignado" Style="width: 10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Agente" Width="10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Nivel" Width="6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Estatus Ticket" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Estatus Asignacion" Width="12%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Accion Estatus" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Accion Asignación" />
                                            <div class="clearfix clear-fix"></div>
                                        </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="panel-body row" style="padding-bottom: 9px; margin: 0 0 20px; padding-left: 0">
                                        <%--ID--%>
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdTicket") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdUsuario") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdUsuarioAsignado") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusTicket.Id") %>' Visible="False" ID="lblEstatusActual" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusAsignacion.Id") %>' Visible="False" ID="lblEstatusAsignacionActual" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdGrupoAsignado") %>' Visible="False" ID="lblIdGrupoAsignado" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EsPropietario") %>' Visible="False" ID="lblEsPropietario" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("CambiaEstatus") %>' Visible="False" ID="lblCambiaEstatus" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("Asigna") %>' Visible="False" ID="lblAsigna" />
                                        <%--Pantalla--%>
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("FechaHora",  "{0:dd/MM/yy HH:mm}") %>' Width="10.6%" />
                                        <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("NumeroTicket") %>' ID="lbntIdticket" OnClick="lbntIdticket_OnClick" CommandArgument='<%#Eval("IdTicket") %>' Width="8%" />
                                        <asp:Label runat="server" CssClass="col-xs-2" Text='<%#Eval("Tipificacion").ToString().Length > 12 ? Eval("Tipificacion").ToString().Substring(0, 12) : Eval("Tipificacion")%>' Width="14%" />
                                        <asp:Label runat="server" CssClass="col-xs-2" Style="width: 10.66666667%" Text='<%#Eval("GrupoAsignado") %>' />
                                        <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("UsuarioAsignado").ToString(). Length > 12 ? Eval("UsuarioAsignado").ToString().Substring(0, 12) : Eval("UsuarioAsignado") %>' ID="btnUsuarioAsignado" OnClick="btnUsuario_OnClick" CommandArgument='<%#Eval("IdUsuario") %>' Width="10.6%" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("NivelUsuarioAsignado").ToString().Length > 3 ? Eval("NivelUsuarioAsignado").ToString().Substring(0,3)  : Eval("NivelUsuarioAsignado") %>' Width="6%" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusTicket.Descripcion") %>' />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusAsignacion.Descripcion") %>' Width="12%" />
                                        <div class="col-xs-1"  visible='<%# (bool) Eval("CambiaEstatus")%>'>
                                            <asp:LinkButton runat="server" CssClass='<%# (bool) Eval("CambiaEstatus") ? "btn btn-sm btn-primary" : "btn btn-sm btn-primary disabled"%>' Text="Modificar" ID="btnCambiarEstatus" CommandArgument='<%#Eval("IdTicket") %>' OnClick="btnCambiarEstatus_OnClick" />
                                        </div>
                                        <div class="col-xs-1" runat="server" visible='<%# (bool) Eval("Asigna")%>'>
                                            <asp:LinkButton runat="server" CssClass='<%# (bool) Eval("Asigna") ? "btn btn-sm btn-primary" : "btn btn-sm btn-primary disabled" %>' Text="Modificar" ID="btnAsignar" CommandArgument='<%#Eval("IdTicket") %>' OnClick="btnAsignar_OnClick" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <asp:Repeater ID="rptPager" runat="server">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                                    CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                                    OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleTicket" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <uc1:UcDetalleTicket runat="server" ID="UcDetalleTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalAsignacionCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusAsignacion runat="server" ID="UcCambiarEstatusAsignacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" ID="UcCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


</asp:Content>
