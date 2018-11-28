<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaMisTickets.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaMisTickets" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>

<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2017.2.711.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfMuestraEncuesta" />

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="active">Mis Tickets</li>
            </ol>

            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Mis Tickets" /></h3>
                            </div>
                        </div>
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="col-xs-12 col-sm-12 col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta de Tickets:</label>
                                    <div class="col-xs-11 col-sm-11 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-1 col-sm-1 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
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
                            <div class="table-responsive">

                                <tc:RadGrid runat="server" ID="gvTickets" CssClass="table table-striped display"
                                    FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                    PagerStyle-AlwaysVisible="true"
                                    OnNeedDataSource="gvTickets_OnNeedDataSource" AllowFilteringByColumn="True" OnItemCommand="gvTickets_OnItemCommand"
                                    PageSize="14" PagerStyle-PageButtonCount="10" OnFilterCheckListItemsRequested="gvTickets_OnFilterCheckListItemsRequested"
                                    AllowPaging="True" AllowSorting="true" ShowGroupPanel="False" RenderMode="Classic">
                                    <GroupingSettings ShowUnGroupButton="False" CaseSensitive="False" />

                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true"></ExportSettings>
                                    <MasterTableView AutoGenerateColumns="False" TableLayout="Fixed" ShowHeadersWhenNoRecords="True" CommandItemDisplay="None"
                                        DataKeyNames="IdTicket" NoDetailRecordsText="No hay Registros" HeaderStyle-CssClass="textoTabla" HeaderStyle-Font-Bold="true"
                                        HeaderStyle-Font-Names="Proxima Nova" HeaderStyle-ForeColor="#6E6E6E" 
                                        ItemStyle-Font-Names="Proxima Nova" ItemStyle-ForeColor="#6E6E6E"
                                        AlternatingItemStyle-Font-Names="Proxima Nova" AlternatingItemStyle-ForeColor="#6E6E6E"
                                        FooterStyle-BackColor="White">

                                        <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="False"></CommandItemSettings>
                                        <Columns>
                                            <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="IdTicket" HeaderText="Ticket" Display="True" UniqueName="IdTicket"/>
                                            <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="Tipificacion" HeaderText="Asunto" Display="True" UniqueName="Tipificacion"/>
                                            <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="False" EnableHeaderContextMenu="False" DataField="FechaHora" HeaderText="Solicitado" Display="True" UniqueName="FechaHora"/>
                                            <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="Estatusticket.Descripcion" HeaderText="Estatus" Display="True" UniqueName="Estatusticket"/>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings EnableAlternatingItems="True" EnableRowHoverStyle="True" EnablePostBackOnRowClick="True">
                                        <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true"></Selecting>
                                        <Resizing AllowResizeToFit="True"></Resizing>
                                    </ClientSettings>
                                </tc:RadGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="modal fade" id="modalCambiaEstatus" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel ID="upCambiaestatus" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" ID="ucCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



</div>











