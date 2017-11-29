<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaAreas.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaAreas" %>
<%@ Register Src="~/UserControls/Altas/AltaArea.ascx" TagPrefix="uc1" TagName="AltaArea" %>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <br>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                / Cátegorias </h3>
            <hr />
            <section class="module">
                <div class="row">
                    <div class="col-lg-8 col-md-9">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title"><asp:Label runat="server" ID="lblBranding"></asp:Label></h3>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="module-inner">
                            <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Crear Nueva Area" OnClick="btnNew_OnClick"/>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-lg-6 col-md-6">
                        <div class="module-inner">
                            CONSULTA CATEGORIAS:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon fa fa-search" OnClick="btnBuscar_OnClick"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section class="module module-headings">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">
                                <asp:Repeater runat="server" ID="rptResultados">
                                    <HeaderTemplate>
                                        <table class="table table-striped display" id="tblResults">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <asp:Label runat="server">Descripción</asp:Label></th>
                                                    <th>
                                                        <asp:Label runat="server">Habilitado</asp:Label></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Descripcion")%></td>
                                            <td id="colHabilitado">
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </td>
                                            <td id="colEditar">
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                        <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Editar" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--MODAL ALTA--%>
    <div class="modal fade" id="modalAltaArea" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" style="margin-top: 60px">
        <asp:UpdatePanel ID="upAltaArea" runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:AltaArea runat="server" id="ucAltaArea" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

