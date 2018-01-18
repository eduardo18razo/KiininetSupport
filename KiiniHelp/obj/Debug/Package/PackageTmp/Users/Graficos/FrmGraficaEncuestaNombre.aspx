<%@ Page Title="" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmGraficaEncuestaNombre.aspx.cs" Inherits="KiiniHelp.Users.Graficos.FrmGraficaEncuestaNombre" %>

<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upGeneral">
        <ContentTemplate>
            <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
                <div class="alert alert-danger">
                    <div>
                        <div style="float: left">
                            <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                        </div>
                        <div style="float: left">
                            <h3>Error</h3>
                        </div>
                        <div class="clearfix clear-fix" />
                    </div>
                    <hr />
                    <asp:Repeater runat="server" ID="rptErrorGeneral">
                        <ItemTemplate>
                            <ul>
                                <li><%# Container.DataItem %></li>
                            </ul>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </header>
            <div class="panel panel-primary">
                <div class="panel-body">
                    <asp:HiddenField runat="server" ID="hfGraficaGenerada" Value="false" />
                    <div class="form-inline">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Encuesta" CssClass="col-sm-2" />
                            <div class="col-sm-8">
                                <asp:DropDownList runat="server" ID="ddlEncuesta" CssClass="DropSelect" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button runat="server" ID="btnFiltroFechas" Text="Fecha" CssClass="btn btn-primary" OnClick="btnFiltroFechas_OnClick"/>
                            </div>
                        </div>
                    </div>
                    <div class="center-content-div">
                        <asp:Repeater runat="server" ID="rptGraficos" OnItemDataBound="rptGraficos_OnItemDataBound">
                            <HeaderTemplate>
                                <div class="panel panel-primary">
                                    <div class="panel-heading"></div>
                                    <div class="panel-body">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Chart ID="cGrafico" runat="server" Width="600px" Height="450px">
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

                            </ItemTemplate>
                            <FooterTemplate>
                                </div>
                            </div>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="panel-footer center-content-div">
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnbtnGraficar" Text="Graficar" OnClick="btnbtnGraficar_OnClick" />
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalFiltroFechas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upFiltroFechas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="width: 450px">
                    <div class="modal-content" style="width: 450px">
                        <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    
    <div class="modal fade" id="modalDetalleGrafico" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upDetalleGrafico" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" style="width: 1310px">
                    <div class="modal-content" style="width: 1310px">
                        <div class="panel panel-primary">
                            <div class="panel-body" style="overflow-y: auto">
                                <asp:GridView runat="server" ID="gvResult" CssClass="table table-bordered table-hover table-responsive" />
                            </div>
                            <div class="panel-footer">
                                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cerrar" ID="btnCerrar" OnClick="btnCerrar_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
