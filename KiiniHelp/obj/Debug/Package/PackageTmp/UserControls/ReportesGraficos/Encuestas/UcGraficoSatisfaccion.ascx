<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcGraficoSatisfaccion.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.Encuestas.UcGraficoSatisfaccion" %>

<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script>
    (function (global) {
        var chartPie;
        var chartColumn;
        var chartPareto;

        function chartLoadPie(sender, args) {
            chartPie = sender.get_kendoWidget();
        }
        function chartLoadPareto(sender, args) {
            chartPareto = sender.get_kendoWidget();
        }
        function chartLoadColumn(sender, args) {
            chartColumn = sender.get_kendoWidget();
        }

        global.chartLoadPie = chartLoadPie;
        global.chartLoadPareto = chartLoadPareto;
        global.chartLoadColumn = chartLoadColumn;

        function resizeChart() {
            if (chartPie)
                chartPie.resize();
            if (chartColumn)
                chartColumn.resize();
            if (chartPareto)
                chartPareto.resize();
        }

        var to = false;
        window.onresize = function () {
            if (to !== false)
                clearTimeout(to);
            to = setTimeout(resizeChart, 200);
        }

    })(window);
</script>
<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>   
            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/FrmReportes.aspx">Analíticos</asp:HyperLink></li>
                <li class="active">Escala de Valoración
                </li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Escala 0-10: " /><asp:Label runat="server" ID="lblTitulo"></asp:Label></h3>
                            </div>
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="col-lg-4 col-md-5 col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-4 col-xs-11 pull-bottom">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary" ID="btnBuscar" OnClick="btnBuscar_OnClick">Aplicar</asp:LinkButton>
                                        <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary margin-left-3" OnClick="btnDownload_OnClick"><i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </section>


            <section class="module">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="row">
                                <div class="center-content-div">
                                    <div class="row">
                                        <div class="module-inner">
                                            <div class="row">
                                                <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12" draggable="true">
                                                    <section class="module bloque500">
                                                        <div class="row center-content-div">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="form-group text-center clearfix">
                                                                    <label class="col-lg-12 col-md-12 col-sm-12 h5">Vista General</label>
                                                                </div>
                                                                <div class="form-group text-center clearfix" style="height: 400px">
                                                                    <tc:RadHtmlChart runat="server" ID="rhcLikePie" Width="100%" EnableViewState="True">
                                                                        <ClientEvents OnLoad="chartLoadPie" />
                                                                    </tc:RadHtmlChart>
                                                                </div>
                                                                <div class="form-group text-center clearfix">
                                                                    <asp:Label class="col-lg-12 col-md-12 col-sm-12 h6 no-padding-top" runat="server" ID="lblPieGeneral" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </section>
                                                </div>
                                                <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12" draggable="true">
                                                    <section class="module bloque500">
                                                        <div class="row center-content-div">
                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                <div class="form-group text-center clearfix">
                                                                    <label class="col-lg-12 col-md-12 col-sm-12 h5">Vista General</label>
                                                                </div>
                                                                <div class="form-group text-center clearfix" style="height: 400px">
                                                                    <tc:RadHtmlChart runat="server" ID="rhcLikeBarra" Width="100%" EnableViewState="True">
                                                                        <ClientEvents OnLoad="chartLoadColumn" />
                                                                    </tc:RadHtmlChart>
                                                                </div>
                                                                <div class="form-group text-center clearfix">
                                                                    <asp:Label class="col-lg-12 col-md-12 col-sm-12 h6 no-padding-top" runat="server" ID="lblPieTendencia" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </section>
                                                </div>

                                                <asp:Repeater runat="server" ID="rptPreguntas" OnItemDataBound="rptPreguntas_OnItemDataBound">
                                                    <ItemTemplate>
                                                        <%--Grafica Pie Pregunta--%>
                                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12" draggable="true">
                                                            <section class="module bloque500">
                                                                <div class="row center-content-div">
                                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                        <div class="form-group text-center clearfix">
                                                                            <label class="col-lg-12 col-md-12 col-sm-12 h5">Vista General - <asp:Label runat="server" ID="lblPreguntaTituloPie" Text="Pregunta"></asp:Label></label>
                                                                        </div>
                                                                        <div class="form-group text-center clearfix" style="height: 400px">
                                                                            <tc:RadHtmlChart runat="server" ID="rhGraficoPreguntaPie" Width="100%" EnableViewState="True">
                                                                                <ClientEvents OnLoad="chartLoadPie" />
                                                                            </tc:RadHtmlChart>
                                                                        </div>
                                                                        <div class="form-group text-center clearfix">
                                                                            <asp:Label class="col-lg-12 col-md-12 col-sm-12 h6 no-padding-top" runat="server" ID="lblPieGeneral" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </section>
                                                        </div>
                                                        <%--Grafica pregunta--%>
                                                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12" draggable="true">
                                                            <section class="module bloque500">
                                                                <div class="row center-content-div">
                                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                        <div class="form-group text-center clearfix">
                                                                            <label class="col-lg-12 col-md-12 col-sm-12 h5">Vista General - <asp:Label runat="server" ID="lblPreguntaTituloColumn" Text="Pregunta"></asp:Label></label>
                                                                        </div>
                                                                        <div class="form-group text-center clearfix" style="height: 400px">
                                                                            <tc:RadHtmlChart runat="server" ID="rhGraficoPregunta" Width="100%">
                                                                                <ClientEvents OnLoad="chartLoadColumn" />
                                                                            </tc:RadHtmlChart>
                                                                        </div>
                                                                        <div class="form-group text-center clearfix">
                                                                            <asp:Label class="col-lg-12 col-md-12 col-sm-12 h6 no-padding-top" runat="server" ID="lblPieTendencia" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </section>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
</div>
