<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaUsuarios.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaUsuarios" %>
<%@ Register TagPrefix="uc1" TagName="UcDetalleUsuario" Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server" style="height: 100%">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li class="breadcrumb-item">
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li class="breadcrumb-item">Administración</li>
                <li class="breadcrumb-item active">Usuarios</li>
            </ol>

            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Usuarios" /></h3>
                            </div>
                            <p>
                                Los usuarios son las personas que tienen un registro en tu cuenta. Aquí se incluyen los usuarios finales que son los solicitantes de eventos (clientes, proveedores y empleados) así como los operadores que son las personas designadas a resolverlos.
                            </p>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-5 col-md-5">
                            Consulta Usuarios:<br />
                            <div class="search-box form-inline margin-bottom-lg">
                                <label class="sr-only" for="txtFiltro">Buscar</label>
                                <div class="form-group">
                                    <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control help_search_form" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" placeholder="Busca con una palabra clave..." />
                                    <asp:LinkButton runat="server" class="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 separador-vertical-derecho">
                            ... o Consulta por Tipo de Usuario<br />
                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" Width="190px" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" AutoPostBack="true" />
                        </div>

                        <div class="col-lg-4 col-md-4 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Nuevo" OnClick="btnNew_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <!--/MÓDULO-->

            <div id="masonry" class="row">
                <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">

                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" 
                                            CssClass="table table-striped display" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                            BorderStyle="None" PagerSettings-Mode="Numeric" 
                                            PageSize="25" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <div style="min-height: 30px;">
                                                            <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                                <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre Completo" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("NombreCompleto")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Usuario" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("NombreUsuario")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Ult. Movimiento" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("FechaActualizacion") == null ?  Eval("FechaAlta", "{0:d}") : Eval("FechaActualizacion", "{0:d}")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Verificado" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Text='<%# (bool) Eval("Activo") ? "SI" : "NO"%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <ul class="list list-unstyled" id="hiddenEnabled">
                                                            <li>
                                                                <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# !(bool) Eval("Sistema") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
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

                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
            </div>
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
    <%--MODAL DETALLE--%>
    <div class="modal fade" id="modalDetalleUsuario" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario1" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>



