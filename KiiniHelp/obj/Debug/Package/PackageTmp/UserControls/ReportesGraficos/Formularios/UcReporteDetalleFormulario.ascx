<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReporteDetalleFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.Formularios.UcReporteDetalleFormulario" %>

<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasConsultas.ascx" TagPrefix="uc1" TagName="UcFiltroFechasConsultas" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style>
    .pull-bottom {
        display: inline-block;
        vertical-align: bottom;
        float: none;
    }
</style>
<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdFormulario" />
            <asp:HiddenField runat="server" ID="hfNombreformulario" />

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/FrmReportes.aspx">Analíticos</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/ReportesGraficos/Formularios/FrmFormularios.aspx">Formularios</asp:HyperLink>
                </li>
                <li class="active"><asp:Label runat="server" ID="lblTituloFormulario"></asp:Label></li>
            </ol>

            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading clearfix">
                                <h3 class="module-title col-lg-8 col-md-7 col-sm-12 col-xs-12">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Formulario Reporte" /></h3>
                                <h3 class="module-title col-lg-4 col-md-5 col-sm-12 col-xs-12">
                                    <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                </h3>
                            </div>
                            
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">

                            <div class="col-lg-2 col-md-3 col-sm-4 col-xs-12">
                                <div class="form-group">
                                    <uc1:UcFiltroFechasConsultas runat="server" ID="ucFiltroFechasConsultas" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-md-3 col-sm-3 col-xs-12 pull-bottom">
                                <div class="form-group">
                                    <label class="col-lg-12 col-md-12 col-sm-12" style="color: transparent">h</label>
                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-11 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary" OnClick="btnBuscar_OnClick">Aplicar</asp:LinkButton>
                                        <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">Descargar</asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            
                        </div>


                    </div>
            </section>
            <!--/MÓDULO-->
            <section class="module">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <tc:RadGrid RenderMode="Classic" runat="server" ID="rgResult" AllowPaging="True" AllowSorting="true" PageSize="15"
                                FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true" AllowFilteringByColumn="True"
                                OnFilterCheckListItemsRequested="rgResult_OnFilterCheckListItemsRequested" FilterCheckListEnableLoadOnDemand="True"
                                OnSortCommand="rgResult_SortCommand" OnPageIndexChanged="rgResult_PageIndexChanged" OnPageSizeChanged="rgResult_PageSizeChanged"
                                OnNeedDataSource="rgResult_NeedDataSource" EnableViewState="True" OnItemDataBound="rgResult_OnItemDataBound"
                                OnBiffExporting="rgResult_BiffExporting">
                                <MasterTableView AutoGenerateColumns="true" IsFilterItemExpanded="True" CommandItemDisplay="None" EnableViewState="True">
                                </MasterTableView>
                                <ClientSettings>
                                    <Scrolling AllowScroll="True"></Scrolling>
                                </ClientSettings>
                            </tc:RadGrid>
                            <tc:RadGrid RenderMode="Classic" runat="server" ID="rgExport" OnBiffExporting="rgResult_BiffExporting" Visible="False" EnableViewState="True">
                                <MasterTableView AutoGenerateColumns="true" EnableViewState="True"></MasterTableView>
                            </tc:RadGrid>
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

