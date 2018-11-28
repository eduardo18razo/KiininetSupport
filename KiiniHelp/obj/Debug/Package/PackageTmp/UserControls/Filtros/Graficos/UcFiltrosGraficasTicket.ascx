<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosGraficasTicket.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Graficos.UcFiltrosGraficasTicket" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupo.ascx" TagPrefix="uc1" TagName="UcFiltroGrupo" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroCanalApertura.ascx" TagPrefix="uc1" TagName="UcFiltroCanalApertura" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroServicioIncidente.ascx" TagPrefix="uc1" TagName="UcFiltroServicioIncidente" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipificacion.ascx" TagPrefix="uc1" TagName="UcFiltroTipificacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroPrioridad.ascx" TagPrefix="uc1" TagName="UcFiltroPrioridad" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEstatus.ascx" TagPrefix="uc1" TagName="UcFiltroEstatus" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroSla.ascx" TagPrefix="uc1" TagName="UcFiltroSla" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
        </div>
        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="col-lg-1">
                <uc1:UcFiltroGrupo runat="server" ID="ucFiltroGrupo" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroCanalApertura runat="server" ID="ucFiltroCanalApertura" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroTipoUsuario runat="server" ID="ucFiltroTipoUsuario" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroOrganizacion runat="server" ID="ucFiltroOrganizacion" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroUbicacion runat="server" ID="ucFiltroUbicacion" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroServicioIncidente runat="server" ID="ucFiltroServicioIncidente" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroPrioridad runat="server" ID="ucFiltroPrioridad" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroEstatus runat="server" ID="ucFiltroEstatus" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroSla runat="server" ID="ucFiltroSla" />
            </div>
            <div class="col-lg-1">
                <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
            </div>
            <div class="col-lg-1">
                
            </div>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="panel panel-primary">
                    <%--<div class="panel-heading">
                        Filtros
                    </div>--%>
                    <div class="panel-body text-center">
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroFechas" Text="Fechas" OnClick="btnFiltroFechas_OnClick" />
                        <asp:Label runat="server" Width="35px"></asp:Label>
                        <asp:Button runat="server" CssClass="btn btn-success" ID="btnGraficar" Text="Graficar" OnClick="btnGraficar_Click" />
                    </div>
                    <%--<div class="panel-footer">
                    </div>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

