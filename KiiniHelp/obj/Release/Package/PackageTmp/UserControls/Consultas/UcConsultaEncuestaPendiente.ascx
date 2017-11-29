<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaEncuestaPendiente.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaEncuestaPendiente" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <div class="modal-header" id="panelAlertaGeneral" runat="server" visible="false">
                <div class="alert alert-danger" role="alert">
                    <div>
                        <div style="float: left">
                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                        </div>
                        <div style="float: left">
                            <h3>Error</h3>
                        </div>
                        <div class="clearfix clear-fix"></div>
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
            </div>
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <asp:Label runat="server" ID="lbotest"></asp:Label>
                    <h3>Encuestas Pendientes</h3>
                </div>
                <div class="panel-body">
                    <asp:Repeater runat="server" ID="rptEncuestasPendientes">
                        <HeaderTemplate>
                            <div style="overflow-y: auto; min-height: 380px;">
                                <div class="panel-heading" style="padding-bottom: 9px; margin: 0 0 20px; padding-left: 0">
                                    <asp:Label CssClass="col-xs-1" runat="server" Text="Ticket" Width="8%" />
                                    <asp:Label CssClass="col-xs-2" runat="server" Text="Descripcion" Width="14%" />
                                    <asp:Label CssClass="col-xs-2" runat="server" Text="Encuesta" Style="width: 14%" />
                                    <asp:Label CssClass="col-xs-1" runat="server" Text="Respondida" Width="10.6%" />
                                    <div class="clearfix clear-fix"></div>
                                </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="panel-body row" style="padding-bottom: 9px; margin: 0 0 20px; padding-left: 0">
                                <%--ID--%>
                                <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdTicket") %>' Visible="False" />
                                <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("IdEncuesta") %>' Visible="False" />
                                <%--Pantalla--%>
                                <asp:Label runat="server" CssClass="col-xs-1" Text='<%#Eval("NumeroTicket") %>' ID="lbntIdticket" Width="8%" />
                                <asp:Label runat="server" CssClass="col-xs-2" Text='<%#Eval("Tipificacion").ToString().Length > 12 ? Eval("Tipificacion").ToString().Substring(0, 12) : Eval("Tipificacion")%>' Width="14%" />
                                <asp:Label runat="server" CssClass="col-xs-2" Text='<%#Eval("Descripcion").ToString().Length > 12 ? Eval("Descripcion").ToString().Substring(0, 12) : Eval("Descripcion")%>' Width="14%" />
                                <asp:Label runat="server" CssClass="col-xs-2" Style="width: 10.66666667%" Text='<%#(bool) Eval("Respondida") ? "SI" : "NO" %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
