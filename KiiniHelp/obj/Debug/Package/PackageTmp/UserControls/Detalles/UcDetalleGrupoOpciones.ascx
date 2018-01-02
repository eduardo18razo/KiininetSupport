﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleGrupoOpciones.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleGrupoOpciones" %>
<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfIdGrupo"></asp:HiddenField>
            <div class="modal-header">
                <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h2 class="modal-title" id="modal-new-ticket-label">
                    <br />
                    <asp:Label runat="server" ID="lbltitulo"></asp:Label></h2>
                <asp:Label runat="server" Text="Alta Grupo de Usuario" ID="lblTitle" Visible="False"></asp:Label>
            </div>

            <section class="module">

                <div class="row">
                    <div class="module-inner separador-vertical-derecho">
                        <div class="col-lg-8 col-md-8">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <asp:Label runat="server" ID="lblTipoGrupo" Text="Configuración de Menú" />
                                    <asp:Label runat="server" ID="lblNombreGrupo" Text="Configuración de Menú" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 text-center">
                            <div class="module-inner">
                                <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Bajar a Excel" ID="btnDownload" OnClick="btnDownload_OnClick" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <div id="masonry" class="row">
                <div class="module-wrapper masonry-item col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <section class="module module-headings">
                        <div class="module-inner">
                            <div class="module-heading">
                                <ul class="actions list-inline">
                                    <li><a class="collapse-module" data-toggle="collapse" href="#content-1" aria-expanded="false" aria-controls="content-1"><span aria-hidden="true" class="icon arrow_carrot-up"></span></a></li>
                                </ul>
                            </div>
                            <div class="module-content collapse in" id="content-1">
                                <div class="module-content-inner no-padding-bottom">
                                    <div class="table-responsive">

                                        <asp:GridView runat="server" ID="tblResults" AllowPaging="true" AutoGenerateColumns="false"
                                            CssClass="table table-striped display" Width="99%"
                                            OnPageIndexChanging="gvPaginacion_PageIndexChanging"
                                            BorderStyle="None" PagerSettings-Mode="Numeric"
                                            PageSize="5" PagerSettings-Position="Bottom" PagerStyle-BorderStyle="None"
                                            PagerStyle-HorizontalAlign="Right" PagerStyle-CssClass="paginador" PagerSettings-PageButtonCount="20">
                                            <Columns>
                                                <asp:TemplateField HeaderText="TU" ControlStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("TipoUsuario")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Titulo" HeaderStyle-Width="25%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Titulo")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Categoría" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Categoria")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tipificación" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("DescripcionTipificacion")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Tipo")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Nivel" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Nivel")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Activo" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Activo")%></label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Opciones" HeaderStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text="Ir" CommandArgument='<%# Eval("Id")%>' ID="lnkBtnDetalleOpcion" OnClick="lnkBtnDetalleOpcion_OnClick"></asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
        </Triggers>
    </asp:UpdatePanel>
</div>