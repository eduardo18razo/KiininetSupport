<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaServicio.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcAltaServicio" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row" runat="server" id="divData">
                    <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                        <!--Step 1-->
                        <div class="margin-top" runat="server" id="divStep1">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel1" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnPaso1" CommandArgument="1" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step1.png" Width="20" Height="20" alt="" runat="server" />&nbsp;CONFIGURACIÓN
                            </asp:LinkButton>
                        </div>

                        <!--Step 2-->
                        <div class="margin-top" runat="server" id="divStep2" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel2" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnStatusNivel2" CommandArgument="2" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step2.png" Width="20" Height="20" alt="" runat="server" />&nbsp;NIVEL DEL MENÚ
                            </asp:LinkButton>
                        </div>

                        <!--Step 3-->
                        <div class="margin-top" runat="server" id="divStep3" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel3" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="btnStatusNivel3" CommandArgument="3" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;GRUPOS
                            </asp:LinkButton>
                        </div>
                        <!--Step 4-->
                        <div class="margin-top" runat="server" id="divStep4" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I1" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="LinkButton1" CommandArgument="4" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step4.png" Width="20" Height="20" alt="" runat="server" />&nbsp;SLA
                            </asp:LinkButton>
                        </div>
                        <!--Step 5-->
                        <div class="margin-top" runat="server" id="divStep5" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I2" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="LinkButton2" CommandArgument="5" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step5.png" Width="20" Height="20" alt="" runat="server" />&nbsp;NOTIFICACIONES
                            </asp:LinkButton>
                        </div>
                        <!--Step 6-->
                        <div class="margin-top" runat="server" id="divStep6" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I3" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square-short" ID="LinkButton3" CommandArgument="6" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step6.png" Width="20" Height="20" alt="" runat="server" />&nbsp;ENCUESTAS
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div>
                        <div class="col-lg-8 col-md-8 col-sm-12 col-xs-12">
                            <div class="module no-border-bottom">
                                <div class="module-inner padding-10-left no-padding-right no-padding-top">
                                    <div runat="server" id="divStep1Data">
                                        <div class="form-group">
                                            Escribe el título de la opción<br />
                                            <asp:TextBox runat="server" ID="txtDescripcionNivel" class="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                        </div>
                                        <div class="form-group">
                                            Quien ve el contenido<br />
                                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" OnSelectedIndexChanged="ddlTipoUsuarioNivel_OnSelectedIndexChanged" />
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2 no-padding-top">Público</label>
                                            <asp:CheckBox runat="server" ID="chkPublico" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" />
                                        </div>
                                        <br />
                                        <div class="form-group">
                                            Tipificación<br />
                                            <asp:DropDownList runat="server" ID="ddlTipificacion" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            Formulario<br />
                                            <asp:DropDownList runat="server" ID="ddlFormularios" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            <label class="col-lg-2  no-padding-top">Activo</label>
                                            <asp:CheckBox runat="server" ID="chkNivelHabilitado" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="True" />
                                        </div>
                                    </div>
                                    <div runat="server" id="divStep2Data" visible="false">
                                        <div class="form-group">
                                            Mostrar en categoría<br />
                                            <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <asp:TextBox CssClass="form-control margin-top-10" ID="txtDescripcionArea" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnGuardarArea" OnClick="btnGuardarArea_OnClick" class="fa fa-plus-circle" />

                                        </div>
                                        <hr />
                                        <div class="form-group">
                                            Mostrar en sección
                                        </div>
                                        <div class="form-group">
                                            Nivel 1<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel1" CssClass="form-control" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>
                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN1" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel1" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" />
                                        </div>
                                        <div class="form-group" runat="server" id="divNivel2" visible="False">
                                            Nivel 2<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel2" CssClass="form-control" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>

                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN2" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel2" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />

                                        </div>
                                        <div class="form-group" runat="server" id="divNivel3" visible="False">
                                            Nivel 3<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel3" CssClass="form-control" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>
                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN3" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel3" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />

                                        </div>
                                        <div class="form-group" runat="server" id="divNivel4" visible="False">
                                            Nivel 4<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel4" CssClass="form-control" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>
                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN4" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel4" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />

                                        </div>
                                        <div class="form-group" runat="server" id="divNivel5" visible="False">
                                            Nivel 5<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel5" CssClass="form-control" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>
                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN5" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel5" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                                        </div>
                                        <div class="form-group" runat="server" id="divNivel6" visible="False">
                                            Nivel 6<br />
                                            <asp:DropDownList runat="server" ID="ddlNivel6" CssClass="form-control" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" />
                                            <label class="margin-top-5">Crear nueva sección</label>
                                            <asp:TextBox CssClass="form-control" ID="txtDescripcionN6" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                            <asp:LinkButton runat="server" ID="btnAgregarNivel6" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="6" Enabled="False" />
                                        </div>
                                    </div>
                                    <div runat="server" id="divStep3Data" visible="false">
                                        <div class="form-group">
                                            Cuáles grupos ven la opción
                                        </div>
                                        <div class="form-group">
                                            Usuarios<br />
                                            <asp:DropDownList runat="server" ID="ddlGrupoAcceso" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            Responsable de Contenido<br />
                                            <asp:DropDownList runat="server" ID="ddlGrupoResponsableMantenimiento" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            Responsable de Operación<br />
                                            <asp:DropDownList runat="server" ID="ddlGruporesponsableOperacion" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            Responsable de Desarrollo<br />
                                            <asp:DropDownList runat="server" ID="ddlGrupoResponsableDesarrollo" CssClass="form-control" />
                                        </div>
                                        <div class="form-group">
                                            Responsable de Atención<br />
                                            <asp:DropDownList runat="server" ID="ddlGrupoResponsableAtencion" CssClass="form-control" />
                                        </div>

                                        <div class="form-group">
                                            Responsable de Categoría<br />
                                            <asp:DropDownList runat="server" ID="ddlGrupoDuenoServicio" CssClass="form-control" />
                                        </div>

                                        <div class="form-group">
                                            Agente Universal<br />
                                            <asp:ListBox ID="lstGruposAu" SelectionMode="Multiple" runat="server" />
                                        </div>
                                    </div>
                                    <div runat="server" id="divStep4Data" visible="false">
                                        <div class="form-group">
                                            SLA <br />
                                            Establece un tiempo de respuesta para este evento. 
                                        </div>
                                        <hr />
                                        <div class="form-group">
                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 no-padding">
                                                Tiempo total<br />
                                                <asp:TextBox runat="server" ID="txtTiempoTotal" CssClass="form-control" MaxLength="3" onkeypress="return ValidaCampo(this, 2)" />
                                            </div>
                                            <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12 no-padding">
                                                <div class="margin-left-10">
                                                    Unidad de tiempo<br />
                                                    <asp:DropDownList runat="server" ID="ddlTiempoTotal" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" runat="server" visible="False">
                                            <label class="col-lg-10">Detallado</label>
                                            <asp:CheckBox runat="server" Text="Detallado" ID="chkEstimado" AutoPostBack="True" CssClass="chkIphone" Width="30px" OnCheckedChanged="chkEstimado_OnCheckedChanged" />
                                        </div>
                                        <div runat="server" id="divDetalle" class="form-group" visible="False">
                                            <asp:Repeater runat="server" ID="rptSubRoles" OnItemDataBound="rptSubRoles_OnItemDataBound">
                                                <HeaderTemplate>
                                                    <table class="table table-responsive" id="tblHeader">
                                                        <thead>
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:Label runat="server">Sub Rol</asp:Label></td>
                                                                <td>
                                                                    <asp:Label runat="server">Tiempo</asp:Label></td>
                                                                <td>
                                                                    <asp:Label runat="server">Unidad</asp:Label></td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr align="center" id='<%# Eval("IdSubRol")%>'>
                                                        <td style="padding: 0; display: none">
                                                            <asp:Label runat="server" Text='<%# Eval("IdSubRol") %>' ID="lblIdSubRol" /></td>
                                                        <td>
                                                            <asp:Label runat="server" Text='<%# Eval("SubRol.Descripcion") %>' /></td>
                                                        <td>
                                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" ID="txtDias" Enabled="False" /></td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlDuracionRepeater" CssClass="form-control" Enabled="False" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                               <tfoot>
                               </tfoot>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding margin-top-20" />
                                        <div class="form-group">
                                            <div style="width: 50%; float: left">
                                                <div class="form-group">
                                                    Gravedad<br />
                                                    <asp:DropDownList runat="server" ID="ddlPrioridad" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlImpacto_OnSelectedIndexChanged" />
                                                </div>
                                                <div class="form-group">
                                                    Atención<br />
                                                    <asp:DropDownList runat="server" ID="ddlUrgencia" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlUrgencia_OnSelectedIndexChanged" />
                                                </div>
                                            </div>
                                            <div style="width: 50%; float: left">
                                                <div class="form-group center-content-div margin-top-40" runat="server" id="divImpacto" visible="False">
                                                    Prioridad<br />
                                                    <%--  <br />
                                                    <asp:Image runat="server" ID="imgImpacto" Style="width: 50px" />
                                                    <br />--%>
                                                    <br />
                                                    <strong>
                                                        <asp:Label runat="server" ID="lblImpacto" /></strong>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div runat="server" id="divStep5Data" visible="False">
                                        <div class="form-group">
                                            Notificaciones <br />
                                            Establece el tiempo en el que se recibirán notificaciones sobre este evento.
                                        </div>


                                        <hr />
                                        <%--Inicio Responsable de categoría--%>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionDueno" AutoPostBack="True" OnCheckedChanged="chkNotificacionDueno_OnCheckedChanged" CssClass="col-sm-12 col-md-12 col-lg-12" Text="Responsable de categoría" />
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Tiempo total
                                                <br />
                                                <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtTiempoNotificacionDueno" />
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Unidad de tiempo
                                                <br />
                                                <asp:DropDownList runat="server" ID="ddlNotificacionGrupoDueño" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3">
                                                Medio<br />
                                                <asp:DropDownList runat="server" ID="ddlCanalDueño" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <asp:CheckBox runat="server" ID="chkVencimientoDueño" Checked="False" Text="Despues de vencimiento" Enabled="False" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <%--Fin Responsable de categoría--%>

                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding" />

                                        <%--Inicio Responsable de contenido--%>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionMtto" AutoPostBack="True" OnCheckedChanged="chkNotificacionMtto_OnCheckedChanged" CssClass="col-sm-12 col-md-12 col-lg-12" Text="Responsable de contenido." />
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Tiempo total
                                                <br />
                                                <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtTiempoNotificacionMtto" />
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Unidad de tiempo
                                                <br />
                                                <asp:DropDownList runat="server" ID="ddlNotificacionGrupoMtto" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3">
                                                Medio<br />
                                                <asp:DropDownList runat="server" ID="ddlCanalMtto" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <asp:CheckBox runat="server" ID="chkVencimientoMtto" Checked="False" Text="Despues de vencimiento" Enabled="False" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <%--Fin Responsable de contenido--%>

                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding" />

                                        <%--Inicio Responsable de desarrollo--%>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionDesarrollo" AutoPostBack="True" OnCheckedChanged="chkNotificacionDesarrollo_OnCheckedChanged" CssClass="col-sm-12 col-md-12 col-lg-12" Text="Responsable de desarrollo" />
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Tiempo total
                                                <br />
                                                <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtTiempoNotificacionDev" />
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Unidad de tiempo
                                                <br />
                                                <asp:DropDownList runat="server" ID="ddlNotificacionGrupoDev" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3">
                                                Medio<br />
                                                <asp:DropDownList runat="server" ID="ddlCanalDev" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <asp:CheckBox runat="server" ID="chkVencimientoDev" Checked="False" Text="Despues de vencimiento" Enabled="False" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <%--Fin Responsable de desarrollo--%>

                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding" />

                                        <%--Inicio Responsable de operación--%>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionOperacion" AutoPostBack="True" OnCheckedChanged="chkNotificacionOperacion_OnCheckedChanged" CssClass="col-sm-12 col-md-12 col-lg-12" Text="Responsable de operación" />
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Tiempo total
                                                <br />
                                                <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control" Enabled="False" ID="txtTiempoNotificacionOperacion" />
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4">
                                                Unidad de tiempo
                                                <br />
                                                <asp:DropDownList runat="server" ID="ddlNotificacionGrupoOperacion" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <div class="col-sm-3 col-md-3 col-lg-3">
                                                Medio<br />
                                                <asp:DropDownList runat="server" ID="ddlCanalOperacion" CssClass="form-control" Enabled="False" />
                                            </div>
                                            <asp:CheckBox runat="server" ID="chkVencimientoOperacion" Checked="False" Text="Despues de vencimiento" Enabled="False" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <%--Fin Responsable de operación--%>

                                        <hr class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-padding" />

                                    </div>
                                    <div runat="server" id="divStep6Data" visible="False">
                                        Encuesta<br />
                                        <hr />
                                        <label class="col-lg-2">Activo</label>
                                        <asp:CheckBox runat="server" ID="chkEncuestaActiva" CssClass="chkIphone no-margin-bottom" Width="30px" Text="Activo" Checked="False" OnCheckedChanged="chkEncuestaActiva_OnCheckedChanged" AutoPostBack="True" />
                                        <asp:DropDownList runat="server" ID="ddlEncuesta" CssClass="form-control" Style="width: 100%" Enabled="False" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <p class="text-right margin-top">
                        <asp:Button runat="server" ID="btnPreview" Text="Previsualizar" CssClass="btn btn-default" Visible="False" OnClick="btnPreview_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Siguiente" ID="btnSiguiente" OnClick="btnSiguiente_OnClick" CommandArgument="1" />
                        <asp:Button runat="server" ID="btnSaveAll" Text="Guardar" CssClass="btn btn-primary" Visible="False" OnClick="btnSaveAll_OnClick" />
                    </p>
                </div>
                </div>
        <script type="text/javascript">
            $(function () {
                $('[id*=lstGruposAu]').multiselect({
                    includeSelectAllOption: true
                });
            });
            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function () {
                $('[id*=lstGruposAu]').multiselect({
                    includeSelectAllOption: true
                });
            });

        </script>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
