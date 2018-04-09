<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcDetalleArbolAcceso.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcDetalleArbolAcceso" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbolAcceso" />

        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnClose_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblOperacion" Text="Consulta Servicio/Incidente" />
            </h6>
        </div>

        <section class="module no-border">
            <div class="row">
                <div class="module-inner">
                    <div class="col-lg-8 col-md-8 padding-8-top">
                        <asp:Label runat="server" ID="Label1" Text="Consulta los detalles de la consulta, servicio o incidente" />
                    </div>
                    <%--<div class="col-lg-4 col-md-4 text-right">
                        <asp:LinkButton ID="btnDownload" runat="server" CssClass="btn btn-primary" OnClick="btnDownload_OnClick">
                                 <i class="fa fa-download"></i>  Descargar reporte</asp:LinkButton>
                    </div>--%>
                    <hr class="col-lg-12 col-md-12 no-margin-bottom" />
                </div>
            </div>
        </section>

        <section class="module">
            <div class="module-inner">
                <div class="row">
                    <asp:Label runat="server" ID="Label2" Text="Título Opción:" class="col-lg-4 col-md-4" />
                    <asp:Label runat="server" ID="lblTitulo" class="col-lg-8 col-md-8" />
                    <br />
                    <asp:Label runat="server" ID="Label3" Text="Categoría:" class="col-lg-4 col-md-4" />
                    <asp:Label runat="server" ID="lblCategoria" class="col-lg-8 col-md-8" />
                    <br />
                    <%--<asp:Label runat="server" ID="Label4" Text="Tipo Usuario:" class="col-lg-3 col-md-3" />
                    <asp:Label runat="server" ID="lblTipoUsuario" class="col-lg-9 col-md-9" />--%>
                </div>
            </div>

            <div class="row">
                <div class="module-inner">

                    <div class="row form-group">
                        <span class="col-lg-4">Quien ve el contenido</span>
                        <asp:Label runat="server" ID="lblTipoUsuarioOpcion" Text="Tipo Usuario" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Tipificación</span>
                        <asp:Label runat="server" ID="lblTipificacion" Text="Tipificación" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Formulario</span>
                        <asp:Label runat="server" ID="lblFormulario" Text="Formulario?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Activo</span>
                        <asp:Label runat="server" ID="lblActivo" Text="Activo?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Público </span>
                        <asp:Label runat="server" ID="lblPublico" Text="Activo?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Título de la opción</span>
                        <asp:Label runat="server" ID="lblTituloOpcion" Text="Titulo?" class="col-lg-8" />
                    </div>
                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner">
                    <div class="row form-group">
                        <span class="col-lg-4">Categoria:</span>
                        <asp:Label runat="server" ID="lblCategoriaOpcion" Text="Categoria?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 1:</span>
                        <asp:Label runat="server" ID="lblNivel1" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 2:</span>
                        <asp:Label runat="server" ID="lblNivel2" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 3:</span>
                        <asp:Label runat="server" ID="lblNivel3" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 4:</span>
                        <asp:Label runat="server" ID="lblNivel4" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 5:</span>
                        <asp:Label runat="server" ID="lblNivel5" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 6:</span>
                        <asp:Label runat="server" ID="lblNivel6" Text="Nivel?" class="col-lg-8" />
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Nivel 7:</span>
                        <asp:Label runat="server" ID="lblNivel7" Text="Nivel?" class="col-lg-8" />
                    </div>

                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner">

                    <div class="row form-group">
                        <span class="col-lg-12">Cuáles grupos ven la opción</span>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Usuarios:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptUsuarios">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Contenido:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptContenido">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Operación:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptOperacion">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Desarrollo:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptDesarrollo">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Atención:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptAtencion">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Consultas Especiales:</span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptConsulta">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label><br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Categoría: </span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptCategoria">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Agente Universal: </span>
                        <div class="col-lg-8">
                            <asp:Repeater runat="server" ID="rptUniversal">
                                <ItemTemplate>
                                    <label class="no-padding-top"><%# Eval("Descripcion")%></label>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner">

                    <div class="row form-group">
                        <span class="col-lg-4">SLA</span>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Tiempo Total</span>
                        <asp:Label runat="server" ID="lblSlaTotal" Text="Nivel?" class="col-lg-8"/>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Prioridad</span>
                        <asp:Label runat="server" ID="lblPrioridad" Text="Nivel?" class="col-lg-8"/>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Impacto</span>
                        <asp:Label runat="server" ID="lblImpacto" Text="Nivel?" class="col-lg-8"/>
                    </div>

                </div>
            </div>
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner">

                    <div class="row form-group">
                        <span class="col-lg-12">Notificaciones</span>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Categoria</span>
                        <asp:Repeater runat="server" ID="rptInformeCategoria">
                            <ItemTemplate>
                                <label><%# Eval("GrupoUsuario.Descripcion")%></label>
                                <label><%# Eval("TipoNotificacion.Descripcion")%></label>
                                <label><%# "Después de vencimiento" + ((bool)Eval("AntesVencimiento") ? "No" : "Si")%></label>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable de Contenido</span>
                        <asp:Repeater runat="server" ID="rptInformeContenido">
                            <ItemTemplate>
                                <label><%# Eval("GrupoUsuario.Descripcion")%></label>
                                <label><%# Eval("TipoNotificacion.Descripcion")%></label>
                                <label><%# "Después de vencimiento" + ((bool)Eval("AntesVencimiento") ? "No" : "Si")%></label>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Responsable desarrollo</span>
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
        </section>

        <section class="module">
            <div class="row">
                <div class="module-inner">

                    <div class="row form-group">
                        <span class="col-lg-4">Encuesta</span>
                        <asp:Label runat="server" ID="lblEncuesta" Text="Nivel?" class="col-lg-8"/>
                    </div>

                    <div class="row form-group">
                        <span class="col-lg-4">Activo</span>
                        <asp:Label runat="server" ID="lblActivoEncuesta" Text="Nivel?" class="col-lg-8"/>
                    </div>

                </div>
            </div>
        </section>

    </ContentTemplate>
</asp:UpdatePanel>
