<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosTicketSla.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Graficos.Eficiencia.UcFiltrosTicketSla" %>

<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEstatus.ascx" TagPrefix="uc" TagName="UcFiltroEstatus" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc" TagName="UcFiltroFechasGrafico" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroCategoria.ascx" TagPrefix="uc" TagName="UcFiltroCategoria" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupo.ascx" TagPrefix="uc" TagName="UcFiltroGrupo" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroCanalApertura.ascx" TagPrefix="uc" TagName="UcFiltroCanalApertura" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroSla.ascx" TagPrefix="uc" TagName="UcFiltroSla" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroAtendedores.ascx" TagPrefix="uc" TagName="UcFiltroAtendedores" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroServicioIncidente.ascx" TagPrefix="uc" TagName="UcFiltroServicioIncidente" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEstatusAsignacion.ascx" TagPrefix="uc" TagName="UcFiltroEstatusAsignacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipificacion.ascx" TagPrefix="uc" TagName="UcFiltroTipificacion" %>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
            <div class="col-lg-11">
                <uc:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
            </div>

            <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                <div class="row">
                    <div class="form-group">
                        <label></label>

                    </div>
                </div>
                <div class="row">
                    <div class="form-group">
                        <label></label>
                        <asp:Button runat="server" ID="btnGraficar" Text="Graficar" CssClass="btn btn-success" OnClick="btnGraficar_OnClick" />
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc:UcFiltroSla runat="server" ID="ucFiltroSla" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroTipoUsuario runat="server" ID="ucFiltroTipoUsuario" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroCategoria runat="server" ID="ucFiltroCategoria" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroGrupo runat="server" ID="ucFiltroGrupo" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroAtendedores runat="server" ID="ucFiltroAtendedores" />
                    </div>
                    
                    <div class="col-lg-1">
                        <uc:UcFiltroEstatusAsignacion runat="server" id="ucFiltroEstatusAsignacion" />
                    </div>

                    <div class="col-lg-1">
                        <uc:UcFiltroCanalApertura runat="server" ID="ucFiltroCanalApertura" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroServicioIncidente runat="server" ID="ucFiltroServicioIncidente" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroEstatus runat="server" ID="ucFiltroEstatus" />
                        
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroOrganizacion runat="server" ID="ucFiltroOrganizacion" />
                    </div>
                    <div class="col-lg-1">
                        <uc:UcFiltroUbicacion runat="server" ID="ucFiltroUbicacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </ContentTemplate>
</asp:UpdatePanel>
