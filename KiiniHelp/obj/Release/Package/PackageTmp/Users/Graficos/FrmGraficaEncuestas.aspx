<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficaEncuestas.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGraficaEncuestas" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosGraficasEncuestas.ascx" TagPrefix="uc1" TagName="UcFiltrosGraficasEncuestas" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosParametrosGraficoEncuestas.ascx" TagPrefix="uc1" TagName="UcFiltrosParametrosGraficoEncuestas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
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
            <div class="panel panel-primary">
                <div class="panel-body">
                    <uc1:UcFiltrosGraficasEncuestas runat="server" ID="ucFiltrosGraficasEncuestas" />
                    <div class="center-content-div">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" Visible="True" ID="upGrafica">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfGraficado" Value="false"/>
                                <iframe name="geocharts" runat="server" ID="frameGeoCharts" width="800px" height="600px" Visible="False" style="width: 850px; height: 650px; border: none">
                                </iframe>
                                <asp:Chart ID="cGrafico" runat="server" Width="800px" Height="600px" Visible="False">
                                    <Titles>
                                        <asp:Title ShadowOffset="3" Name="Items" />
                                    </Titles>
                                    <Legends>
                                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" Title="Titulo" />
                                    </Legends>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                                    </ChartAreas>
                                </asp:Chart>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="panel-footer text-center">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnGraficar" Text="Graficar" OnClick="btnConsultar_OnClick" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="modalFiltroParametros" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upFiltroUbicacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcFiltrosParametrosGraficoEncuestas runat="server" ID="ucFiltrosParametrosGraficoEncuestas" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div class="modal fade" id="modalDetalleGrafico" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upDetalleGrafico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="width: 1310px">
                    <div class="modal-content" style="width: 1310px">
                        <div class="panel panel-primary">
                            <div class="panel-body" style="overflow-y: auto">
                                <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" />
                            </div>
                            <div class="panel-footer">
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>