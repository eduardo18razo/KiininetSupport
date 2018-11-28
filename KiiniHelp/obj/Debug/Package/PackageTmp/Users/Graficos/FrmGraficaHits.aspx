<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficaHits.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGraficaHits" %>

<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosGraficasHits.ascx" TagPrefix="uc1" TagName="UcFiltrosGraficasHits" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosParametrosGraficoHits.ascx" TagPrefix="uc1" TagName="UcFiltrosParametrosGraficoHits" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGeograficoHit.ascx" TagPrefix="uc1" TagName="UcDetalleGeograficoHit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upGeneral">
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
                <div class="panel-body">
                    <uc1:UcFiltrosGraficasHits runat="server" ID="ucFiltrosGraficasHits" />
                    <asp:HiddenField runat="server" ID="hfRegion" />
                    <div class="center-content-div">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" Visible="True" ID="upGrafica">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfGraficaGenerada" Value="false" />                                
                                <iframe name="geocharts" runat="server" id="frameGeoCharts" visible="False" style="width: 650px; height: 500px; border: none"></iframe>
                                <asp:Chart ID="cGrafico" runat="server" Width="700px" Height="500px" Visible="False">
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
                <div class="text-center"> 
                    <asp:Button runat="server" CssClass="btn btn-success hidden" ID="btnDetalleGeografico" Text="Graficar" OnClick="btnDetalleGeografico_OnClick" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalFiltroParametros" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upFiltroUbicacion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcFiltrosParametrosGraficoHits runat="server" ID="UcFiltrosParametrosGraficoHits" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleGeografico" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upDetalleGeografico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcDetalleGeograficoHit runat="server" ID="ucDetalleGeograficoHit" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

   <div class="modal fade" id="modalDetalleGrafico" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upDetalleGrafico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg widht1310px">
                    <div class="modal-content widht1310px">
                            <div class="panel-body widht100" style="overflow: scroll; height: 500px;">
                            <asp:GridView runat="server" ID="gvResult" CssClass="table table-striped display table-responsive" AutoGenerateColumns="false" >
                                    <EmptyDataTemplate>
                                ¡No hay resultados para mostrar!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Hit" Width="40px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("IdHit") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="TU" Width="30px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuarioColor") + " !important; width: 25px !important" %>'>
                                            <%# Eval("TipoUsuarioAbreviacion") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Fecha" Width="30px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("FechaHora") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Hora" Width="30px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Hora") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Organización" Width="700px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Organizacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Ubicación" Width="700px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Tipificación" Width="350px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Vip" Width="30px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Vip") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label runat="server" Text="Total" Width="40px" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%#Eval("Total") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                
                                </asp:GridView>
                            </div>
                            <div class="text-center">  
                                <br />
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" />
                                <br />
                            </div>
                         <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


