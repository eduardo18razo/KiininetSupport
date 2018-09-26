<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReportSelect.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcReportSelect" %>

<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="text-theme">
                    <asp:LinkButton runat="server" OnClick="OnClick" Text="text-theme">Home</asp:LinkButton></li>
                <li class="active">
                    <asp:Label runat="server" ID="lbltipoUsuario" /></li>
            </ol>

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module">
                        <div class="row">
                            <div class="module-inner">
                                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                                    <div class="module-heading no-border-bottom" runat="server" id="divCategoriaReference">
                                        <h3 class="text-center module-title">Reportes</h3>
                                    </div>
                                    <div class="module-heading no-border-bottom" runat="server" id="divCategoriaTitle" visible="False">
                                        <h3 class="module-title">
                                            <asp:Label runat="server" ID="lblCategoria"></asp:Label>
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section class="module">
                        <div class="row module-inner">
                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_1">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-star icon iconoFontXg"></i>
                                                    <br />
                                                    Eficiencia
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse " id="faq5_1">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            
                                                            <asp:HyperLink runat="server" ID="LinkButton1" Text='Estatus Atención' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsEstatusAtencion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton2" Text='Tipo Ticket' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsTipoTicket.aspx"/>
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton3" Text='Dentro y Fuera de SLA' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsSla.aspx"/>
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse" >
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton4" Text='Opción' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsTipificacion.aspx"/>
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton5" Text='Estatus Asignación' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsEstatusAsignacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton6" Text='Canal' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsCanal.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton7" Text='Organización' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsOrganizacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton8" Text='Estados' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_2">
                                                <div class="text-center">
                                                    <i class="pe-icon pe-7s-notebook icon iconoFontXg"></i>
                                                    <br />
                                                    Reportes Funcionales
                                                </div>
                                            </a></h4>
                                    </div>
                                    <div class="panel-collapse" id="faq5_2">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink1" Text='Formularios' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/ReportesGraficos/Formularios/FrmFormularios.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink2" Text='Artículos' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink3" Text='Encuesta NPS' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink4" Text='Encuesta Calificación' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink5" Text='Encuesta Satisfacción' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>
                                                
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink6" Text='Encuesta Logica' CssClass="text-theme" OnClick="verOpcion_Click" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                    <div class="panel-collapse">
                                                        <div class="panel-body">
                                                            DescripcionReporte
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </section>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</div>
