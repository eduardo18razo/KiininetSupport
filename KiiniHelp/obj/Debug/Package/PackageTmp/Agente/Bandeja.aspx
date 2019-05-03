<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="Bandeja.aspx.cs" Inherits="KiiniHelp.Agente.Bandeja" %>


<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusAsignacion.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusAsignacion" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleTicket.ascx" TagPrefix="uc1" TagName="UcDetalleTicket" %>
<%@ Register Src="~/UserControls/Agentes/UcAsignacionUsuario.ascx" TagPrefix="uc1" TagName="UcAsignacionUsuario" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function seleccion(sender, eventArgs) {
            var hfComentario = document.getElementById('<%= hfFilaSeleccionada.ClientID%>');
               hfComentario.value = true;
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="heigth100">
        <asp:UpdatePanel runat="server" class="heigth100">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfTicketActivo" />
                <asp:HiddenField runat="server" ID="hfFiltroTodos" Value="false" />
                <asp:HiddenField runat="server" ID="hfFiltroAbierto" Value="false" />
                <asp:HiddenField runat="server" ID="fhFiltroSinAsignar" Value="false" />
                <asp:HiddenField runat="server" ID="hfFiltroPrendientes" Value="false" />
                <asp:HiddenField runat="server" ID="hfFiltroResueltos" Value="false" />
                <asp:HiddenField runat="server" ID="hfFiltroFueraSla" Value="false" />
                <asp:HiddenField runat="server" ID="hfFiltroRecienActualizados" Value="false" />
                <asp:HiddenField runat="server" ID="hfFilaSeleccionada" Value="false"/>
                <asp:Timer runat="server" ID="tmLoadTickets" Interval="30000" OnTick="tmLoadTickets_OnTick"></asp:Timer>
                <div>
                    <div class="row">
                        <div class="col-lg-10 col-md-8 col-sm-8 no-padding-right">
                            <div class="module-inner">

                                <section class="module">
                                    <div class="module-inner">
                                        <div class="row">
                                            <div class="module-heading col-lg-12 col-md-12 col-sm-12">
                                                <div class="module-title">
                                                    <asp:Label runat="server" CssClass="TitulosAzul" ID="lblTicketAbiertosHeader" /><asp:Label runat="server" CssClass="TitulosAzul" Text=" (" /><asp:Label runat="server" CssClass="TitulosAzul" ID="lblcontadorTicketHeader" /><asp:Label runat="server" CssClass="TitulosAzul" Text=")" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row margin-top-18">
                                            <div class="col-lg-8 col-md-10 col-sm-10 col-xs-12 no-padding-left">
                                                <asp:LinkButton runat="server" CssClass="btn col-lg-2 col-md-3 col-sm-4 col-xs-12 btnManejoTickets margin-right-10 margin-bottom-10" ID="btnAutoasignar" OnClick="btnAutoasignar_OnClick">
                                                        <i class="fa fa-long-arrow-down"></i>
                                                        Asignármelo
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn col-lg-2 col-md-3 col-sm-4 col-xs-12 btnManejoTickets margin-right-10 margin-bottom-10" ID="btnAsignar" OnClick="btnAsignar_OnClick">
                                                        <i class="fa fa-long-arrow-right"></i>
                                                        Asignar
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn col-lg-2 col-md-3 col-sm-4 col-xs-12 btnManejoTickets margin-right-10 margin-bottom-10" ID="btnCambiarEstatus" OnClick="btnCambiarEstatus_OnClick" Visible="False">
                                                        <i class="fa fa-long-arrow-right"></i>
                                                        Cambiar Estatus
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn col-lg-1 col-md-1 col-sm-1 col-xs-12 btnManejoTickets margin-right-10 margin-bottom-10" ID="btnRefresh" OnClick="btnRefresh_OnClick">
                                                        <i class="fa fa-refresh"></i>
                                                </asp:LinkButton>

                                                <asp:LinkButton runat="server" CssClass="btn col-xs-12 fa fa-long-arrow-right margin-bottom-10" Text="Escalar" ID="btnEscalar" OnClick="btnEscalar_OnClick" Visible="False" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-3 col-xs-12 text-right no-padding-right">
                                                <asp:LinkButton runat="server" CssClass="btn col-sm-11 col-xs-12 btn-success" ID="btnSearchUsuario" OnClick="btnSearchUsuario_OnClick">
                                            <i class="fa fa-search"></i>  Usuarios                                          
                                                </asp:LinkButton>
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-3 col-xs-12 text-right no-padding-right">
                                                <asp:LinkButton runat="server" CssClass="btn col-sm-11 col-xs-12 btn-success" OnClick="OnClick">
                                            <i class="fa fa-plus"></i>  Nuevo Ticket                                          
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                        
                                        <tc:RadGrid runat="server" ID="gvTickets" CssClass="table table-striped display"
                                            FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                                            PagerStyle-AlwaysVisible="true" OnFilterCheckListItemsRequested="gvTickets_OnFilterCheckListItemsRequested" 
                                            OnNeedDataSource="gvTickets_OnNeedDataSource" AllowFilteringByColumn="True" OnItemCommand="gvTickets_OnItemCommand"
                                            OnSelectedIndexChanged="gvTickets_SelectedIndexChanged" PageSize="14" PagerStyle-PageButtonCount="10" OnItemCreated="gvTickets_OnItemCreated"
                                            AllowPaging="True" AllowSorting="true" ShowGroupPanel="False" RenderMode="Classic">
                                            <GroupingSettings ShowUnGroupButton="False" CaseSensitive="False" />

                                            <ExportSettings ExportOnlyData="true" IgnorePaging="true"></ExportSettings>
                                            <MasterTableView AutoGenerateColumns="False" TableLayout="Fixed" ShowHeadersWhenNoRecords="True" CommandItemDisplay="None"
                                                DataKeyNames="NumeroTicket" NoDetailRecordsText="No hay Registros" HeaderStyle-CssClass="textoTabla" HeaderStyle-Font-Bold="true"
                                                HeaderStyle-Font-Names="Proxima Nova" HeaderStyle-ForeColor="#6E6E6E"
                                                ItemStyle-Font-Names="Proxima Nova" ItemStyle-ForeColor="#6E6E6E" 
                                                AlternatingItemStyle-Font-Names="Proxima Nova" AlternatingItemStyle-ForeColor="#6E6E6E"
                                                FooterStyle-BackColor="White">

                                                <CommandItemSettings ShowAddNewRecordButton="False" ShowRefreshButton="False"></CommandItemSettings>
                                                <Columns>
                                                    <tc:GridClientSelectColumn UniqueName="chkSelected" ColumnGroupName="1" EnableHeaderContextMenu="False" >
                                                        <HeaderStyle Width="30px"></HeaderStyle>
                                                    </tc:GridClientSelectColumn>

                                                    <tc:GridTemplateColumn UniqueName="Sla" DataField="DentroSla" EnableHeaderContextMenu="False" SortExpression="Sla" ShowFilterIcon="True" AllowFiltering="True">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i class='<%# (bool)Eval("DentroSla") ? "fa fa-bomb fontGreen iconoFont" : "fa fa-bomb fontRed iconoFont" %>' ></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn EnableHeaderContextMenu="False" SortExpression="ImagenPrioridad" AutoPostBackOnFilter="True" ShowFilterIcon="False" AllowFiltering="False" UniqueName="ImagenPrioridad">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i runat="server" class="fa fa-exclamation fontRed iconoFont" visible='<%# Eval("Impacto").ToString() == "Alto"%>'></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn UniqueName="Vip" EnableHeaderContextMenu="False" ShowFilterIcon="False" AllowFiltering="False">
                                                        <HeaderStyle Width="25px"></HeaderStyle>
                                                        <ItemTemplate>
                                                            <i runat="server" class="fa fa-star iconoFont" visible='<%# Eval("Vip") %>'></i>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridTemplateColumn UniqueName="TipoUsuario" Visible="True" DataField="UsuarioSolicito.TipoUsuario.Descripcion" FilterCheckListEnableLoadOnDemand="True" HeaderText="TU" AllowFiltering="True" ShowFilterIcon="False">
                                                        <HeaderStyle Width="60px"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Middle"></ItemStyle>
                                                        <ItemTemplate>
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Background: " + Eval("UsuarioSolicito.TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("UsuarioSolicito.TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="NumeroTicket" HeaderText="Ticket" SortExpression="NumeroTicket" ShowFilterIcon="False"
                                                        UniqueName="NumeroTicket" CurrentFilterFunction="EqualTo" AutoPostBackOnFilter="True">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridTemplateColumn FilterCheckListEnableLoadOnDemand="True" DataField="UsuarioSolicito.NombreCompleto" HeaderText="Solicitante" SortExpression="UsuarioSolicito.NombreCompleto" UniqueName="Solicitante"
                                                        CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" CommandName="Save" Text='<%#Eval("UsuarioSolicito.NombreCompleto").ToString(). Length > 12 ? Eval("UsuarioSolicito.NombreCompleto").ToString().Substring(0, 12) : Eval("UsuarioSolicito.NombreCompleto") %>' ID="UsuarioSolicito" />
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="Tipificacion" HeaderText="Asunto" SortExpression="NumeroTicket" UniqueName="Tipificacion" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" Visible="false" DataField="TipoTicketAbreviacion" HeaderText="Tipo" SortExpression="TipoTicketAbreviacion" UniqueName="Tipo" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>

                                                    <tc:GridDateTimeColumn FilterCheckListEnableLoadOnDemand="True" DataField="FechaHora" DataFormatString="{0:dd/MM/yy HH:mm}" HeaderText="Solicitado" SortExpression="FechaHora" UniqueName="Solicitado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridDateTimeColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" DataField="EstatusTicket.Descripcion" HeaderText="Estatus" SortExpression="EstatusTicket.Descripcion" UniqueName="EstatusTicket" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>
                                                    
                                                    <tc:GridDateTimeColumn FilterCheckListEnableLoadOnDemand="True" DataField="FechaUltimoEvento" HeaderText="Ultimo Mov." DataFormatString="{0:dd/MM/yy HH:mm}" SortExpression="FechaUltimoEvento" UniqueName="FechaUltimoEvento" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridDateTimeColumn>
                                                    
                                                    <tc:GridTemplateColumn  FilterCheckListEnableLoadOnDemand="True" DataField="UsuarioAsignado" HeaderText="Asignado a" SortExpression="UsuarioAsignado" UniqueName="UsuarioAsignaco" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                        <ItemTemplate>
                                                            <asp:label runat="server" Text='<%#Eval("UsuarioAsignado").ToString(). Length > 12 ? Eval("UsuarioAsignado").ToString().Substring(0, 12) : Eval("UsuarioAsignado") %>'/>
                                                        </ItemTemplate>
                                                    </tc:GridTemplateColumn>

                                                    <tc:GridBoundColumn FilterCheckListEnableLoadOnDemand="True" Visible="false" DataField="GrupoAsignado" HeaderText="Grupo" SortExpression="GrupoAsignado" UniqueName="GrupoAsignado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="True" ShowFilterIcon="False">
                                                    </tc:GridBoundColumn>
                                                    
                                                    <tc:GridBoundColumn DataField="EsPropietario" HeaderText="Es propietario" Display="False" UniqueName="EsPropietario"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="IdGrupoAsignado" HeaderText="Id Grupo Asignado" Display="False" UniqueName="IdGrupoAsignado"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EstatusTicket.Id" HeaderText="Estatus Ticket" Display="False" UniqueName="IdEstatusTicket"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EstatusAsignacion.Id" HeaderText="Id Estatus Asignacion" Display="False" UniqueName="IdEstatusAsignacion"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="Asigna" HeaderText="PuedeAsignar" Display="False" UniqueName="puedeasignar"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="IdNivelAsignado" ShowFilterIcon="False" HeaderText="IdNivelAsignado" Display="False" UniqueName="IdNivelAsignado"></tc:GridBoundColumn>
                                                    

                                                    <tc:GridBoundColumn DataField="TipoTicketDescripcion" HeaderText="Tipo Ticket" Display="False" UniqueName="TipoTicketDescripcion"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="EstatusAsignacion.Descripcion" HeaderText="Estatus Asignacion" Display="False" UniqueName="EstatusAsignacionDescripcion"></tc:GridBoundColumn>
                                                    <tc:GridBoundColumn DataField="Canal" HeaderText="Canal" Display="False" UniqueName="Canal"></tc:GridBoundColumn>
                                                    

                                                </Columns>
                                            </MasterTableView>
                                            <ClientSettings EnableAlternatingItems="True" EnableRowHoverStyle="True" EnablePostBackOnRowClick="True" >
                                                <Selecting AllowRowSelect="True" UseClientSelectColumnOnly="true" ></Selecting>
                                                <ClientEvents OnRowClick="seleccion"></ClientEvents>
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
                                        <div class="module-title text-left">
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

                                    <div class="module-heading col-lg-12 col-md-12 col-sm-12 bg-blanco">
                                        <div class="module-title text-left">
                                            <asp:Label runat="server" Text="Tickets" CssClass="TitulosAzul" />
                                        </div>
                                    </div>

                                    <asp:Panel ID="FiltrosTodos" runat="server" CssClass="row borderbootom padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left"  ButtonType="StandardButton" EnableEmbeddedBaseStylesheet="True" Text="Todos"
                                                ID="rBtnFiltroTodos" EnableEmbeddedSkins="False"
                                                CommandArgument="FiltroTodos" OnClick="btnFiltro_OnClick">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 text-left" runat="server" Text="Todos" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsTodos"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosAbiertos" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Abiertos" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" ButtonType="StandardButton" EnableEmbeddedBaseStylesheet="True" ID="rBtnFiltroAbierto" EnableEmbeddedSkins="False"
                                                CommandArgument="FiltroAbierto" OnClick="btnFiltro_OnClick">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 text-left" runat="server" Text="Abiertos" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsAbiertos"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosSinAsignar" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Tickets Sin Asignar" ID="rBtnFiltroSinAsignar" CommandArgument="FiltroSinAsignar" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 text-left" runat="server" Text="Sin asignar" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsSinAsignar"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosEspera" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="En espera" ID="rBtnFiltroEspera" CommandArgument="FiltroEspera" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 text-left" runat="server" Text="Pendientes" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsEspera"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosResueltos" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Recien Resuelto" ID="rBtnFiltroResuelto" CommandArgument="FiltroResuelto" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <div class="col-sm-8 col-md-8 col-lg-8 text-left">
                                                        <asp:Label runat="server" Text="Recién resueltos (36hrs)" /><br />
                                                    </div>
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsResueltos"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosFueraSLA" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Fuera SLA" ID="rBtnFueraSla" CommandArgument="FiltroFueraSla" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <asp:Label class="col-sm-8 col-md-8 col-lg-8 text-left" runat="server" Text="Fuera de SLA" />
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsFueraSla"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>

                                    <asp:Panel ID="FiltrosRecienActualizados" runat="server" CssClass="row borderbootom padding-10-top padding-10-bottom">
                                        <div class="col-lg-12 no-padding-left verical-center">
                                            <tc:RadButton runat="server" Text="Recien actualizados" ID="rBtnRecienActualizados" CommandArgument="FiltroRecienActualizados" CssClass="btn col-sm-12 col-md-12 col-lg-12 no-padding-left no-padding-right text-left" OnClick="btnFiltro_OnClick" EnableEmbeddedSkins="False">
                                                <ContentTemplate>
                                                    <div class="col-sm-8 col-md-8 col-lg-8 text-left">
                                                        <asp:Label runat="server" Text="Recién actualizados" /><br />
                                                        <asp:Label runat="server" Text="(60min)" />
                                                    </div>
                                                    <asp:Label class="col-sm-4 col-md-4 col-lg-4 text-right" runat="server" ID="lblTicketsRecienActualizados"/>
                                                </ContentTemplate>
                                            </tc:RadButton>
                                        </div>
                                    </asp:Panel>
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
                        <uc1:UcAsignacionUsuario runat="server" ID="ucAsignacionUsuario" />
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
