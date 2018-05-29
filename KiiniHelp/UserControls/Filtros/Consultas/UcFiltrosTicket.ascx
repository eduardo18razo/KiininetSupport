<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Consultas.UcFiltrosTicket" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupo.ascx" TagPrefix="uc1" TagName="UcFiltroGrupo" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroCanalApertura.ascx" TagPrefix="uc1" TagName="UcFiltroCanalApertura" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroServicioIncidente.ascx" TagPrefix="uc1" TagName="UcFiltroServicioIncidente" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipificacion.ascx" TagPrefix="uc1" TagName="UcFiltroTipificacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroPrioridad.ascx" TagPrefix="uc1" TagName="UcFiltroPrioridad" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEstatus.ascx" TagPrefix="uc1" TagName="UcFiltroEstatus" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroSla.ascx" TagPrefix="uc1" TagName="UcFiltroSla" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasConsultas.ascx" TagPrefix="uc1" TagName="UcFiltroFechasConsultas" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>



<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="panel-body text-center">
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroGrupo" Text="Grupo" OnClick="btnFiltroGrupo_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroCanal" Text="Canal Apertura" OnClick="btnFiltroCanal_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroTipoUsuario" Text="Tipo Usuario" OnClick="btnFiltroTipoUsuario_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroOrganizacion" Text="Organización" OnClick="btnFiltroOrganizacion_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroUbicacion" Text="Ubicación" OnClick="btnFiltroUbicacion_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroServicioIncidente" Text="Servicio/Incidente" OnClick="btnFiltroServicioIncidente_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary disabled" ID="btnFiltroTipificacion" Text="Tipificación" OnClick="btnFiltroTipificacion_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroPrioridad" Text="Prioridad" OnClick="btnFiltroPrioridad_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroEstatus" Text="Estatus" OnClick="btnFiltroEstatus_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroSla" Text="SLA" OnClick="btnFiltroSla_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroVip" Text="Vip" OnClick="btnFiltroVip_OnClick" />
                    <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroFechas" Text="Fechas" OnClick="btnFiltroFechas_OnClick" />
                    <asp:Label runat="server" Width="60px"></asp:Label>
                    <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_Click" />
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

<div class="modal fade" id="modalFiltroCanal" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroCanalApertura runat="server" ID="ucFiltroCanalApertura" />
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

<div class="modal fade" id="modalFiltroServicioIncidente" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroServicioIncidente" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroServicioIncidente runat="server" ID="ucFiltroServicioIncidente" />
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

<div class="modal fade" id="modalFiltroPrioridad" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroPrioridad" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroPrioridad runat="server" ID="ucFiltroPrioridad" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroEstatus" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroEstatus" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroEstatus runat="server" ID="ucFiltroEstatus" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroSla" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroSla" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroSla runat="server" ID="ucFiltroSla" />
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
