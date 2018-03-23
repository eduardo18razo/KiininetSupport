<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosParametrosGraficoEficienciaTickets.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Graficos.UcFiltrosParametrosGraficoEficienciaTickets" %>
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
                            <div class="float-left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div class="float-left">
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
                            <div class="panel-heading">
                                Filtros
                            </div>
                            <div class="panel-body">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        Tipo de Grafico
                                    </div>
                                    <div class="panel-body">
                                        <div class="float-left">
                                            <asp:RadioButton runat="server" GroupName="gpoTipoGrafico" AutoPostBack="True" ID="rbtnGeografico" Text="Geografico" /><br />
                                            <asp:RadioButton runat="server" GroupName="gpoTipoGrafico" AutoPostBack="True" ID="rbtnTendenciaStack" Text="Tendencia Stack" /><br />
                                            <asp:RadioButton runat="server" GroupName="gpoTipoGrafico" AutoPostBack="True" ID="rbtnTendenciaBarraCompetitiva" Text="Tendencia Barra Comparativa" />
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="panel panel-primary">
                                    <div class="panel-heading">
                                        Stack
                                    </div>
                                    <div class="panel-body">
                                        <asp:RadioButton runat="server" Text="Ubicaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnUbicaciones"/>
                                        <br />
                                        <asp:RadioButton runat="server" Text="Organizaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnOrganizaciones"/>
                                        <br />
                                        <asp:RadioButton runat="server" Text="Tipo Ticket" AutoPostBack="True" GroupName="Stack" ID="rbtnTipoTicket"/>
                                        <br />
                                        <asp:RadioButton runat="server" Text="Tipificaciones" AutoPostBack="True" GroupName="Stack" ID="rbtnTipificaciones"/>
                                        <br />
                                        <asp:RadioButton runat="server" Text="Estatus Ticket" AutoPostBack="True" GroupName="Stack" ID="rbtnEstaus"/>
                                        <br />
                                        <asp:RadioButton runat="server" Text="SLA" AutoPostBack="True" GroupName="Stack" ID="rbtnSla"/>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-footer text-center">
                                <asp:Button runat="server" CssClass="btn btn-success" Text="Generar" ID="btnGenerar" OnClick="btnGenerar_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
