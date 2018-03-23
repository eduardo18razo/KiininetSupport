<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Consultas.UcFiltrosConsulta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupo.ascx" TagPrefix="uc1" TagName="UcFiltroGrupo" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipificacion.ascx" TagPrefix="uc1" TagName="UcFiltroTipificacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasConsultas.ascx" TagPrefix="uc1" TagName="UcFiltroFechasConsultas" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfTicket" />
        <asp:HiddenField runat="server" ID="hfConsulta" />
        <asp:HiddenField runat="server" ID="hfEncuesta" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="modal-header" id="panelAlerta" runat="server" visible="false">
                    <div class="alert alert-danger">
                        <div>
                            <div class="float-left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div class="float-left">
                                <h3>Error</h3>
                            </div>
                            <div class="clearfix clear-fix" />
                        </div>
                        <hr />
                        <asp:Repeater runat="server" ID="rptError">
                            <ItemTemplate>
                                <ul>
                                    <li><%# Container.DataItem %></li>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </header>
                <div class="panel panel-primary">
                    <div class="panel-body text-center">
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroGrupo" Text="Grupo" OnClick="btnFiltroGrupo_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroTipoUsuario" Text="Tipo Usuario" OnClick="btnFiltroTipoUsuario_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroOrganizacion" Text="Organización" OnClick="btnFiltroOrganizacion_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroUbicacion" Text="Ubicación" OnClick="btnFiltroUbicacion_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroTipificacion" Text="Tipificación" OnClick="btnFiltroTipificacion_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroVip" Text="Vip" OnClick="btnFiltroVip_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroFechas" Text="Fechas" OnClick="btnFiltroFechas_OnClick" />
                        <asp:Label runat="server" Width="120px"></asp:Label>
                        <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="modal fade" id="modalFiltroGrupo" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroGpo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroGrupo runat="server" ID="ucFiltroGrupo" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroTipoUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroTipoUsuario" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroTipoUsuario runat="server" ID="ucFiltroTipoUsuario" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroOrganizacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroOrganizacion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroOrganizacion runat="server" ID="ucFiltroOrganizacion" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroUbicacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroUbicacion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroUbicacion runat="server" ID="ucFiltroUbicacion" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroTipificacion" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroTipificacion" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroVip" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroVip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroFechas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroFechas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg widht450px">
                <div class="modal-content widht450px">
                    <uc1:UcFiltroFechasConsultas runat="server" ID="ucFiltroFechasConsultas" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
