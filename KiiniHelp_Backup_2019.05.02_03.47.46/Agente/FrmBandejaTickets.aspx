<%@ Page Title="" Language="C#" MasterPageFile="~/Agente.Master" AutoEventWireup="true" CodeBehind="FrmBandejaTickets.aspx.cs" Inherits="KiiniHelp.Agente.FrmBandejaTickets" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusAsignacion.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusAsignacion" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%">
            <ContentTemplate>

                <section class="module">
                    <div class="row">
                        <div class="col-lg-10 col-md-8 col-sm-8 no-padding-right borderright">
                            <div class="module-inner no-padding-right">
                                <div class="row">
                                    <div class="col-lg-8 col-md-9 col-sm-9">
                                        <div class="module-heading">
                                            <h3 class="module-title">
                                                <asp:Label runat="server" Text="tickets Abiertos (" />
                                                <asp:Label runat="server" ID="lblTicketAbiertosHeader"></asp:Label><asp:Label runat="server" Text=" )" />
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-3 col-sm-3">
                                        <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Nuevo Ticket" OnClick="OnClick" />
                                    </div>
                                </div>
                                <div class="row margin-top-5">
                                    <div class="col-lg-5 col-md-6 col-sm-12">
                                        <div class="form-group">
                                            <asp:DropDownList runat="server" ID="ddlGrupo" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlGrupo_OnSelectedIndexChanged" />
                                        </div>
                                        <div class="form-group">
                                            <asp:DropDownList runat="server" ID="ddlAgente" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlAgente_OnSelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row margin-top-5">
                                    <div class="col-lg-1 col-md-3 col-sm-3">
                                        <asp:LinkButton runat="server" CssClass="btn fa fa-long-arrow-down" Text="Asignarmelo" ID="btnAutoasignar" OnClick="btnAutoasignar_OnClick" />
                                    </div>
                                    <div class="col-lg-1 col-md-2 col-sm-2">
                                        <asp:LinkButton runat="server" CssClass="btn fa fa-long-arrow-right" Text="Asignar" ID="btnAsignar" OnClick="btnAsignar_OnClick" />
                                    </div>
                                    <div class="col-lg-1 col-md-2 col-sm-2">
                                        <asp:LinkButton runat="server" CssClass="btn fa fa-long-arrow-right" Text="Asignar" ID="Escalar" OnClick="Escalar_OnClick" />
                                    </div>
                                    <div class="col-lg-1 col-md-2 col-sm-2">
                                        <asp:LinkButton runat="server" CssClass="btn fa fa-refresh" ID="btnRefresh" OnClick="btnRefresh_OnClick" />
                                    </div>
                                </div>

                                <div class="row margin-top-5">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        
                                        <asp:GridView runat="server" CssClass="table table-striped display" ID="gvTickets" DataKeyNames="NumeroTicket" AllowSorting="True" AutoGenerateColumns="False" GridLines="None"
                                            OnRowCommand="gvTickets_OnRowCommand" OnSorting="gvTickets_OnSorting">
                                            <EmptyDataTemplate>
                                                !No hay tickets Abiertos!
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkboxSelectAll" runat="server" OnCheckedChanged="chkboxSelectAll_OnCheckedChanged" AutoPostBack="True" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelected" runat="server" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" SortExpression="ImagenSla">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="SLA" CommandArgument="sla" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgSla" runat="server" ImageUrl='<%#Eval("ImagenSla") %>' Width="30px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" SortExpression="ImagenPrioridad">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Pri" CommandArgument="pri" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("ImagenPrioridad") %>' Width="30px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" SortExpression="Vip">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="VIP" CommandArgument="vip" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ImageUrl="~/assets/images/icons/vip.png" Width="30px" Visible='<%# Eval("Vip") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Left" SortExpression="TipoUsuario">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="TU" CommandArgument="tu" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                    <ItemTemplate>
                                                        <%--<button type="button" class='<%# 
int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Empleado || int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado : 
int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Cliente || int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado  : 
int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Proveedor || int.Parse(Eval("UsuarioSolicito.TipoUsuario.Id").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado 
                                                        %>'>--%>
                                                        <%# Eval("UsuarioSolicito.TipoUsuario.Descripcion").ToString().Substring(0,1) %></button>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NumeroTicket">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Ticket" CommandArgument="NumeroTicket" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text='<%#Eval("NumeroTicket") %>' CommandArgument='<%#Eval("NumeroTicket") %>' CommandName="redirect"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Canal" CommandArgument="Canal" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("Canal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Solicitante" CommandArgument="UsuarioSolicito" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("UsuarioSolicito.NombreCompleto") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Tipificacion" CommandArgument="Asunto" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("NumeroTicket") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Tipo" CommandArgument="TipoTicket" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("TipoTicketAbreviacion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Solicitado" CommandArgument="FechaHora" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("FechaHora", "{0:MM/dd/yy HH:mm}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Estatus" CommandArgument="EstatusTicket" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("EstatusTicket.Descripcion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Asignación" CommandArgument="EstatusAsignacion" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("EstatusAsignacion.Descripcion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Asignado a" CommandArgument="UsuarioAsignado" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("UsuarioAsignado") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:LinkButton runat="server" Text="Grupo" CommandArgument="GrupoAsignado" CommandName="sort" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("GrupoAsignado") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="EsPropietario" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("EsPropietario") %>' ID="lblEsPropieatrio"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="IdGrupoAsignado" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("IdGrupoAsignado") %>' ID="lblIdGrupoAsignado"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="False">
                                                    <HeaderTemplate>
                                                        <asp:Label runat="server" Text="EstatusAsignacionActual" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%#Eval("EstatusAsignacion.Id") %>' ID="lblEstatusAsignacionActual"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-4 col-sm-4 no-padding-left">
                            <div class="module-inner">
                                <div class="row borderbootom" style="margin-top: 10px">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <asp:Label class="col-lg-10" runat="server" Text="Tickets Abiertos"></asp:Label>
                                        <asp:Label runat="server" ID="lblTicketsAbiertos">2</asp:Label>
                                    </div>
                                </div>
                                <div class="row borderbootom" style="margin-top: 10px">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <asp:Label class="col-lg-10" runat="server" Text="Tickets sin asignar"></asp:Label>
                                        <asp:Label runat="server" ID="lblTicketsSinAsignar">2</asp:Label>
                                    </div>
                                </div>
                                <div class="row borderbootom" style="margin-top: 10px">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <asp:Label class="col-lg-10" runat="server" Text="Tickets pendientes"></asp:Label>
                                        <asp:Label runat="server" ID="lblTicketsPendientes">2</asp:Label>
                                    </div>
                                </div>
                                <div class="row borderbootom" style="margin-top: 10px">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <asp:Label class="col-lg-10" runat="server" Text="Tickets recien cerrados (36 hrs)"></asp:Label>
                                        <asp:Label runat="server" ID="lblTicketsRecienCerrados">2</asp:Label>
                                    </div>
                                </div>
                                <div class="row borderbootom" style="margin-top: 10px">
                                    <div class="form-group col-lg-12 no-padding-left verical-center">
                                        <asp:Label class="col-lg-10" runat="server" Text="Tickets fuera de sla"></asp:Label>
                                        <asp:Label runat="server" ID="lblTicketsFueraSla">2</asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="modal fade" id="modalAsignacionCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusAsignacion runat="server" id="ucCambiarEstatusAsignacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
