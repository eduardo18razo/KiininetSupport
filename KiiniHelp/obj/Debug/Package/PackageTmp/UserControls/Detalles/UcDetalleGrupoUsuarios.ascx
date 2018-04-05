<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleGrupoUsuarios.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleGrupoUsuarios" %>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdGrupo"></asp:HiddenField>

            <div class="modal-header">
                <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h6 class="modal-title" id="modal-new-ticket-label">
                    <asp:Label runat="server" ID="lblOperacion" Text="Usuarios" Font-Bold="true" />
                </h6>
            </div>

            <section class="module no-border">
                <div class="row">
                    <div class="module-inner">
                        <div class="col-lg-9 col-md-9 padding-8-top">
                            <asp:Label runat="server" ID="Label1" Text="Consulta los usuarios asignados a este grupo" />
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
                    <div class="module module-headings no-border">
                        <div class="module-inner no-padding-top">
                            <div class="row">
                                <asp:Label runat="server" ID="Label2" Text="Grupo:" class="col-lg-2 col-md-2" />
                                <asp:Label runat="server" ID="lblNombreGrupo" Text="Configuración de Menú" class="col-lg-10 col-md-10" />
                                <br />
                                <asp:Label runat="server" ID="Label3" Text="Tipo de usuario:" class="col-lg-2 col-md-2" />
                                <asp:Label runat="server" ID="lblTipoUsuario" Text="Configuración de Menú" class="col-lg-10 col-md-10" />
                                <br />
                                <asp:Label runat="server" ID="Label4" Text="Rol:" class="col-lg-2 col-md-2" />
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
                                        <asp:TemplateField HeaderText="Nombre Usuario" HeaderStyle-Width="35%">
                                            <ItemTemplate>
                                                <label><%# Eval("NombreCompleto")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Nombre en el Sistema" Visible="false">
                                            <ItemTemplate>
                                                <label><%# Eval("NombreUsuarioCompleto")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Supervisor" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <label><%# Eval("Supervisor")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="1er Nivel" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <label><%# Eval("PrimerNivel")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="2do Nivel" HeaderStyle-Width="12%">
                                            <ItemTemplate>
                                                <label><%# Eval("SegundoNivel")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="3er Nivel" HeaderStyle-Width="11%">
                                            <ItemTemplate>
                                                <label><%# Eval("TercerNivel")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="4to Nivel" HeaderStyle-Width="11%">
                                            <ItemTemplate>
                                                <label><%# Eval("CuartoNivel")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <label><%# Eval("Activo")%></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>

                        </div>
                    </div>
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
