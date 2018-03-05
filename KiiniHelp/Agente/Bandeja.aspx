<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="Bandeja.aspx.cs" Inherits="KiiniHelp.Agente.Bandeja" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusAsignacion.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusAsignacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleTicket.ascx" TagPrefix="uc1" TagName="UcDetalleTicket" %>
<%@ Register Src="~/UserControls/Agentes/UcAsignacionUsuario.ascx" TagPrefix="uc1" TagName="UcAsignacionUsuario" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rcbList li img {
            width: 30px;
        }

        .RadGrid{
            border-style: none !important;
        }

        .RadGrid_Default .rgAltRow {
            background: #ffffff;
        }

        .rgRow .rgHoveredRow {
            /*background: violet !important;*/
        }

        .RadGrid_Default .rgHeader {
            background:#ffffff 0 -2300px !important;
        }

        .RadGrid_Default .rgHeader, .RadGrid_Default th.rgResizeCol, .RadGrid_Default .rgHeaderWrapper{
            border-bottom: 2px solid #ddd !important;
        }



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfTicketActivo" />
                <asp:HiddenField runat="server" ID="hfFiltroSla" Value="false" />
                <asp:HiddenField runat="server" ID="fhFiltroSinAsignar" Value="false" />
                <div>
                    <div class="row">
                        <div class="col-lg-10 col-md-8 col-sm-8 no-padding-right">
                            <div class="module-inner">

                                <section class="module">
                                    <div class="module-inner">
                                        <div class="row ">
                                            <div class="module-heading col-lg-12 col-md-12 col-sm-12">
                                                <div class="module-title">
                                                    <asp:Label runat="server" CssClass="TitulosAzul" ID="lblTicketAbiertosHeader" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--empieza div botones operar ticket--%>
                                        <div class="row" style="margin-top: 18px">
                                            <div class="col-lg-10 col-md-10 col-sm-10 no-padding-left">
                                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-right-10 margin-bottom-10" ID="btnAutoasignar" OnClick="btnAutoasignar_OnClick">
                                                        <i class="fa fa-long-arrow-down"></i>
                                                        Asignármelo
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-right-10 margin-bottom-10" ID="btnAsignar" OnClick="btnAsignar_OnClick">
                                                        <i class="fa fa-long-arrow-right"></i>
                                                        Asignar
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-right-10 margin-bottom-10" ID="btnCambiarEstatus" OnClick="btnCambiarEstatus_OnClick" Visible="False">
                                                        <i class="fa fa-long-arrow-right"></i>
                                                        Cambiar Estatus
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-right-10 margin-bottom-10" ID="btnRefresh" OnClick="btnRefresh_OnClick">
                                                        <i class="fa fa-refresh"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn fa fa-long-arrow-right margin-bottom-10" Text="Escalar" ID="btnEscalar" OnClick="btnEscalar_OnClick" Visible="False" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2 text-right no-padding-right">
                                                <asp:LinkButton runat="server" CssClass="btn btn-success" OnClick="OnClick">
                                            <i class="fa fa-plus"></i>  Nuevo Ticket                                          
                                                </asp:LinkButton>
                                            </div>

                                        </div>
                                        <%--termina div botones operar ticket--%>
                                        <%-- --%>
                                        <asp:HiddenField runat ="server" ID="hfFilaSeleccionada" />
                                        <tc:RadGrid runat="server" ID="gvTickets" CssClass="table table-striped display margin-top-10"
                                            FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                            PagerStyle-AlwaysVisible="true" OnFilterCheckListItemsRequested="gvTickets_OnFilterCheckListItemsRequested"
                                            OnNeedDataSource="gvTickets_OnNeedDataSource" AllowFilteringByColumn="True" OnItemCommand="gvTickets_OnItemCommand"
                                            OnSelectedIndexChanged="gvTickets_SelectedIndexChanged" 
                                            PageSize="14" PagerStyle-PageButtonCount="10" OnItemCreated="gvTickets_OnItemCreated"
                                            AllowPaging="True" AllowSorting="true" ShowGroupPanel="False" RenderMode="Classic">
                                            <GroupingSettings ShowUnGroupButton="False" CaseSensitive="False" />

                                            <ExportSettings ExportOnlyData="true" IgnorePaging="true"></ExportSettings>
                                            <MasterTableView AutoGenerateColumns="False" TableLayout="Fixed" ShowHeadersWhenNoRecords="True" CommandItemDisplay="None"
                                                DataKeyNames="NumeroTicket" NoDetailRecordsText="No hay Registros" HeaderStyle-CssClass="textoTabla" HeaderStyle-Font-Bold="true"
                                                HeaderStyle-Font-Names="Proxima Nova" HeaderStyle-ForeColor="#6E6E6E"
                                                ItemStyle-Font-Names="Proxima Nova" ItemStyle-ForeColor="#6E6E6E"
                                                AlternatingItemStyle-Font-Names="Proxima Nova" AlternatingItemStyle-ForeColor="#6E6E6E"
                                                FooterStyle-BackColor="White">

                                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="False" ></CommandItemSettings>
                                                <Columns>
                                                    <tc:GridClientSelectColumn UniqueName="chkSelected" ColumnGroupName="1" >
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                    </tc:GridClientSelectColumn>

                                                    <tc:GridTemplateColumn UniqueName="Sla" HeaderText="" DataField="DentroSla" FilterCheckListEnableLoadOnDemand="True" SortExpression="Sla" ShowFilterIcon="True" AllowFiltering="True">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i class='<%# (bool)Eval("DentroSla") ? "fa fa-bomb fontGreen iconoFont" : "fa fa-bomb fontRed iconoFont" %>'></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn FilterCheckListEnableLoadOnDemand="True" HeaderText=" " SortExpression="ImagenPrioridad" AutoPostBackOnFilter="True" ShowFilterIcon="False" AllowFiltering="False" UniqueName="ImagenPrioridad">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i runat="server" class="fa fa-exclamation fontRed iconoFont" visible='<%# Eval("Impacto").ToString() == "Alto"%>'></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn UniqueName="Vip" HeaderText="" FilterCheckListEnableLoadOnDemand="True" ShowFilterIcon="False" AllowFiltering="False">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i runat="server" class="fa fa-star iconoFont" visible='<%# Eval("Vip") %>'></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn UniqueName="TipoUsuario" Visible="false" DataField="UsuarioSolicito.TipoUsuario.Descripcion" FilterCheckListEnableLoadOnDemand="True" HeaderText="TU" AllowFiltering="True" ShowFilterIcon="False">
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                        <ItemTemplate>

                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "top:0; Border: none !important; Background: " + Eval("UsuarioSolicito.TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("UsuarioSolicito.TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="NumeroTicket" HeaderText="Ticket" SortExpression="NumeroTicket" ShowFilterIcon="False"
                                                        UniqueName="NumeroTicket" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" Visible="false" DataField="Canal" HeaderText="Canal" SortExpression="Canal" UniqueName="Canal" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridTemplateColumn FilterCheckListEnableLoadOnDemand="True" DataField="UsuarioSolicito.NombreCompleto" HeaderText="Solicitante" SortExpression="UsuarioSolicito.NombreCompleto" UniqueName="Solicitante"
                                                        CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%#Eval("UsuarioSolicito.NombreCompleto").ToString(). Length > 12 ? Eval("UsuarioSolicito.NombreCompleto").ToString().Substring(0, 12) : Eval("UsuarioSolicito.NombreCompleto") %>' ID="UsuarioSolicito" />
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="Tipificacion" HeaderText="Asunto" SortExpression="NumeroTicket" UniqueName="Tipificacion" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" Visible="false" DataField="TipoTicketAbreviacion" HeaderText="Tipo" SortExpression="TipoTicketAbreviacion" UniqueName="Tipo" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="FechaHora" DataFormatString="{0:MM/dd/yy HH:mm}" HeaderText="Solicitado" SortExpression="FechaHora" UniqueName="Solicitado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>
                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="EstatusTicket.Descripcion" HeaderText="Estatus" SortExpression="EstatusTicket.Descripcion" UniqueName="EstatusTicket" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>
                                                    <tc:GridTemplateColumn FilterCheckListEnableLoadOnDemand="True" DataField="UsuarioAsignado" HeaderText="Asignado a" SortExpression="UsuarioAsignado" UniqueName="UsuarioAsignaco" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" Text='<%#Eval("UsuarioAsignado").ToString(). Length > 12 ? Eval("UsuarioAsignado").ToString().Substring(0, 12) : Eval("UsuarioAsignado") %>' ID="btnUsuarioAsignado" OnClick="btnUsuario_OnClick" CommandArgument='<%#Eval("IdUsuarioAsignado") %>' />
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>
                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" Visible="false" DataField="GrupoAsignado" HeaderText="Grupo" SortExpression="GrupoAsignado" UniqueName="GrupoAsignado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EsPropietario" HeaderText="Es propietario" Display="False" UniqueName="EsPropietario"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="IdGrupoAsignado" HeaderText="Id Grupo Asignado" Display="False" UniqueName="IdGrupoAsignado"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EstatusTicket.Id" HeaderText="Estatus Ticket" Display="False" UniqueName="IdEstatusTicket"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EstatusAsignacion.Id" HeaderText="Estatus Asignacion" Display="False" UniqueName="IdEstatusAsignacion"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="Asigna" HeaderText="PuedeAsignar" Display="False" UniqueName="puedeasignar"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="IdNivelAsignado" HeaderText="IdNivelAsignado" Display="False" UniqueName="IdNivelAsignado"></tc:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableAlternatingItems="True" EnableRowHoverStyle="True" EnablePostBackOnRowClick="True">
                                                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true" ></Selecting> <%--CellSelectionMode="Column"--%>
                                                <Resizing AllowResizeToFit="True"></Resizing>
                                            </ClientSettings>
                                        </tc:RadGrid>
                                    </div>
                                </section>
                            </div>
                        </div>

                        <div class="col-lg-2 col-md-4 col-sm-4">
                            <section class="module">
                                <div class="module-inner padding-18-left padding-18-right">

                                    <div class="module-heading col-lg-12 col-md-12 col-sm-12">
                                        <div class="module-title margin-left-15">
                                            <asp:Label runat="server" Text="Filtros" CssClass="TitulosAzul" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="form-group">
                                            <asp:Label runat="server" class="col-sm-9 control-label" Text="Grupo" />
                                            <asp:DropDownList runat="server" ID="ddlGrupo" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlGrupo_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group">
                                            <asp:Label runat="server" class="col-sm-9 control-label" Text="Agente" />
                                            <asp:DropDownList runat="server" ID="ddlAgente" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlAgente_OnSelectedIndexChanged" />
                                        </div>
                                    </div>

                                    <div class="module-heading col-lg-12 col-md-12 col-sm-12">
                                        <div class="module-title margin-left-15">
                                            <asp:Label runat="server" Text="Tickets" CssClass="TitulosAzul" />
                                        </div>
                                    </div>

                                    <div class="row borderbootom padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" ButtonType="StandardButton" EnableEmbeddedBaseStylesheet="True" Text="Todos"
                                                ID="btnFiltroTodos" EnableEmbeddedSkins="False"
                                                CommandArgument="Todos" Style="text-align: left" OnClick="btnFiltro_OnClick">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 t14" runat="server" Text="Todos" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsAbiertos" Style="text-align: right">0</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>

                                    <div class="row borderbootom padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Abiertos" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" ButtonType="StandardButton" EnableEmbeddedBaseStylesheet="True" ID="btnFiltroAbierto" EnableEmbeddedSkins="False"
                                                CommandArgument="Abierto" Style="text-align: left" OnClick="btnFiltro_OnClick">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 t14" runat="server" Text="Abiertos" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsAbiertos" Style="text-align: right">0</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>
                                    <div class="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Tickets Sin Asignar" ID="btnFiltroSinAsignar" CommandArgument="SinAsignar" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 t14" runat="server" Text="Sin asignar" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsSinAsignar" Style="text-align: right">0</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>
                                    <div class="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="En espera" ID="btnFiltroEspera" CommandArgument="Espera" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 t14" runat="server" Text="Pendientes" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsEspera" Style="text-align: right">2</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>

                                    <div class="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Recien Resuelto" ID="btnFiltroResuelto" CommandArgument="Resuelto" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <div class="col-sm-8 col-md-8 col-lg-8 t14">
                                                        <asp:Label runat="server" Text="Recién resueltos" /><br />
                                                        <asp:Label runat="server" Text="(36hrs)" />
                                                    </div>
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsResueltos" Style="text-align: right">1000000</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>
                                    <div class="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Fuera SLA" ID="btnFueraSla" CommandArgument="FueraSla" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 t14" runat="server" Text="Fuera de SLA" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsFueraSla" Style="text-align: right">0</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>

                                    <div class="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Recien actualizados" ID="RadButton1" CommandArgument="recienActualizados" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <div class="col-sm-8 col-md-8 col-lg-8 t14">
                                                        <asp:Label runat="server" Text="Recién actualizados" /><br />
                                                        <asp:Label runat="server" Text="(60min)" />
                                                    </div>
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 t14" runat="server" ID="lblTicketsFueraSla" Style="text-align: right">1000000</asp:Label>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </div>

                                </div>
                            </section>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleTicket" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <uc1:UcDetalleTicket runat="server" ID="UcDetalleTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalAsignacionCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusAsignacion runat="server" ID="ucCambiarEstatusAsignacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" ID="UcCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalDetalleUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <%--style="background-color: #dbdbdb;"--%>
                            <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalNuevoTicketAgente" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <%-- <div class="modal-body">--%>
                        <uc1:UcAsignacionUsuario runat="server" ID="ucAsignacionUsuario" />
                        <%-- </div>--%>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="modalComentarioObligado" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCerrarModalComentarios_OnClick" Text='&times' />
                            <h6 class="modal-title">
                                <asp:Label runat="server" ID="lblTitleCatalogo" />
                            </h6>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <asp:Label runat="server" Text="Comentarios" CssClass="col-lg-3 margin-top-9" />
                                <div class="col-lg-9 no-padding-right">
                                    <asp:TextBox runat="server" ID="txtComentarioAsignacion" CssClass="form-control" TextMode="MultiLine" Height="100px" />
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <asp:Button runat="server" ID="btnCerrarComentarios" Text="Aceptar" CssClass="btn btn-guardar" OnClick="btnCerrarComentarios_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
