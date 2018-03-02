<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleGrupoOpciones.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleGrupoOpciones" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdGrupo" />

        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblOperacion" Text="Tipificación" />
            </h6>
        </div>

        <section class="module no-border">
            <div class="row">
                <div class="module-inner ">
                    <div class="col-lg-9 col-md-9 padding-8-top">
                        <asp:Label runat="server" ID="Label1" Text="Consulta las tipificaciones asignadas a este grupo" />
                    </div>
                    <div class="col-lg-3 col-md-3 text-right">
                        <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>
                    </div>
                    <hr class="col-lg-12 col-md-12 no-margin-bottom" />
                </div>
            </div>
        </section>
        <div id="masonry" class="row">
            <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <section class="module module-headings no-border">
                    <div class="module-inner">
                        <div class="row">
                            <asp:Label runat="server" ID="Label2" Text="Grupo: " class="col-lg-2 col-md-2" />
                            <asp:Label runat="server" ID="lblNombreGrupo" Text="Configuración de Menú" class="col-lg-10 col-md-10" />
                           <%-- <br />
                            <asp:Label runat="server" ID="Label4" Text="Tipo de usuario:" class="col-lg-2 col-md-2" />
                            <asp:Label runat="server" ID="lblTipoUsuario" Text="Configuración de Menú" class="col-lg-10 col-md-10" />--%>
                            <br />
                            <asp:Label runat="server" ID="Label3" Text="Rol:" class="col-lg-2 col-md-2" />
                            <asp:Label runat="server" ID="lblTipoGrupo" Text="Configuración de Menú" class="col-lg-10 col-md-10" />
                        </div>

                        <div class="table-responsive">

                            <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false"
                                CssClass="table table-striped display alineaTablaIzquierda margin-top-20" Width="99%"
                                OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                BorderStyle="None" PagerSettings-Mode="Numeric"
                                PageSize="10" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                <Columns>
                                    <asp:TemplateField HeaderText="TU" HeaderStyle-Width="8%" Visible="true">
                                        <ItemTemplate>
                                            <label><%# Eval("TipoUsuario")%></label>
                                            <%--<div class="altoFijo">
                                                <button type="button" class="btn btn-default-alt btn-square-usuario" style='<%# "Border: none !important; Background: " + Eval("TipoUsuario.Color") + " !important" %>'>
                                                    <%# Eval("TipoUsuario.Abreviacion") %></button>
                                            </div>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <label><%# Eval("Tipo")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Titulo" HeaderStyle-Width="19%">
                                        <ItemTemplate>
                                            <label><%# Eval("Titulo")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Categoría" HeaderStyle-Width="19%">
                                        <ItemTemplate>
                                            <label><%# Eval("Categoria")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Tipificación" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <label><%# Eval("DescripcionTipificacion")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>




                                    <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <label><%# Eval("Nivel")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                        <ItemTemplate>
                                            <label><%# Eval("Activo")%></label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Opciones" HeaderStyle-Width="7%">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Text="Ver" CommandArgument='<%# Eval("Id")%>' ID="lnkBtnDetalleOpcion" OnClick="lnkBtnDetalleOpcion_OnClick"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
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
    <Triggers>
        <asp:PostBackTrigger ControlID="btnDownload" />
    </Triggers>
</asp:UpdatePanel>
