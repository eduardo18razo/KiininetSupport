<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="DashBoardAgente.aspx.cs" Inherits="KiiniHelp.Agente.DashBoard" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function handleDragStart(e) {
            this.style.opacity = '0.4';  // this / e.target is the source node.
        }

        var cols = document.querySelectorAll('#columns .column');
        [].forEach.call(cols, function (col) {
            col.addEventListener('dragstart', handleDragStart, false);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="full">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <section class="module">
                    <div class="module-inner">
                        <div class="row">
                            <label class="TitulosAzul">Dashboard</label>
                        </div>
                        <hr />
                        <div class="row">
                            <div class="col-lg-3 col-md-2 col-sm-2 no-padding-right">
                                <label>Grupo</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlGrupo" CssClass="ComboEstandar" AutoPostBack="true" OnSelectedIndexChanged="ddlGrupo_OnSelectedIndexChanged" />
                                </div>
                            </div>

                            <div class="col-lg-3 col-md-2 col-sm-2 no-padding-right">
                                <label>Agente</label>
                                <div>
                                    <asp:DropDownList runat="server" ID="ddlAgente" CssClass="ComboEstandar" AutoPostBack="true" OnSelectedIndexChanged="ddlAgente_OnSelectedIndexChanged" />
                                </div>
                            </div>
                        </div>

                        <hr />


                        <div class="row">
                            <div class="module-inner">
                                <div class="row text-center">
                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right bordered" style="background-color: #EEEEEE">
                                        <div class="form-group padding-10-top">
                                            <strong>Acumulado</strong>
                                        </div>
                                    </div>
                                    <div class="col-lg-10 col-md-10 col-sm-10 no-padding-right bordered">
                                        <div class="form-group padding-10-top">
                                            <strong>Últimos 7 Días</strong>
                                        </div>
                                    </div>
                                </div>

                                <div class="row text-center">
                                    <div class="col-lg-2 col-md-2 col-sm-2 bordered" style="background-color: #EEEEEE">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Abiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsAcumulados" CssClass="h4" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Abiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsAbiertos7dias" CssClass="h4" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados7dias" CssClass="h4" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2  bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResueltos7dias" CssClass="h4" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos vs Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResCreados7dias" CssClass="h4" />
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos Reabiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsReabiertos7dias" CssClass="h4" />
                                        </div>
                                    </div>
                                </div>

                                <br />

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true" id="MisMetricas">
                                        <section class="module">
                                            <div class="module-inner">
                                                <div class="row">
                                                    <div class="form-group margin-right-5">
                                                        <label class="col-lg-9 col-md-9 col-sm-9 text-left no-padding-right margin-top-10 margin-bottom-10" style="font-weight: bolder;">Mis métricas esta semana</label>
                                                        <label class="col-lg-3 col-md-3 col-sm-3 text-left no-padding-right margin-top-10 margin-bottom-10 text-center" style="font-weight: bolder;">Semana pasada</label>
                                                    </div>
                                                </div>

                                                <div class="row">

                                                    <div class="col-lg-6 col-md-6 col-sm-6">
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


                                                    <div class="col-lg-1 col-md-1 col-sm-1">
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

                                                    <div class="col-lg-1 col-md-1 col-sm-1">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="V" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="V" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="V" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="V" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="V" />
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-1 col-md-1 col-sm-1 borderright">
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

                                                    <div class="col-lg-3 col-md-3 col-sm-3 text-center">
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
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Abiertos</label>
                                                </div>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsAbiertos">
                                                </tc:RadHtmlChart>
                                            </div>
                                        </section>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Prioridad</label>
                                                </div>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsPrioridad">
                                                </tc:RadHtmlChart>
                                            </div>
                                        </section>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados por Canal</label>
                                                </div>
                                                <tc:RadHtmlChart runat="server" ID="rhcTicketsCanal">
                                                </tc:RadHtmlChart>
                                            </div>
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados vs Resueltos</label>
                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsCreadosAbiertos">
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets por Grupos Principales</label>
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
                                                                <label class="col-lg-1"><%# Eval("TotalActual") %>               </label>
                                                                <label class="col-lg-1"><%# Eval("TotalAnterior") %>             </label>
                                                                <label class="col-lg-1 borderright"><%# Eval("TotalPorcentaje") %>           </label>
                                                                <label class="col-lg-1"><%# Eval("TotalAbiertosActual") %>       </label>
                                                                <label class="col-lg-1"><%# Eval("TotalAbiertosAnterior") %>     </label>
                                                                <label class="col-lg-1 borderright"><%# Eval("TotalAbiertosPorcentaje") %>   </label>
                                                                <label class="col-lg-1"><%# Eval("TotalImpactoAltoActual") %>    </label>
                                                                <label class="col-lg-1"><%# Eval("TotalImpactoAltoAnterior") %>  </label>
                                                                <label class="col-lg-1"><%# Eval("TotalImpactoAltoPorcentaje") %></label>
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
