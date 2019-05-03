<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosGraficasHits.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Graficos.UcFiltrosGraficasHits" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupo.ascx" TagPrefix="uc1" TagName="UcFiltroGrupo" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipificacion.ascx" TagPrefix="uc1" TagName="UcFiltroTipificacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="panel panel-primary">
                    <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
                    </div>
                    <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                        <div class="col-lg-1">
                            <uc1:UcFiltroGrupo runat="server" ID="ucFiltroGrupo" />
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
                            <uc1:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
                        </div>
                        <div class="col-lg-1">
                            <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
                        </div>
                    </div>

                    <div class="panel-body text-center">
                        <asp:Button runat="server" CssClass="btn btn-success" ID="btnGraficar" Text="Graficar" OnClick="btnGraficar_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>

