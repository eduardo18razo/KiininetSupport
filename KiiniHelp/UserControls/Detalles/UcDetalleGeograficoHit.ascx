<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleGeograficoHit.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleGeograficoHit" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTicket" />
        <asp:HiddenField runat="server" ID="hfConsulta" />
        <asp:HiddenField runat="server" ID="hfEncuesta" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="modal-header" id="panelAlerta" runat="server" visible="false">
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
                        <asp:Repeater runat="server" ID="rptError">
                            <ItemTemplate>
                                <ul>
                                    <li><%# Container.DataItem %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="panel panel-primary">
                            <div class="modal-header">
                                <h3 class="modal-title">Detalle de zona</h3>
                                <hr class="bordercolor">
                            </div>
                            <%--<div class="panel-heading">
                                Detalle de zona
                            </div>--%>
                            <div class="panel-body">
                                <asp:RadioButton runat="server" Text="Ubicaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnUbicaciones" OnCheckedChanged="rbtUbicaciones_OnCheckedChanged" />
                                <div class="panel panel-default" id="pnlUbicacion" runat="server" visible="False">
                                    <div class="panel-body">
                                        <asp:Repeater runat="server" ID="rptUbicaciones">
                                            <ItemTemplate>
                                                <div class="container-fluid" style="margin-top: 2px">
                                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                                    <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" AutoPostBack="True" />
                                                    (<asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>)
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <br />
                                <asp:RadioButton runat="server" Text="Organizaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnOrganizaciones" OnCheckedChanged="rbtnOrganizaciones_OnCheckedChanged" />
                                <div class="panel panel-default" id="pnlOrganizacion" runat="server" visible="False">
                                    <div class="panel-body">
                                        <asp:Repeater runat="server" ID="rptOrganizaciones">
                                            <ItemTemplate>
                                                <div class="container-fluid" style="margin-top: 2px">
                                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                                    <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" AutoPostBack="True" />
                                                    (<asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>)
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                                <br />
                                <asp:RadioButton runat="server" Text="Tipificaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnTipificaciones" OnCheckedChanged="rbtnTipificaciones_OnCheckedChanged" />
                                <div class="panel panel-default" runat="server" id="pnlTipificaciones" visible="False">
                                    <div class="panel-body">
                                        <asp:Repeater runat="server" ID="rptTipicaciones">
                                            <ItemTemplate>
                                                <div class="container-fluid" style="margin-top: 2px">
                                                    <asp:Label CssClass="col-sm-1" runat="server" Visible="False" ID="lblId" Text='<%# Eval("Id") %>' />
                                                    <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' ID="lblDescripcion" AutoPostBack="True" />
                                                    (<asp:Label runat="server" Text='<%# Eval("Total") %>'></asp:Label>)
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>

                            </div>
                            <div class="text-center"> <%--panel-footer--%>
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="Cerrar" OnClick="btnCerrar_OnClick" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
