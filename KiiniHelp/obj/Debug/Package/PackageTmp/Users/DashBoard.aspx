<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="KiiniHelp.Users.DashBoard" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        (function (global) {
            var chartPie;
            var chartBar;
            var chartPareto;
            var chartColumn;

            function chartLoadPie(sender, args) {
                chartPie = sender.get_kendoWidget();
            }
            function chartLoadPareto(sender, args) {
                chartPareto = sender.get_kendoWidget();
            }
            function chartLoadBar(sender, args) {
                chartBar = sender.get_kendoWidget();
            }
            function chartLoadColumn(sender, args) {
                chartBar = sender.get_kendoWidget();
            }

            global.chartLoadPie = chartLoadPie;
            global.chartLoadPareto = chartLoadPareto;
            global.chartLoadBar = chartLoadBar;
            global.chartLoadColumn = chartLoadColumn;

            function resizeChart() {
                if (chartPie)
                    chartPie.resize();
                if (chartBar)
                    chartBar.resize();
                if (chartPareto)
                    chartPareto.resize();
                if (chartColumn)
                    chartColumn.resize();
            }

            var to = false;
            window.onresize = function () {
                if (to !== false)
                    clearTimeout(to);
                to = setTimeout(resizeChart, 200);
            }

        })(window);
    </script>
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
                        <div class="row">
                            <div class="module-inner">
                                <div class="row text-center">
                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group padding-10-top separador-vertical-derecho-bold">
                                            <label class="no-padding-right textoCabecera"><i class="fa fa-user margin-right-4"></i>Usuarios Registrados (actual)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblUsuariosRegistrados" CssClass="contadores">210</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group padding-10-top separador-vertical-derecho-bold">
                                            <label class="no-padding-right textoCabecera"><i class="fa fa-clock-o margin-right-4"></i>Usuarios Activos (actual)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblUsuariosActivos" CssClass="contadores">98%</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group padding-10-top separador-vertical-derecho-bold">
                                            <label class="no-padding-right textoCabecera"><i class="fa fa-user margin-right-4"></i>Tickets Creados (histórico)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados" CssClass="contadores resaltaNaranja">1,520</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right textoCabecera"><i class="fa fa-user margin-right-4"></i>Agentes (actual)</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblOperadoresAcumulados" CssClass="contadores">10</asp:Label>
                                        </div>
                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12" draggable="true">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Tickets Creados por Canal</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcTicketsCanal">
                                                        <ClientEvents OnLoad="chartLoadPie" />
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12" draggable="true">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Usuarios Registrados</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcUsuarios" Width="100%" EnableViewState="True">
                                                        <ClientEvents OnLoad="chartLoadPie" />
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12 " draggable="true">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Almacenamiento</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>
                                                    <div class="row">
                                                        <asp:Label runat="server" CssClass="col-lg-6" ID="lblEspacio"></asp:Label>
                                                        <asp:Label runat="server" CssClass="col-lg-6" ID="lblArchivos"></asp:Label>
                                                    </div>
                                                    <tc:RadHtmlChart runat="server" ID="rhcEspacio" Width="100%" EnableViewState="True">
                                                        <ClientEvents OnLoad="chartLoadColumn" />
                                                    </tc:RadHtmlChart>
                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-12" draggable="true">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Help center</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-left">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Categorias" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Articulos" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Formularios" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Catalogos" />
                                                        </div>
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-right">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="164" ID="lblCategorias" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="110" ID="lblArticulos" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="57" ID="lblFormularios" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="57" ID="lblCatalogos" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-12" draggable="true" style="cursor: move" id="MisMetricas">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">
                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Agente por rol</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <div class="row">
                                                        <asp:Repeater runat="server" ID="rptOperadorRol">
                                                            <ItemTemplate>
                                                                <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-left">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" Text='<%# Eval("Descripcion")%>' />
                                                                    </div>
                                                                </div>
                                                                <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-right">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" Text='<%# Eval("Total")%>' />
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module bloque">
                                            <div class="row center-content-div">
                                                <div class="col-lg-10 col-md-10 col-sm-10 col-xs-10 col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1">

                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Administrador</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-left">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Organización" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Ubicación" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Puestos" />
                                                        </div>
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-right">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="164" ID="lblOrganizaciones" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="110" ID="lblUbicaciones" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="57" ID="lblPuestos" />
                                                        </div>

                                                    </div>

                                                    <div class="form-group text-center">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 h5">Atención</label>
                                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 bold" />
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-left">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Grupos" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Horarios" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="Feriados" />
                                                        </div>
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6 text-right">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="164" ID="lblGrupos" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="110" ID="lblHorarios" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="57" ID="lblFeriados" />
                                                        </div>

                                                    </div>
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
