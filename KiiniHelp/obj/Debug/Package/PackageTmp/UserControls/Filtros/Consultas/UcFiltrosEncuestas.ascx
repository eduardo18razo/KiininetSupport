<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosEncuestas.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Consultas.UcFiltrosEncuestas" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupoEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroGrupoEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroPrioridad.ascx" TagPrefix="uc1" TagName="UcFiltroPrioridad" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroSla.ascx" TagPrefix="uc1" TagName="UcFiltroSla" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasConsultas.ascx" TagPrefix="uc1" TagName="UcFiltroFechasConsultas" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroServicioIncidenteEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroServicioIncidenteEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroResponsablesEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroResponsablesEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroAtendedores.ascx" TagPrefix="uc1" TagName="UcFiltroAtendedores" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-lg-11">
                        <uc1:UcFiltroFechasConsultas runat="server" ID="ucFiltroFechasConsultas" />
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
                                <asp:Button runat="server" CssClass="btn btn-success" ID="btnConsultar" Text="Consultar" OnClick="btnConsultar_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroGrupoEncuesta runat="server" ID="ucFiltroGrupoEncuesta" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroServicioIncidenteEncuesta runat="server" ID="ucFiltroServicioIncidenteEncuesta" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroResponsablesEncuesta runat="server" ID="ucFiltroResponsablesEncuesta" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroEncuesta runat="server" ID="ucFiltroEncuesta" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroAtendedores runat="server" ID="ucFiltroAtendedores" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroTipoUsuario runat="server" ID="ucFiltroTipoUsuario" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroPrioridad runat="server" ID="ucFiltroPrioridad" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroSla runat="server" ID="ucFiltroSla" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroUbicacion runat="server" ID="ucFiltroUbicacion" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroOrganizacion runat="server" ID="ucFiltroOrganizacion" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>


