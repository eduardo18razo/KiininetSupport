﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficaTickets.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGraficaTickets" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosGraficasTicket.ascx" TagPrefix="uc1" TagName="UcFiltrosGraficasTicket" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/UcFiltrosParametrosGraficoTicket.ascx" TagPrefix="uc1" TagName="UcFiltrosParametrosGraficoTicket" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGeograficoTickets.ascx" TagPrefix="uc1" TagName="UcDetalleGeograficoTickets" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.1.215.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>


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
                    <uc1:UcFiltrosGraficasTicket runat="server" ID="ucFiltrosGraficas" />
                    <asp:HiddenField runat="server" ID="hfRegion"/>
                    <div class="center-content-div">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" Visible="True" ID="upGrafica">
                            <ContentTemplate>
                                <asp:HiddenField runat="server" ID="hfGraficaGenerada" Value="false" />
                                <iframe name="geocharts" runat="server" id="frameGeoCharts" Visible="false" class="frameGraficas">
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
                                <tc:RadHtmlChart runat="server" ID="rhcTickets"></tc:RadHtmlChart>
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
                        <uc1:UcFiltrosParametrosGraficoTicket runat="server" ID="ucFiltrosGrafico" />
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
                        <uc1:UcDetalleGeograficoTickets runat="server" id="ucDetalleGeograficoTickets" />
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
                            <div class="panel-body" style="overflow-y: auto; width: 100%; height: 500px;">
                                <asp:GridView runat="server" ID="gvResult" CssClass="table table-striped display table-responsive" Font-Size="11px"
                       AutoGenerateColumns ="false">  
                        <EmptyDataTemplate>
                                                ¡No hay resultados para mostrar!
                        </EmptyDataTemplate>
                        <Columns>  
                             <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="IdTicket" Width="40px"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("IdTicket") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>                          
                            <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Canal" Width="40px"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Canal") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="TU"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: "  + Eval("TipoUsuario.Color") + " !important" %>'>
                                            <%# Eval("TipoUsuario.Abreviacion") %></span>                                    
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="100px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoEspecialConsulta"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoEspecialConsulta") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoAtendedor"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoAtendedor") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoMantenimiento"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoMantenimiento") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="GrupoDesarrollo"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("GrupoDesarrollo") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="700px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Organización"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Organizacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="700px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Ubicación"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField  ControlStyle-Width="100px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="ServicioIncidente"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("ServicioIncidente") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="120px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Tipificación"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="60px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Prioridad"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Prioridad") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="45px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Estatus"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Estatus") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField ControlStyle-Width="45px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Urgencia"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Urgencia") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField ControlStyle-Width="45px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Impacto"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Impacto") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Sla"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Sla") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>    
                             <asp:TemplateField ControlStyle-Width="45px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="Respuesta"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("Respuesta") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>                                                         
                             <asp:TemplateField ControlStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" Text="FechaHora"/>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("FechaHora") %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                                       
                        </Columns> 
                       </asp:GridView>
                            </div>
                            <div class="text-center">
                                 <br />
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" />
                                
                            </div>
                       <%-- </div>--%>
                    </div>
                     <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>


