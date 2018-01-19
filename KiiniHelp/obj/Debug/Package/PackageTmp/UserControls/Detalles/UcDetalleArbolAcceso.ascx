<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleArbolAcceso.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleArbolAcceso" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbolAcceso" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnClose_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h2 class="modal-title" id="modal-new-ticket-label">
                <br />
                <asp:Label runat="server" Text="Consulta Servicio/Incidente"></asp:Label></h2>
        </div>

        <section class="module">

            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblTitulo" Text="Titulo Opcion" />
                                <asp:Label runat="server" ID="lblCategoria" Text="Categoria" />
                                <asp:Label runat="server" ID="lblTipoUsuario" Text="TipoUsuario" />
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-3 col-md-3 text-center">
                        <div class="module-inner">
                            <asp:LinkButton runat="server" CssClass="btn btn-success fa fa-plus" Text="Bajar a Excel" ID="btnDownload" />
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Quien ve el contenido</span><asp:Label runat="server" ID="lblTipoUsuarioOpcion" Text="Tipo Usuario" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Tipificación</span><asp:Label runat="server" ID="lblTipificacion" Text="Tipificación" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Formulario</span><asp:Label runat="server" ID="lblFormulario" Text="Formulario?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Activo</span><asp:Label runat="server" ID="lblActivo" Text="Activo?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Publico </span><asp:Label runat="server" ID="lblPublico" Text="Activo?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Titulo de la opción</span><asp:Label runat="server" ID="lblTituloOpcion" Text="Titulo?" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Categoria</span><asp:Label runat="server" ID="lblCategoriaOpcion" Text="Catgoria?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 1</span><asp:Label runat="server" ID="lblNivel1" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 2</span><asp:Label runat="server" ID="lblNivel2" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 3</span><asp:Label runat="server" ID="lblNivel3" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 4</span><asp:Label runat="server" ID="lblNivel4" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 5</span><asp:Label runat="server" ID="lblNivel5" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 6</span><asp:Label runat="server" ID="lblNivel6" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Nivel 7</span><asp:Label runat="server" ID="lblNivel7" Text="Nivel?" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Cuáles grupos ven la opción</span>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Usuarios</span>
                                <asp:Repeater runat="server" ID="rptUsuarios">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Contenido</span>
                                <asp:Repeater runat="server" ID="rptContenido">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Operación</span>
                                <asp:Repeater runat="server" ID="rptOperacion">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Desarrollo</span>
                                <asp:Repeater runat="server" ID="rptDesarrollo">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Atencion</span>
                                <asp:Repeater runat="server" ID="rptAtencion">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Consultas Especiales</span>
                                <asp:Repeater runat="server" ID="rptConsulta">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Categoría</span>
                                <asp:Repeater runat="server" ID="rptCategoria">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Agente Universal</span>
                                <asp:Repeater runat="server" ID="rptUniversal">
                                    <ItemTemplate>
                                        <label><%# Eval("Descripcion")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">SLAs</span>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Tiempo Total</span><asp:Label runat="server" ID="lblSlaTotal" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Prioridad</span><asp:Label runat="server" ID="lblPrioridad" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Impacto</span><asp:Label runat="server" ID="lblImpacto" Text="Nivel?" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Notificaciones</span>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Categoria</span>
                                <asp:Repeater runat="server" ID="rptInformeCategoria">
                                    <ItemTemplate>
                                        <label><%# Eval("GrupoUsuario.Descripcion")%></label>
                                        <label><%# Eval("TipoNotificacion.Descripcion")%></label>
                                        <label><%# "Después de vencimiento" + ((bool)Eval("AntesVencimiento") ? "No" : "Si")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable de Contenido</span>
                                <asp:Repeater runat="server" ID="rptInformeContenido">
                                    <ItemTemplate>
                                        <label><%# Eval("GrupoUsuario.Descripcion")%></label>
                                        <label><%# Eval("TipoNotificacion.Descripcion")%></label>
                                        <label><%# "Después de vencimiento" + ((bool)Eval("AntesVencimiento") ? "No" : "Si")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Responsable desarrollo</span>
                                <asp:Repeater runat="server" ID="rptInformeDesarrollo">
                                    <ItemTemplate>
                                        <label><%# Eval("GrupoUsuario.Descripcion")%></label>
                                        <label><%# Eval("TipoNotificacion.Descripcion")%></label>
                                        <label><%# "Después de vencimiento" + ((bool)Eval("AntesVencimiento") ? "No" : "Si")%></label>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner separador-vertical-derecho">
                    <div class="col-lg-8 col-md-8">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Encuesta</span><asp:Label runat="server" ID="lblEncuesta" Text="Nivel?" />
                            </div>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <span class="col-lg-3">Activo</span><asp:Label runat="server" ID="lblActivoEncuesta" Text="Nivel?" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>


    </ContentTemplate>
</asp:UpdatePanel>
