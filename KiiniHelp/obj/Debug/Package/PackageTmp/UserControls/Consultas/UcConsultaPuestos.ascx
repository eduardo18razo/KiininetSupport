<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaPuestos.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaPuestos" %>
<%@ Register Src="~/UserControls/Altas/UcAltaPuesto.ascx" TagPrefix="uc1" TagName="UcAltaPuesto" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>
            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Administración</li>
                <li class="breadcrumb-item active">Puestos</li>
            </ol>

            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Puestos" /></h3>
                            </div>
                            <p>
                               Los puestos representan las tareas y deberes específicos de un usuario dentro de la organización. Puedes crear puestos para facilitar la administración de los usuarios y sus eventos.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner">

                        <div class="col-lg-5 col-md-5">
                            Consulta Puestos:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-3 col-md-3 separador-vertical-derecho">
                            ... o consulta por Tipo de Usuario<br />
                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" Width="190px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />

                        </div>

                        <div class="col-lg-4 col-md-4 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Nuevo" OnClick="btnNew_OnClick" />
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

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="15"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display">
                                    <Columns>
                                        <asp:TemplateField HeaderText="TU" ControlStyle-Width="10%">
                                            <ItemTemplate>
                                                <div style="min-height: 30px;">
                                                    <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                        <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Puesto" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <label><%# Eval("Descripcion")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEdit">
                                                    <li>
                                                        <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <%--<asp:Repeater runat="server" ID="rptResultados">
                                    <HeaderTemplate>
                                        <table class="table table-striped display" id="tblResults">
                                            <thead>
                                                <tr>
                                                    <th style="width: 20%">TU</th>
                                                    <th style="width: 50%">Puesto</th>
                                                    <th style="width: 15%">Activo</th>
                                                    <th style="width: 15%">Editar</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div style="min-height: 30px;">
                                                    <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                        <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                </div>
                                            </td>
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
                                                        <asp:ImageButton runat="server" ImageUrl="~/assets/images/icons/editar.png" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" />
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <script type="text/javascript">
                $(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {
                    hidden('#' + "<%=tblResults.ClientID %>");
                });

            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="modalAltaPuesto" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true" class="modal fade">
        <asp:UpdatePanel ID="upAltaPuesto" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <uc1:UcAltaPuesto runat="server" ID="ucAltaPuesto" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
