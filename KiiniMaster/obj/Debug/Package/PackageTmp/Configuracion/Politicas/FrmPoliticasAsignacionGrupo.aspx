<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FrmPoliticasAsignacionGrupo.aspx.cs" Inherits="KiiniMaster.Configuracion.Politicas.FrmPoliticasAsignacionGrupo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height: 100%;">
        <asp:UpdatePanel runat="server" style="height: 100%" ID="upGeneral">
            <ContentTemplate>
                <br>
                <h3 class="h6">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
                    / Politica de Asignacion Default </h3>
                <hr />
                <section class="module">
                    <div class="row">
                        <div class="col-lg-12 col-md-6">
                            <div class="module-inner">
                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Grupo Usuario"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlGrupoUsuario" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlGrupoUsuario_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="SubRol"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlSubRol" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSubRol_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-horizontal col-lg-12">
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Estatus Actual"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlEstatusActual" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlEstatusActual_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group col-lg-4">
                                        <asp:Label runat="server" CssClass="col-lg-4" Text="Estatus Accion"></asp:Label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList runat="server" ID="ddlEstatusAccion" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlEstatusAccion_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <br />
                               <%-- <div class="form-horizontal col-lg-12">
                                    <div class="form-group">
                                        CONSULTA ESTATUS ASIGNACION:<br />
                                        <div class="search-box form-inline margin-bottom-lg">
                                            <label class="sr-only" for="txtFiltro">Buscar</label>
                                            <div class="form-group">
                                                <asp:TextBox runat="server" ID="txtFiltroDecripcion" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                                <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon fa fa-search" OnClick="btnBuscar_OnClick" />
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>




                            </div>
                        </div>
                    </div>
                </section>

                <section class="module module-headings">
                    <div class="module-inner">
                        <div class="module-content collapse in" id="content-1">
                            <div class="module-content-inner no-padding-bottom">
                                <div class="table-responsive">
                                    <asp:Repeater ID="rptResultados" runat="server" >
                                        <HeaderTemplate>
                                            <table class="table table-striped display" id="tblResults">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            <asp:Label runat="server">Id</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">GrupoUsuario</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">Rol</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">SubRol</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">EstatusAsignacionActual</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">EstatusAsignacionAccion</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">ComentarioObligado</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">TieneSupervisor</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">Propietario</asp:Label></th>
                                                        <th>
                                                            <asp:Label runat="server">Habilitado</asp:Label></th>

                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>

                                                <td><%# Eval("Id")%></td>
                                                <td><%# Eval("GrupoUsuario.Descripcion")%></td>
                                                <td><%# Eval("Rol.Descripcion")%></td>
                                                <td><%# Eval("SubRol.Descripcion")%></td>
                                                <td><%# Eval("EstatusAsignacionActual.Descripcion")%></td>
                                                <td><%# Eval("EstatusAsignacionAccion.Descripcion")%></td>
                                                <td><%# Eval("ComentarioObligado")%></td>
                                                <td><%# Eval("TieneSupervisor")%></td>
                                                <td><%# Eval("Propietario")%></td>

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
                                                            <asp:Button runat="server" CssClass="btn btn-sm btn-primary" Text="Editar" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" Visible="false" />
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
        <script type="text/javascript">
            $(function () {
                hidden();
            });
        </script>
    </div>
</asp:Content>
