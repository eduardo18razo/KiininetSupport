<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcReporteDetalleFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.ReportesGraficos.Formularios.UcReporteDetalleFormulario" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register TagPrefix="atk" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdFormulario" />
            <asp:HiddenField runat="server" ID="hfNombreformulario" />

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Administración</li>
                <li class="active">Usuarios</li>
            </ol>

            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Formulario Reporte" /></h3>
                            </div>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            
                            <div class="col-lg-1">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Visualizar:</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoFiltro" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoFiltro_OnSelectedIndexChanged">
                                            <asp:ListItem Text="Diario" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Semanal" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Mensual" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Anual" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-2 ">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Fecha Inicio:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaInicio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-2 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Fecha Fin:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" CssClass="form-control" type="date" step="1" ID="txtFechaFin" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" ID="btnBuscar" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4 col-lg-offset-6 col-md-offset-6 col-sm-offset-6 col-xs-offset-6 text-center">
                                <div class="form-group margin-top-btn-consulta">
                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>
                                </div>
                            </div>
                        </div>


                    </div>
            </section>
            <!--/MÓDULO-->
            <section class="module module-headings">
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
                                
                            </tc:RadGrid>
                            <tc:RadGrid RenderMode="Classic" runat="server" ID="rgExport" OnBiffExporting="rgResult_BiffExporting" Visible="False" EnableViewState="True"
                                >
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

