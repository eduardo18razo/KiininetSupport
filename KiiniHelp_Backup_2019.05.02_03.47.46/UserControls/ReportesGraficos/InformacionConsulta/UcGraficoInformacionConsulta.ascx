﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcGraficoInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.InformacionConsulta.UcGraficoInformacionConsulta" %>

<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2019.1.215.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

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
                <li class="active">Net Promoter Score</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="NPS: " /><asp:Label runat="server" ID="lblTitulo"></asp:Label></h3>
                            </div>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="col-lg-4 col-md-5 col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <uc1:ucfiltrofechasgrafico runat="server" id="ucFiltroFechasGrafico" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-4 col-xs-11 pull-bottom">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary " OnClick="btnBuscar_OnClick">Aplicar</asp:LinkButton>
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
                                                <div class="col-lg-12 col-md-12 col-sm-12" draggable="true">
                                                    <section class="module bloque500">
                                                        <div class="row center-content-div">
                                                            <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
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
                                                <div class="col-lg-12 col-md-12 col-sm-12" draggable="true">
                                                    <section class="module bloque500">
                                                        <div class="row center-content-div">
                                                            <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                                <div class="form-group text-center clearfix">
                                                                    <label class="col-lg-12 col-md-12 col-sm-12 h5">Tendencia</label>
                                                                </div>
                                                                <div class="form-group text-center clearfix" style="height: 400px">
                                                                    <tc:RadHtmlChart runat="server" ID="rhcLikeBarra" Width="100%" EnableViewState="True">
                                                                        <ClientEvents OnLoad="chartLoadColumn"></ClientEvents>
                                                                    </tc:RadHtmlChart>
                                                                </div>
                                                                <div class="form-group text-center clearfix">
                                                                    <asp:Label class="col-lg-12 col-md-12 col-sm-12 h6 no-padding-top" runat="server" ID="lblPieTendencia" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </section>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</div>