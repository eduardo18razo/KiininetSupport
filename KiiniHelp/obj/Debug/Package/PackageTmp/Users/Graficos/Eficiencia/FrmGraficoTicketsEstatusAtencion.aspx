<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficoTicketsEstatusAtencion.aspx.cs" Inherits="KiiniHelp.Users.Graficos.Eficiencia.FrmGraficoTicketsEstatusAtencion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleGeograficoTickets.ascx" TagPrefix="uc1" TagName="UcDetalleGeograficoTickets" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Src="~/UserControls/Filtros/Graficos/Eficiencia/UcFiltrosTicketEstatusAtencion.ascx" TagPrefix="uc1" TagName="UcFiltrosTicketEstatusAtencion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <style>
        .RadComboBox {
            width: 100% !important;
        }
        .RadComboBoxDropDown_Default,.rcbWidth,.RadComboBoxDropDown_Default:before,.rcbWidth:before {
            width: 250px !important
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ol class="breadcrumb">
                    <li>
                        <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                    <li>Graficos</li>
                    <li class="active">Tickets: Estatus de Atención</li>
                </ol>

                <section class="module">
                    <div class="row">
                        <div class="module-inner">
                            <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                                <div class="module-heading">
                                    <h3 class="module-title">
                                        <asp:Label runat="server" ID="lblSeccion" Text="Tickets: Estatus de Atención" /></h3>
                                </div>
                                <p>
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <uc1:UcFiltrosTicketEstatusAtencion runat="server" ID="UcFiltrosTicketEstatusAtencion" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </p>
                            </div>

                        </div>
                    </div>
                </section>

                <asp:HiddenField runat="server" ID="hfRegion" />
                <section class="module module-headings">
                    <div class="module-inner">

                        <div class="module-content collapse in" id="content-1">
                            <div class="module-content-inner no-padding-bottom">
                                <div class="row">
                                    <div class="center-content-div">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" Visible="True" ID="upGrafica">
                                            <ContentTemplate>
                                                <asp:HiddenField runat="server" ID="hfGraficaGenerada" Value="false" />
                                                <iframe name="geocharts" runat="server" id="frameGeoCharts" visible="false" class="frameGraficas"></iframe>
                                                
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsPie"></tc:RadHtmlChart>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsStack"></tc:RadHtmlChart>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsPareto"></tc:RadHtmlChart>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="text-center">
                    <%--<asp:Button runat="server" CssClass="btn btn-success hidden" ID="btnDetalleGeografico" Text="Graficar" OnClick="btnDetalleGeografico_OnClick" />--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="modal fade" id="modalDetalleGeografico" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
            <asp:UpdatePanel ID="upDetalleGeografico" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <uc1:UcDetalleGeograficoTickets runat="server" ID="ucDetalleGeograficoTickets" />
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
                                    AutoGenerateColumns="false">
                                    <EmptyDataTemplate>
                                        ¡No hay resultados para mostrar!
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="IdTicket" Width="40px" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("IdTicket") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Canal" Width="40px" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Canal") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="TU" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: "  + Eval("TipoUsuario.Color") + " !important" %>'>
                                                    <%# Eval("TipoUsuario.Abreviacion") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoEspecialConsulta" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoEspecialConsulta") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="40px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoAtendedor" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoAtendedor") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoMantenimiento" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoMantenimiento") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="GrupoDesarrollo" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("GrupoDesarrollo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="700px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Organización" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Organizacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="700px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Ubicación" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Ubicacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="100px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="ServicioIncidente" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("ServicioIncidente") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="120px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Tipificación" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Tipificacion") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="60px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Prioridad" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Prioridad") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="45px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Estatus" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Estatus") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="45px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Urgencia" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Urgencia") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="45px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Impacto" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Impacto") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Sla" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Sla") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="45px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="Respuesta" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("Respuesta") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ControlStyle-Width="30px">
                                            <HeaderTemplate>
                                                <asp:Label runat="server" Text="FechaHora" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#Eval("FechaHora") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="text-center">
                                <br />
                                <%--<asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" />--%>

                            </div>
                            <%-- </div>--%>
                        </div>
                        <br />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
