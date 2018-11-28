<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcConsultaUsuarios.ascx.cs" Inherits="KiiniHelp.UserControls.Consultas.UcConsultaUsuarios" %>
<%@ Register TagPrefix="uc1" TagName="UcDetalleUsuario" Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" %>
<div class="heigth100">
    <asp:UpdatePanel runat="server" class="heigth100">
        <ContentTemplate>

            <ol class="breadcrumb">
                <li>
                    <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
                <li>Administración</li>
                <li class="active">Usuarios</li>
            </ol>

            <!--MÓDULO-->
            <section class="module">
                <div class="row">
                    <div class="module-inner">
                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                            <div class="module-heading">
                                <h3 class="module-title">
                                    <asp:Label runat="server" ID="lblSeccion" Text="Usuarios" /></h3>
                            </div>
                            <p>
                                Los usuarios son las personas que tienen un registro en tu cuenta. Aquí se incluyen los usuarios finales que son los solicitantes de eventos (clientes, proveedores y empleados) así como los agentes que son las personas designadas a resolverlos.
                            </p>
                        </div>

                        <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12">
                            <div class="form col-lg-5">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">Consulta Usuarios:</label>
                                    <div class="col-xs-10 col-sm-10 col-md-10 col-lg-10 no-padding-left no-margin-left">
                                        <asp:TextBox runat="server" ID="txtFiltro" CssClass="form-control no-padding-left no-margin-left" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                    <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 margin-top-3">
                                        <asp:LinkButton runat="server" ID="btnBuscar" CssClass="btn btn-primary btn-single-icon" OnClick="btnBuscar_OnClick"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-3 col-sm-3 col-md-3 col-lg-3 separador-vertical-derecho">
                                <div class="form-group">
                                    <label class="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left no-margin-left">... o Consulta por Tipo de Usuario</label>
                                    <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12  no-padding-left no-margin-left">
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control no-padding-left no-margin-left" Width="190px" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>

                            <div class="form col-xs-4 col-sm-4 col-md-4 col-lg-4 text-center">
                                <div class="form-group margin-top-btn-consulta">
                                    <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>

                                    <asp:LinkButton CssClass="btn btn-success" ID="btnNuevo" OnClick="btnNew_OnClick" runat="server">
                                <i class="fa fa-plus"></i>Nuevo</asp:LinkButton>

                                </div>
                            </div>
                        </div>


                    </div>
            </section>
            <!--/MÓDULO-->


            <section class="module module-headings">
                <div class="module-inner">
                    <div class="module-content collapse in" id="content-1">
                        <div class="module-content-inner no-padding-bottom">
                            <div class="table-responsive">

                                <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false" Width="99%"
                                    OnPageIndexChanging="gvPaginacion_PageIndexChanging" PagerSettings-PageButtonCount="25"
                                    BorderStyle="None" PagerSettings-Mode="Numeric" PageSize="15" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                    PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" CssClass="table table-striped display alineaTablaIzquierda">
                                    <Columns>
                                        <asp:TemplateField HeaderText="TU" HeaderStyle-Width="25px">
                                            <ItemTemplate>
                                                <div class="altoFijo">
                                                    <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                        <%# Eval("TipoUsuario.Abreviacion") %></button>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre Completo" HeaderStyle-Width="44%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("NombreCompleto")%>'><%# Eval("NombreCompleto")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Usuario" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("NombreUsuario")%>'><%# Eval("NombreUsuario")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ult. Movimiento" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# Eval("FechaActualizacion") == null ?  Eval("FechaAlta", "{0:d}") : Eval("FechaActualizacion", "{0:d}")%>'><%# Eval("FechaActualizacion") == null ?  Eval("FechaAlta", "{0:d}") : Eval("FechaActualizacion", "{0:d}")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Verificado" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <label runat="server" class="ocultaTexto" title='<%# (bool) Eval("Activo") ? "SI" : "NO"%>'><%# (bool) Eval("Activo") ? "SI" : "NO"%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Editar" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled hidden" id="hiddenEditar">
                                                    <li>
                                                        <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id")%>' OnClick="btnEditar_OnClick" Visible='<%# (bool) Eval("Habilitado") %>'><asp:Image runat="server" ImageUrl="~/assets/images/icons/editar.png" /> </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="4%">
                                            <ItemTemplate>
                                                <ul class="list list-unstyled" id="hiddenEnabled">
                                                    <li>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" Checked='<%# (bool) Eval("Habilitado") %>' Visible='<%# !(bool) Eval("Sistema") %>' CssClass="chkIphone" Width="30px" data-id='<%# Eval("Id")%>' Text='<%# (bool) Eval("Habilitado") ? "SI" : "NO"%>' OnCheckedChanged="OnCheckedChanged" />
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

        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>

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



