<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaPuestos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaPuestos" %>
<%@ Import Namespace="KinniNet.Business.Utils" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc1" TagName="UcAltaPuesto" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <br>
            <h3 class="h6">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                / Puestos </h3>
            <hr />
            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="col-lg-8 col-md-9">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblBranding" /></h3>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="module-inner">
                            <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Crear Nuevo Puesto" OnClick="btnNew_OnClick" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-6 col-md-6">
                        <div class="module-inner">
                            CONSULTA PUESTOS:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon fa fa-search" OnClick="btnBuscar_OnClick" />
                                </div>
                            </div>
                            <br />
                            ... o consulta por Tipo de Usuario<br />
                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />

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
                                                    <th>Tipo Usuario</th>
                                                    <th>Puesto</th>
                                                    <th>Activo</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <button type="button" class='<%# 
int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Empleado || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.EmpleadoInvitado || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.EmpleadoPersonaFisica ? "btn btn-default-alt btn-square-usuario empleado" : 
int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Cliente || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ClienteInvitado || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ClientaPersonaFisica ? "btn btn-default-alt btn-square-usuario cliente" : 
int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.Proveedor || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ProveedorInvitado || int.Parse(Eval("IdTipoUsuario").ToString()) == (int)BusinessVariables.EnumTiposUsuario.ProveedorPersonaFisica ? "btn btn-default-alt btn-square-usuario proveedor" : "btn btn-default-alt btn-square-usuario"
                                                        %>'>
                                                    <%# Eval("TipoUsuario.Descripcion").ToString().Substring(0,1) %></button></td>
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
    <div id="modalAltaPuesto" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" class="modal fade" style="margin-top: 60px">
        <asp:UpdatePanel ID="upAltaPuesto" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaPuesto runat="server" id="ucAltaPuesto" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
