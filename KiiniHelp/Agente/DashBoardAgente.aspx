<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="DashBoardAgente.aspx.cs" Inherits="KiiniHelp.Agente.DashBoard" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../assets/css/dashboards.css" rel="stylesheet" />
    <link href="../assets/css/fontello-codes.css" rel="stylesheet" />
    <link href="../assets/css/fontello-ie7-codes.css" rel="stylesheet" />
    <link href="../assets/css/fontello-ie7.css" rel="stylesheet" />
    <link href="../assets/css/fontello.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="full">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <section class="module fontCabecera">
                    <div class="module-inner">
                        <div class="row">
                            <label class="TitulosAzul">Dashboard</label>
                        </div>
                        <hr />
                        <div class="row module-inner">
                            <div class="col-lg-3 col-md-2 col-sm-2 margin-right-10">
                                <label>Grupo</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlGrupo" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo_OnSelectedIndexChanged" />
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-2 col-sm-2">
                                <label>Agente</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlAgente" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAgente_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <%--                        <hr />--%>
                        <div class="row">
                            <div class="module-inner">
                                <div class="row text-center">
                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right" style="background-color: #EEEEEE">
                                        <div class="form-group padding-10-top">
                                            <strong class="h5">Acumulado</strong>
                                        </div>
                                    </div>
                                    <div class="col-lg-10 col-md-10 col-sm-10 no-padding-right">
                                        <div class="form-group padding-10-top">
                                            <strong class="h5">Últimos 7 Días</strong>
                                            <hr />
                                        </div>
                                    </div>
                                </div>

                                <div class="row text-center">
                                    <div class="col-lg-2 col-md-2 col-sm-2 separador-vertical-derecho-bold" style="background-color: #EEEEEE">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Abiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsAcumulados" CssClass="contadores" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 separador-vertical-derecho-bold">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Abiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsAbiertos7dias" CssClass="contadores" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 separador-vertical-derecho-bold">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados7dias" CssClass="contadores" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2  separador-vertical-derecho-bold">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Resueltos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResueltos7dias" CssClass="contadores resaltaAzul" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right separador-vertical-derecho-bold">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Resueltos vs Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResCreados7dias" CssClass="contadores" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera">Tickets Resueltos Reabiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsReabiertos7dias" CssClass="contadores resaltaNaranja" />
                                        </div>
                                    </div>
                                </div>

                                <br />

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12" id="MisMetricas">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="row">
                                                    <div class="col-lg-9 col-md-9 col-sm-9 text-left no-padding-right">
                                                        <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                            <div class="form-group text-center">
                                                                <label class="col-lg-12 col-md-12 col-sm-12 h5">Mis métricas esta semana</label>
                                                                <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                            </div>

                                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding-top">
                                                                <div class="col-lg-8 col-md-8 col-sm-8">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" Text="Tiempo Primera Respuesta (promedio)" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" Text="Tiempo de Resolución (promedio)" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" CssClass="margin-top-10 margin-bottom-10" Text="Resolución al Primer Contacto(promedio)" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" CssClass="margin-top-10 margin-bottom-10" Text="Intervenciones de Agentes (total)" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" CssClass="margin-top-10 margin-bottom-10" Text="Clientes Únicos Atendidos (total)" />
                                                                    </div>
                                                                </div>


                                                                <div class="col-lg-1 col-md-1 col-sm-1 text-right">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblTiempoPromedioPrimeraRespuestaActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblTiempoPromedioResolucionActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblResolucionAlPrimerContactoPromedioActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIntervencionesAgenteActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblClientesUnicosAtendidosActual" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-lg-1 col-md-1 col-sm-1 text-right">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIndicadorTiempoPromedioPrimeraRespuestaActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIndicadorTiempoPromedioResolucionActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIndicadorResolucionAlPrimerContactoPromedioActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIndicadorIntervencionesAgenteActual" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIndicadorClientesUnicosAtendidosActual" />
                                                                    </div>
                                                                </div>

                                                                <div class="col-lg-2 col-md-2 col-sm-2 text-right">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblTiempoPromedioPrimeraRespuestaPorcentaje" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblTiempoPromedioResolucionPorcentaje" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblResolucionAlPrimerContactoPromedioPorcentaje" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblIntervencionesAgentePorcentaje" />
                                                                    </div>
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" ID="lblClientesUnicosAtendidosPorcentaje" />
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3 text-left no-padding-right text-center">
                                                        <div class="col-lg-11 col-md-11 col-sm-11 col-xs-11">
                                                            <div class="form-group text-center">
                                                                <label class="col-lg-12 col-md-12 col-sm-12 no-padding-left no-padding-right h5">Semana pasada</label>
                                                                <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                            </div>

                                                            <div class="col-lg-4 col-md-4 col-sm-4 col-lg-offset-4 col-md-offset-4 col-sm-offset-4 col-xs-offset-4 text-right">
                                                                <div class="form-group margin-top-10 margin-bottom-10">
                                                                    <asp:Label runat="server" ID="lblTiempoPromedioPrimeraRespuestaAnterior" />
                                                                </div>
                                                                <div class="form-group margin-top-10 margin-bottom-10">
                                                                    <asp:Label runat="server" ID="lblTiempoPromedioResolucionAnterior" />
                                                                </div>
                                                                <div class="form-group margin-top-10 margin-bottom-10">
                                                                    <asp:Label runat="server" ID="lblResolucionAlPrimerContactoPromedioAnterior" />
                                                                </div>
                                                                <div class="form-group margin-top-10 margin-bottom-10">
                                                                    <asp:Label runat="server" ID="lblIntervencionesAgenteAnterior" />
                                                                </div>
                                                                <div class="form-group margin-top-10 margin-bottom-10">
                                                                    <asp:Label runat="server" ID="lblClientesUnicosAtendidosAnterior" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Tickets Abiertos</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsAbiertos">
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">

                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Prioridad</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsPrioridad">
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Tickets Creados por Canal</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsCanal">
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Tickets Creados vs Resueltos</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsCreadosAbiertos">
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>


                                    <div class="col-lg-6 col-md-6 col-sm-12">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Tickets por Grupos Principales</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <asp:Repeater runat="server" ID="rptMetricasGrupo">
                                                        <HeaderTemplate>
                                                            <div class="row">
                                                                <label class="col-lg-offset-3 col-lg-3 text-center">Total</label>
                                                                <label class="col-lg-3 text-center">Abiertos</label>
                                                                <label class="col-lg-3 text-center">Impacto Alto</label>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="row">
                                                                <label class="col-lg-3"><%# Eval("DescripcionGrupo") %>   </label>
                                                                <label class="col-lg-1"><%# Eval("TotalActual") %>        </label>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalPorcentaje") >= 0 ? "col-lg-1 icon-down-dir-fontello fontRed" : "col-lg-1 icon-up-dir-fontello fontGreen"%>' />
                                                                <%--<%# Eval("TotalAnterior") %>--%>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalPorcentaje") >= 0 ? "col-lg-1 borderright fontRed" : "col-lg-1 borderright fontGreen"%>' Text='<%# Eval("TotalPorcentaje") %>' />

                                                                <label class="col-lg-1"><%# Eval("TotalAbiertosActual") %>       </label>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalAbiertosPorcentaje") > 0 ? "col-lg-1 icon-down-dir-fontello fontRed" : "col-lg-1 icon-up-dir-fontello fontGreen"%>' />
                                                                <%--<label class="col-lg-1"><%# Eval("TotalAbiertosAnterior") %>     </label>--%>
                                                                <%--<label class="col-lg-1 borderright"><%# Eval("TotalAbiertosPorcentaje") %>   </label>--%>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalAbiertosPorcentaje") > 0 ? "col-lg-1 borderright fontRed" : "col-lg-1 borderright fontGreen"%>' Text='<%# Eval("TotalAbiertosPorcentaje") %>' />


                                                                <label class="col-lg-1"><%# Eval("TotalImpactoAltoActual") %>    </label>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalImpactoAltoPorcentaje") >= 0 ? "col-lg-1 icon-down-dir-fontello fontRed" : "col-lg-1 icon-up-dir-fontello fontGreen"%>' />
                                                                <%--<label class="col-lg-1"><%# Eval("TotalImpactoAltoAnterior") %>  </label>--%>
                                                                <%--<label class="col-lg-1"><%# Eval("TotalImpactoAltoPorcentaje") %></label>--%>
                                                                <asp:Label runat="server" CssClass='<%# (int)Eval("TotalImpactoAltoPorcentaje") >= 0 ? "col-lg-1 borderright fontRed" : "col-lg-1 borderright fontGreen"%>' Text='<%# Eval("TotalImpactoAltoPorcentaje") %>' />

                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
