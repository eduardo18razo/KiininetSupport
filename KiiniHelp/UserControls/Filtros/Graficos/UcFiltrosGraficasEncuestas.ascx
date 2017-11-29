<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFiltrosGraficasEncuestas.ascx.cs" Inherits="KiiniHelp.UserControls.Filtros.Graficos.UcFiltrosGraficasEncuestas" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroGrupoEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroGrupoEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroOrganizacion.ascx" TagPrefix="uc1" TagName="UcFiltroOrganizacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroUbicacion.ascx" TagPrefix="uc1" TagName="UcFiltroUbicacion" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroPrioridad.ascx" TagPrefix="uc1" TagName="UcFiltroPrioridad" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroSla.ascx" TagPrefix="uc1" TagName="UcFiltroSla" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroTipoUsuario.ascx" TagPrefix="uc1" TagName="UcFiltroTipoUsuario" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroVip.ascx" TagPrefix="uc1" TagName="UcFiltroVip" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroServicioIncidenteEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroServicioIncidenteEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroResponsablesEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroResponsablesEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroEncuesta.ascx" TagPrefix="uc1" TagName="UcFiltroEncuesta" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroAtendedores.ascx" TagPrefix="uc1" TagName="UcFiltroAtendedores" %>
<%@ Register Src="~/UserControls/Filtros/Componentes/UcFiltroFechasGrafico.ascx" TagPrefix="uc1" TagName="UcFiltroFechasGrafico" %>


<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <header class="modal-header" id="panelAlerta" runat="server" visible="false">
                    <div class="alert alert-danger">
                        <div>
                            <div style="float: left">
                                <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                            </div>
                            <div style="float: left">
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
                   <%-- <div class="panel-heading">
                        Filtros
                    </div>--%>
                    <div class="panel-body text-center">
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroGrupo" Text="Grupo" OnClick="btnFiltroGrupo_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroServicioIncidente" Text="Tipo Servicio" OnClick="btnFiltroServicioIncidente_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroResponsables" Text="Responsables" OnClick="btnFiltroResponsables_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroEncuestas" Text="Encuestas" OnClick="btnFiltroEncuestas_OnClick"/>
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroAtendedores" Text="Agentes" OnClick="btnFiltroAtendedores_OnClick"/>
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroFechas" Text="Fechas" OnClick="btnFiltroFechas_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroTipoUsuario" Text="Tipo Usuario" OnClick="btnFiltroTipoUsuario_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroPrioridad" Text="Prioridad" OnClick="btnFiltroPrioridad_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroSla" Text="SLA" OnClick="btnFiltroSla_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroUbicacion" Text="Ubicación" OnClick="btnFiltroUbicacion_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroOrganizacion" Text="Organización" OnClick="btnFiltroOrganizacion_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" ID="btnFiltroVip" Text="Vip" OnClick="btnFiltroVip_OnClick" />                        
                        <asp:Label runat="server" Width="120px"></asp:Label>
                        <asp:Button runat="server" CssClass="btn btn-success" ID="btnGraficar" Text="Graficar" OnClick="btnGraficar_Click" />
                    </div>
                    <%--<div class="panel-footer">
                    </div>--%>
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
                    <uc1:UcFiltroGrupoEncuesta runat="server" id="ucFiltroGrupoEncuesta" />
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
                    <uc1:UcFiltroServicioIncidenteEncuesta runat="server" id="ucFiltroServicioIncidenteEncuesta" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroResponsable" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroResponsablesEncuesta runat="server" id="ucFiltroResponsablesEncuesta" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroEncuestas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroEncuesta runat="server" id="ucFiltroEncuesta" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroAtendedores" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroAtendedores runat="server" id="ucFiltroAtendedores" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="modal fade" id="modalFiltroFechas" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroFechas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg" style="width: 450px">
                <div class="modal-content" style="width: 450px">
                    <uc1:UcFiltroFechasGrafico runat="server" ID="ucFiltroFechasGrafico" />
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
                    <uc1:UcFiltroTipoUsuario runat="server" id="ucFiltroTipoUsuario" />
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

<div class="modal fade" id="modalFiltroVip" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upFiltroVip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <uc1:UcFiltroVip runat="server" id="ucFiltroVip" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>