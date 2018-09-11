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
                        <uc1:UcFiltroGrupo runat="server" ID="ucFiltroGrupo" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroTipoUsuario runat="server" ID="ucFiltroTipoUsuario" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroOrganizacion runat="server" ID="ucFiltroOrganizacion" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroUbicacion runat="server" ID="ucFiltroUbicacion" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
                    </div>
                    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                        <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
