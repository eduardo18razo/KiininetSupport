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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfTicketActivo" />
                <asp:HiddenField runat="server" ID="hfFiltroSla" Value="false" />
                <asp:HiddenField runat="server" ID="fhFiltroSinAsignar" Value="false" />
                <div class="module">
                    <div class="row">
                        <div class="col-lg-9 col-md-7 col-sm-7 no-padding-right borderright">

                            <div class="module-inner no-padding-right">
                                <div class="row " >
                                    <div class="col-lg-10 col-md-10 col-sm-10">
                                        <div>
                                            <h4 class="module-title margin-left-15">
                                                <asp:Label runat="server" Text="Tickets Abiertos (" />
                                                <asp:Label runat="server" ID="lblTicketAbiertosHeader"/><asp:Label runat="server" Text=" )" />
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-md-2 col-sm-2 text-right">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success" style="margin-top: 20px; margin-right: 35px" OnClick="OnClick">
                                            <i class="fa fa-plus"></i>  Nuevo Ticket                                            
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="form-group margin-left-15">
                                        <asp:Label runat="server" class="col-sm-1 control-label" Text="Grupo" />
                                        <div class="col-sm-4">
                                            <asp:DropDownList runat="server" ID="ddlGrupo" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlGrupo_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="form-group margin-left-15">
                                        <asp:Label runat="server" class="col-sm-1 control-label" Text="Agente" />
                                        <div class="col-sm-4">
                                            <asp:DropDownList runat="server" ID="ddlAgente" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlAgente_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row" style="width:70%; margin-top:18px">
                                   
                                        <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-long-arrow-down margin-right-10 margin-left-15" Text="" ID="btnAutoasignar" OnClick="btnAutoasignar_OnClick">
                                            <span style="font-family: 'Proxima Nova RG';"> Asignármelo</span>
                                        </asp:LinkButton>
                                    
                                        <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-long-arrow-right margin-right-10" Text="" ID="btnAsignar" OnClick="btnAsignar_OnClick">
                                             <span style="font-family: 'Proxima Nova RG';"> Asignar</span>
                                        </asp:LinkButton>                                                                     
                                    
                                        <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-long-arrow-right margin-right-10" Text="" ID="btnCambiarEstatus" OnClick="btnCambiarEstatus_OnClick">
                                             <span style="font-family: 'Proxima Nova RG';"> Cambiar Estatus</span>
                                        </asp:LinkButton>
                                   
                                        <asp:LinkButton runat="server" CssClass="btn btn-primary fa fa-refresh margin-right-10" ID="btnRefresh" OnClick="btnRefresh_OnClick">
                                             <%--<span style="font-family: 'Proxima Nova RG';"> </span>--%>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CssClass="btn fa fa-long-arrow-right" Text="Escalar" ID="btnEscalar" OnClick="btnEscalar_OnClick" Visible="False"/>                                   
                                    </div>

                                <tc:RadGrid runat="server" ID="gvTickets" CssClass="table table-striped display margin-top-10"
                                    OnNeedDataSource="gvTickets_OnNeedDataSource" AllowFilteringByColumn="True" OnItemCommand="gvTickets_OnItemCommand"
                                    PageSize="10" PagerStyle-PageButtonCount="10" OnItemCreated="gvTickets_OnItemCreated"
                                    AllowPaging="True" AllowSorting="true" ShowGroupPanel="False" RenderMode="Classic">
                                    <GroupingSettings ShowUnGroupButton="False" CaseSensitive="False" />

                                    <ExportSettings ExportOnlyData="true" IgnorePaging="true"></ExportSettings>
                                    <MasterTableView AutoGenerateColumns="False" TableLayout="Fixed" ShowHeadersWhenNoRecords="True" CommandItemDisplay="None" DataKeyNames="NumeroTicket" NoDetailRecordsText="No hay Registros">

                                        <CommandItemSettings ShowAddNewRecordButton="False"  ShowRefreshButton="False" ></CommandItemSettings>
                                        <Columns>
                                            <tc:GridClientSelectColumn UniqueName="chkSelected">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </tc:GridClientSelectColumn>

                                            <tc:GridImageColumn DataImageUrlFields="ImagenSla" HeaderText="SLA" SortExpression="ImagenSla" ShowFilterIcon="False" AllowFiltering="False" UniqueName="Sla" ImageWidth="30px">
                                                <HeaderStyle Width="60px"></HeaderStyle>
                                            </tc:GridImageColumn>

                                            <tc:GridImageColumn DataImageUrlFields="ImagenPrioridad" HeaderText="Pri" SortExpression="ImagenPrioridad" AutoPostBackOnFilter="True" ShowFilterIcon="False" AllowFiltering="False" UniqueName="ImagenPrioridad" ImageWidth="30px">
                                                <HeaderStyle Width="60px"></HeaderStyle>
                                            </tc:GridImageColumn>

                                            <tc:GridTemplateColumn UniqueName="Vip" HeaderText="VIP" ShowFilterIcon="False" AllowFiltering="False">
                                                <HeaderStyle Width="60px"></HeaderStyle>

                                                <ItemTemplate>
                                                    <asp:Image runat="server" ImageUrl="~/assets/images/icons/vip.png" Width="30px" Visible='<%# Eval("Vip") %>' />
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>

                                            <tc:GridTemplateColumn UniqueName="TipoUsuario" DataField="UsuarioSolicito.TipoUsuario.Descripcion" HeaderText="TU" AllowFiltering="True" ShowFilterIcon="False">
                                                <HeaderStyle Width="60px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <div class="btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("UsuarioSolicito.TipoUsuario.Color") + " !important" %>'>
                                                        <%# Eval("UsuarioSolicito.TipoUsuario.Descripcion").ToString().Substring(0,1) %>
                                                    </div>
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>

                                            <tc:GridTemplateColumn DataField="NumeroTicket" HeaderText="Ticket" SortExpression="NumeroTicket" ShowFilterIcon="False"
                                                UniqueName="NumeroTicket" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("NumeroTicket") %>' ID="lbntIdticket" OnClick="lbntIdticket_OnClick" CommandArgument='<%#Eval("IdTicket") %>' Width="8%" />
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>

                                            <tc:GridBoundColumn DataField="Canal" HeaderText="Canal" SortExpression="Canal" UniqueName="Canal" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                            </tc:GridBoundColumn>

                                            <tc:GridTemplateColumn DataField="UsuarioSolicito.NombreCompleto" HeaderText="Solicitante" SortExpression="UsuarioSolicito.NombreCompleto" UniqueName="Solicitante"
                                                CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("UsuarioSolicito.NombreCompleto").ToString(). Length > 12 ? Eval("UsuarioSolicito.NombreCompleto").ToString().Substring(0, 12) : Eval("UsuarioSolicito.NombreCompleto") %>' ID="btnUsuarioSolicito" OnClick="btnUsuario_OnClick" CommandArgument='<%#Eval("IdUsuario") %>' Width="10.6%" />
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>

                                            <tc:GridTemplateColumn DataField="Tipificacion" HeaderText="Asunto" SortExpression="NumeroTicket" UniqueName="Tipificacion" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("Tipificacion").ToString(). Length > 12 ? Eval("Tipificacion").ToString().Substring(0, 12) : Eval("Tipificacion") %>' ID="btnTipificacion" OnClick="btnTipificacion_OnClick" CommandArgument='<%#Eval("IdTicket") %>' Width="10.6%" />
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>

                                            <tc:GridBoundColumn DataField="TipoTicketAbreviacion" HeaderText="Tipo" SortExpression="TipoTicketAbreviacion" UniqueName="Tipo" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                            </tc:GridBoundColumn>

                                            <tc:GridBoundColumn DataField="FechaHora" DataFormatString="{0:MM/dd/yy HH:mm}" HeaderText="Solicitado" SortExpression="FechaHora" UniqueName="Solicitado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                            </tc:GridBoundColumn>
                                            <tc:GridBoundColumn DataField="EstatusTicket.Descripcion" HeaderText="Estatus" SortExpression="EstatusTicket.Descripcion" UniqueName="EstatusTicket" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                            </tc:GridBoundColumn>
                                            <tc:GridTemplateColumn DataField="UsuarioAsignado" HeaderText="Asignado a" SortExpression="UsuarioAsignado" UniqueName="UsuarioAsignaco" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" CssClass="col-xs-1" Text='<%#Eval("UsuarioAsignado").ToString(). Length > 12 ? Eval("UsuarioAsignado").ToString().Substring(0, 12) : Eval("UsuarioAsignado") %>' ID="btnUsuarioAsignado" OnClick="btnUsuario_OnClick" CommandArgument='<%#Eval("IdUsuarioAsignado") %>' Width="10.6%" />
                                                </ItemTemplate>
                                            </tc:GridTemplateColumn>
                                            <tc:GridBoundColumn DataField="GrupoAsignado" HeaderText="Grupo Asignado" SortExpression="GrupoAsignado" UniqueName="GrupoAsignado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
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
                                        <Selecting AllowRowSelect="True"></Selecting>
                                        <Resizing AllowResizeToFit="True"></Resizing>
                                    </ClientSettings>
                                </tc:RadGrid>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-5 col-sm-5 no-padding-left">
                            <div class="module-inner">
                                <div class="row borderbootom">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <tc:RadButton runat="server" CssClass="btn col-sm-12 col-md-12 col-lg-12" ButtonType="StandardButton" EnableEmbeddedBaseStylesheet="True" Text="Abierto" ID="btnFiltroAbierto" EnableEmbeddedSkins="False"
                                            CommandArgument="Abierto" Style="text-align: left" OnClick="btnFiltro_OnClick">
                                            <ContentTemplate>
                                                <asp:Label class="col-sm-10 col-md-10 col-lg-10" Font-Bold="true" runat="server" Text="Tickets Abiertos"/>
                                                <asp:Label class="col-sm-2 col-md-2 col-lg-2" Font-Bold="true" runat="server" ID="lblTicketsAbiertos" Style="text-align: right">0</asp:Label>
                                            </ContentTemplate>
                                        </tc:RadButton>
                                    </div>
                                </div>
                                <div class="row borderbootom">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <tc:RadButton runat="server" Text="Tickets Sin Asignar" ID="btnFiltroSinAsignar" CommandArgument="SinAsignar" CssClass="btn col-sm-12 col-md-12 col-lg-12" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                            <ContentTemplate>
                                                <asp:Label class="col-sm-10 col-md-10 col-lg-10" runat="server" Text="Tickets Sin Asignar"/>
                                                <asp:Label class="col-sm-2 col-md-2 col-lg-2" runat="server" ID="lblTicketsSinAsignar" Style="text-align: right">0</asp:Label>
                                            </ContentTemplate>
                                        </tc:RadButton>
                                    </div>
                                </div>
                                <div class="row borderbootom">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <tc:RadButton runat="server" Text="En Espera" ID="btnFiltroEspera" CommandArgument="Espera" CssClass="btn col-sm-12 col-md-12 col-lg-12" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                            <ContentTemplate>
                                                <asp:Label class="col-sm-10 col-md-10 col-lg-10" Font-Bold="true" runat="server" Text="Tickets Pendientes"/>
                                                <asp:Label class="col-sm-2 col-md-2 col-lg-2" Font-Bold="true" runat="server" ID="lblTicketsEspera" Style="text-align: right">2</asp:Label>
                                            </ContentTemplate>
                                        </tc:RadButton>
                                    </div>
                                </div>
                                
                                <div class="row borderbootom">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <tc:RadButton runat="server" Text="Recien Resuelto" ID="btnFiltroResuelto" CommandArgument="Resuelto" CssClass="btn col-sm-12 col-md-12 col-lg-12" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                            <ContentTemplate>
                                                <asp:Label class="col-sm-10 col-md-10 col-lg-10" Font-Bold="true" runat="server"  Text="Tickets Recién Resueltos (36hrs)"/>
                                                <asp:Label class="col-sm-2 col-md-2 col-lg-2" Font-Bold="true" runat="server" ID="lblTicketsResueltos" Style="text-align: right">0</asp:Label>
                                            </ContentTemplate>
                                        </tc:RadButton>
                                    </div>
                                </div>
                                <div class="row borderbootom">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <tc:RadButton runat="server" Text="Fuera SLA" ID="btnFueraSla" CommandArgument="FueraSla" CssClass="btn col-sm-12 col-md-12 col-lg-12" Style="text-align: left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                            <ContentTemplate>
                                                <asp:Label class="col-sm-10 col-md-10 col-lg-10" runat="server" Text="Tickets Fuera de Sla"/>
                                                <asp:Label class="col-sm-2 col-md-2 col-lg-2" runat="server" ID="lblTicketsFueraSla" Style="text-align: right">0</asp:Label>
                                            </ContentTemplate>
                                        </tc:RadButton>
                                    </div>
                                </div>
                            </div>
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
                        <div class="modal-body">
                            <uc1:UcAsignacionUsuario runat="server" ID="ucAsignacionUsuario" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
