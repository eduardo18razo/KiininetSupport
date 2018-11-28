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
                            <uc1:UcFiltroCanalApertura runat="server" ID="ucFiltroCanalApertura" />
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
                            <uc1:UcFiltroServicioIncidente runat="server" ID="ucFiltroServicioIncidente" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                            <uc1:UcFiltroTipificacion runat="server" ID="ucFiltroTipificacion" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                            <uc1:UcFiltroPrioridad runat="server" ID="ucFiltroPrioridad" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                            <uc1:UcFiltroEstatus runat="server" ID="ucFiltroEstatus" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                            <uc1:UcFiltroSla runat="server" ID="ucFiltroSla" />
                        </div>
                        <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1">
                            <uc1:UcFiltroVip runat="server" ID="ucFiltroVip" />
                        </div>
                    </div>
                    <asp:Label runat="server" Width="60px"></asp:Label>
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
