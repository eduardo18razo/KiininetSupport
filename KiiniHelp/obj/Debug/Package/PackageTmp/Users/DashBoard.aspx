<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="KiiniHelp.Users.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript' src='https://www.gstatic.com/charts/loader.js'></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

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
                            <div class="module-inner">

                                <div class="row text-center">
                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Usuarios Registrados</label>
                                            <br />
                                            <asp:Label runat="server" ID="Label1" Text="Actual"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblUsuariosRegistrados" CssClass="h4">210</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Usuarios Activos</label>
                                            <br />
                                            <asp:Label runat="server" ID="Label2" Text="Actual"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblUsuariosActivos" CssClass="h4">98%</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Tickets Creados</label>
                                            <br />
                                            <asp:Label runat="server" ID="Label4" Text="Histórico"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTicketsCreados" CssClass="h4">1,520</asp:Label>
                                        </div>
                                    </div>

                                    <div class="col-lg-3 col-md-3 col-sm-3 bordered">
                                        <div class="form-group padding-10-top">
                                            <label class="no-padding-right">Operadores</label>
                                            <br />
                                            <asp:Label runat="server" ID="Label6" Text="Actual"></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblOperadoresAcumulados" CssClass="h4">10</asp:Label>
                                        </div>
                                    </div>

                                </div>

                                <br />

                                <div class="row">

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Tickets Creados por Canal</label>
                                                </div>

                                                <asp:Chart ID="cGraficoTicketsCanal" runat="server" Height="300px" Width="400px" Visible="True">
                                                    <Titles>
                                                        <asp:Title ShadowOffset="3" Name="Items" />
                                                    </Titles>
                                                    <Legends>
                                                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                                                    </Legends>
                                                    <Series>
                                                        <asp:Series Name="Default" />
                                                    </Series>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                                                    </ChartAreas>
                                                </asp:Chart>

                                            </div>
                                        </section>
                                    </div>

                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="row center-content-div">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Usuarios Registrados</label>
                                                </div>
                                                <asp:Chart ID="cGraficoUsuarios" runat="server" Height="300px" Width="400px" Visible="True">
                                                    <Titles>
                                                        <asp:Title ShadowOffset="3" Name="Items" />
                                                    </Titles>
                                                    <Legends>
                                                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                                                    </Legends>
                                                    <Series>
                                                        <asp:Series Name="Default" />
                                                    </Series>
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
                                            <div class="row center-content-div">
                                                <div class="form-group margin-right-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 text-center">Almacenamiento</label>
                                                </div>
                                                <div class="row">
                                                    <asp:label runat="server" CssClass="col-lg-6" ID="lblEspacio"></asp:label>
                                                    <asp:label runat="server" CssClass="col-lg-6" ID="lblArchivos"></asp:label>
                                                </div>
                                                <asp:Chart ID="cGraficoEspacio" runat="server" Height="300px" Width="400px" Visible="True">
                                                    <Titles>
                                                        <asp:Title ShadowOffset="3" Name="Items" />
                                                    </Titles>
                                                    <Legends>
                                                        <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="False" Name="Default" LegendStyle="Row" />
                                                    </Legends>
                                                    <ChartAreas>
                                                        <asp:ChartArea Name="ChartArea1" BorderWidth="0" />
                                                    </ChartAreas>
                                                </asp:Chart>
                                            </div>
                                        </section>
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true">
                                        <section class="module">
                                            <div class="module-inner row">
                                                <div class="form-group margin-left-5">
                                                    <label class="col-lg-12 col-md-12 col-sm-12 margin-top-10 margin-bottom-10" style="font-weight: bolder !important;">HELP CENTER</label>
                                                </div>

                                                <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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

                                                <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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
                                        </section>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6" draggable="true" style="cursor: move" id="MisMetricas">
                                        <section class="module">
                                            <div class="module-inner">
                                                <div class="row">
                                                    <div class="form-group margin-right-5">
                                                        <label class="col-lg-9 col-md-9 col-sm-9 text-left no-padding-right margin-top-10 margin-bottom-10" style="font-weight: bolder !important;">OPERADOR POR ROL</label>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="module-inner">
                                                        <asp:Repeater runat="server" ID="rptOperadorRol">
                                                            <ItemTemplate>
                                                                <div class="col-lg-6 col-md-6 col-sm-6">
                                                                    <div class="form-group margin-top-10 margin-bottom-10">
                                                                        <asp:Label runat="server" Text='<%# Eval("Descripcion")%>' />
                                                                    </div>
                                                                </div>
                                                                <div class="col-lg-6 col-md-6 col-sm-6">
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
                                        <section class="module">
                                            <div class="module-inner">
                                                <div class="row">

                                                    <div class="form-group margin-left-5">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 margin-top-10 margin-bottom-10" style="font-weight: bolder !important;">ADMINISTRADOR</label>
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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


                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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



                                                    <div class="form-group margin-left-5">
                                                        <label class="col-lg-12 col-md-12 col-sm-12 margin-top-10 margin-bottom-10" style="font-weight: bolder !important;">ATENCIÓN</label>
                                                    </div>

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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

                                                    <div class="module-inner col-lg-6 col-md-6 col-sm-6">
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
