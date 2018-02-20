<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="DashBoardAgente.aspx.cs" Inherits="KiiniHelp.Agente.DashBoard" %>

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
                                    <asp:DropDownList runat="server" ID="ddlAgente" CssClass="ComboEstandar" AutoPostBack="true" />
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
                                            <asp:Label runat="server" ID="lblTicketsAcumulados" CssClass="h4">8756</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Abiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsAbiertos7dias" CssClass="h4">8756</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados7dias" CssClass="h4">8756</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2  bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResueltos7dias" CssClass="h4">8756</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos vs Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsResCreados7dias" CssClass="h4">19%</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-2 col-md-2 col-sm-2 no-padding-right bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Resueltos Reabiertos</label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsReabiertos7dias" CssClass="h4">56%</asp:Label>
                                        </div>
                                    </div>
                                </div>

                                <br />

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true" style="cursor: move" id="MisMetricas">
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
                                                            <asp:Label runat="server" Text="4:05" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="4:05" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="18%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="15" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="0.5" />
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
                                                            <asp:Label runat="server" Text="38%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="10%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="4%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="34%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="5%" />
                                                        </div>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3 text-center">
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="4:05" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="4:05" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="15%" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="25" />
                                                        </div>
                                                        <div class="form-group margin-top-10 margin-bottom-10">
                                                            <asp:Label runat="server" Text="15" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">

                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Abiertos</label>
                                                </div>

                                                 <asp:Chart ID="Chart3" runat="server" Width="400px" Height="300px" Visible="true">
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

                                            </div>
                                        </section>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Prioridad</label>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados por Canal</label>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados vs Resueltos</label>
                                                </div>
                                            </div>
                                        </section>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets por Grupos Principales</label>
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
