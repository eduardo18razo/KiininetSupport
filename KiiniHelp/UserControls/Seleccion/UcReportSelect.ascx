<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReportSelect.ascx.cs" Inherits="KiiniHelp.UserControls.Seleccion.UcReportSelect" %>
<style>
    .row-centered {
        text-align: center;
    }

    .col-centered {
        display: inline-block;
        float: none;
        text-align: left;
        vertical-align: top;
    }
</style>
<div id="full">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="text-theme">
                    <asp:LinkButton runat="server" OnClick="OnClick" Text="text-theme">Home</asp:LinkButton></li>
                <li class="active">
                    <asp:Label runat="server" Text="Analíticos" /></li>
            </ol>

            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module">
                        <div class="row">
                            <div class="module-inner">
                                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                                    <div class="module-heading no-border-bottom" runat="server" id="divCategoriaReference">
                                        <h3 class="text-center module-title">Analíticos</h3>
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
                        <div class="row module-inner row-centered">
                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 col-centered">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_1">
                                                <div class="text-center">
                                                    Tickets
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <div class="panel-collapse " id="faq5_1">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton1" Text='Estatus de atención' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsEstatusAtencion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton2" Text='Tipo de ticket' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsTipoTicket.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton3" Text='SLA' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsSla.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton5" Text='Asignación' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsEstatusAsignacion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton6" Text='Canal' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsCanal.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton7" Text='Top 20 Organización' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsOrganizacion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton8" Text='Top 20 Localización' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsUbicacion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="LinkButton4" Text='Top 20 Opciones' CssClass="text-theme" NavigateUrl="~/Users/Graficos/Eficiencia/FrmGraficoTicketsTipificacion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 col-centered">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq5_2">
                                                <div class="text-center">
                                                    Centro de Soporte
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <div class="panel-collapse" id="faq5_2">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink1" Text='Formularios' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/Formularios/FrmFormularios.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink2" Text='Artículos' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/InformacionConsulta/FrmLikeDontLike.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-12 col-xs-12 col-centered">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a data-toggle="collapse" class="no-underline" href="#faq6">
                                                <div class="text-center">
                                                    Encuestas
                                                </div>
                                            </a>
                                        </h4>
                                    </div>
                                    <div class="panel-collapse" id="faq6">
                                        <div class="panel-body">
                                            <div class="panel-group panel-group-theme-1">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink9" Text='Net Promoter Score' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/Encuestas/FrmEncuestas.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink10" Text='Escala 0 - 10' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/Encuestas/FrmEncuestaCalificacion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink11" Text='Escala de Valoración' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/Encuestas/FrmEncuestaSatisfaccion.aspx" />
                                                        </h4>
                                                    </div>
                                                </div>

                                                <div class="panel panel-default">
                                                    <div class="panel-heading ">
                                                        <h4 class="panel-title">
                                                            <asp:HyperLink runat="server" ID="HyperLink12" Text='Si o No' CssClass="text-theme" NavigateUrl="~/Users/ReportesGraficos/Encuestas/FrmEncuestaLogica.aspx" />
                                                        </h4>
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
