<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaTicketUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaTicketUsuario" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
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
                                <ul>
                                    <li><%# Container.DataItem %></li> 
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3>Operacion de tickets</h3>
                    </div>
                    <div class="panel-body">
                        <div class="panel panel-primary">

                            <asp:Repeater runat="server" ID="rptTickets" >
                                <HeaderTemplate>
                                    <div style="overflow-y: auto; min-height: 380px;">
                                        <div class="panel-heading padding-9-bottom" style="margin: 0 0 20px; padding-left: 0">
                                            <asp:LinkButton CssClass="col-xs-1" runat="server" Text="Fecha y Hora" CommandName="DateTime" Width="10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Ticket" Width="8%" />
                                            <asp:Label CssClass="col-xs-2" runat="server" Text="Descripcion" Width="14%" />
                                            <asp:Label CssClass="col-xs-2" runat="server" Text="Grupo Asignado" Style="width: 10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Agente" Width="10.6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Nivel" Width="6%" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Estatus Ticket" />
                                            <asp:Label CssClass="col-xs-1" runat="server" Text="Estatus Asignacion" Width="12%" />
                                            <div class="clearfix clear-fix"></div>
                                        </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="panel-body row padding-9-bottom" style="margin: 0 0 20px; padding-left: 0">
                                        <%--ID--%>
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdTicket") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdUsuario") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdUsuarioAsignado") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusTicket.Id") %>' Visible="False" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusAsignacion.Id") %>' Visible="False" ID="lblEstatusAsignacionActual" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdGrupoAsignado") %>' Visible="False" ID="lblIdGrupoAsignado" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EsPropietario") %>' Visible="False" ID="lblEsPropietario" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("CambiaEstatus") %>' Visible="False" ID="lblCambiaEstatus" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("Asigna") %>' Visible="False" ID="lblAsigna" />
                                        <%--Pantalla--%>
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("FechaHora",  "{0:dd/MM/yy HH:mm}") %>' Width="10.6%" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("NumeroTicket") %>' ID="lbntIdticket" Width="8%" />
                                        <asp:Label runat="server" CssClass="col-xs-2" Text='<%#Eval("Tipificacion").ToString().Length > 12 ? Eval("Tipificacion").ToString().Substring(0, 12) : Eval("Tipificacion")%>' Width="14%" />
                                        <asp:Label runat="server" CssClass="col-xs-2" Style="width: 10.66666667%" Text='<%#Eval("GrupoAsignado") %>' />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("UsuarioAsignado").ToString(). Length > 12 ? Eval("UsuarioAsignado").ToString().Substring(0, 12) : Eval("UsuarioAsignado") %>' ID="btnUsuarioAsignado" Width="10.6%" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("NivelUsuarioAsignado").ToString().Length > 3 ? Eval("NivelUsuarioAsignado").ToString().Substring(0,3)  : Eval("NivelUsuarioAsignado") %>' Width="6%" />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusTicket.Descripcion") %>' />
                                        <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("EstatusAsignacion.Descripcion") %>' Width="12%" />
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
                                     OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</div>
