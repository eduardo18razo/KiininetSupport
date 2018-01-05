<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaServicio.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcAltaServicio" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row" runat="server" id="divData">
                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                        <!--Step 1-->
                        <div class="margin-top" runat="server" id="divStep1">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel1" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="btnPaso1" CommandArgument="1" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step1.png" Width="20" Height="20" alt="" runat="server" />&nbsp;CONFIGURACIÓN
                            </asp:LinkButton>
                        </div>

                        <!--Step 2-->
                        <div class="margin-top" runat="server" id="divStep2" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel2" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="btnStatusNivel2" CommandArgument="2" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step2.png" Width="20" Height="20" alt="" runat="server" />&nbsp;NIVEL DEL MENÚ
                            </asp:LinkButton>
                        </div>

                        <!--Step 3-->
                        <div class="margin-top" runat="server" id="divStep3" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel3" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="btnStatusNivel3" CommandArgument="3" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;GRUPOS
                            </asp:LinkButton>
                        </div>
                        <!--Step 4-->
                        <div class="margin-top" runat="server" id="divStep4" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I1" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="LinkButton1" CommandArgument="4" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;SLA
                            </asp:LinkButton>
                        </div>
                        <!--Step 5-->
                        <div class="margin-top" runat="server" id="divStep5" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I2" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="LinkButton2" CommandArgument="5" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;NOTIFICACIONES
                            </asp:LinkButton>
                        </div>
                        <!--Step 6-->
                        <div class="margin-top" runat="server" id="divStep6" visible="False">
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="I3" visible="False"></i>
                            <br />
                            <asp:LinkButton runat="server" CssClass="btn btn-primary btn-square" ID="LinkButton3" CommandArgument="6" OnClick="btnPaso_OnClick">
                        <asp:Image ImageUrl="~/assets/images/step3.png" Width="20" Height="20" alt="" runat="server" />&nbsp;ENCUESTAS
                            </asp:LinkButton>
                        </div>
                    </div>
                    <div>
                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                            <div class="bg-grey">
                                <div runat="server" id="divStep1Data">
                                    <div class="form-group">
                                        Escribe el titulo de la opción<br />
                                        <asp:TextBox runat="server" ID="txtDescripcionNivel" placeholder="DESCRIPCION" class="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                    <div class="form-group">
                                        Quien ve el contenido<br />
                                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" OnSelectedIndexChanged="ddlTipoUsuarioNivel_OnSelectedIndexChanged" />
                                    </div>
                                    <div class="form-group">
                                        Tipificación<br />
                                        <asp:DropDownList runat="server" ID="ddlTipificacion" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        Formulario<br />
                                        <asp:DropDownList runat="server" ID="ddlFormularios" CssClass="form-control" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-lg-10">Publico</label>
                                        <asp:CheckBox runat="server" ID="chkPublico" CssClass="chkIphone" Width="30px" Text="Activo" Checked="False" />
                                    </div>
                                    <div class="form-group">
                                        <label class="col-lg-10">Activo</label>
                                        <asp:CheckBox runat="server" ID="chkNivelHabilitado" CssClass="chkIphone" Width="30px" Text="Activo" Checked="True" />
                                    </div>
                                </div>
                                <div runat="server" id="divStep2Data" visible="false">
                                    <div class="form-group">
                                        Mostrar en categoría<br />
                                        <asp:DropDownList runat="server" ID="ddlArea" CssClass="form-control" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionArea" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnGuardarArea" OnClick="btnGuardarArea_OnClick" class="fa fa-plus-circle" />

                                    </div>
                                    <hr />
                                    <div class="form-group">
                                        Mostrar en sección
                                    </div>

                                    <div class="form-group">
                                        Nivel 1<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel1" CssClass="form-control" OnSelectedIndexChanged="ddlNivel1_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN1" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel1" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="1" />
                                    </div>
                                    <div class="form-group" runat="server" id="divNivel2" visible="False">
                                        Nivel 2<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel2" CssClass="form-control" OnSelectedIndexChanged="ddlNivel2_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN2" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel2" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="2" Enabled="False" />

                                    </div>
                                    <div class="form-group" runat="server" id="divNivel3" visible="False">
                                        Nivel 3<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel3" CssClass="form-control" OnSelectedIndexChanged="ddlNivel3_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN3" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel3" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="3" Enabled="False" />

                                    </div>
                                    <div class="form-group" runat="server" id="divNivel4" visible="False">
                                        Nivel 4<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel4" CssClass="form-control" OnSelectedIndexChanged="ddlNivel4_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN4" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel4" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="4" Enabled="False" />

                                    </div>
                                    <div class="form-group" runat="server" id="divNivel5" visible="False">
                                        Nivel 5<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel5" CssClass="form-control" OnSelectedIndexChanged="ddlNivel5_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN5" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel5" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="5" Enabled="False" />
                                    </div>
                                    <div class="form-group" runat="server" id="divNivel6" visible="False">
                                        Nivel 6<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel6" CssClass="form-control" OnSelectedIndexChanged="ddlNivel6_OnSelectedIndexChanged" AutoPostBack="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN6" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel6" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="6" Enabled="False" />
                                    </div>
                                    <%--<div class="form-group" runat="server" id="divNivel7" visible="False">
                                        Nivel 7<br />
                                        <asp:DropDownList runat="server" ID="ddlNivel7" CssClass="form-control" OnSelectedIndexChanged="ddlNivel7_OnSelectedIndexChanged" AutoPostBack="True" AppendDataBoundItems="True" />
                                        <asp:TextBox CssClass="form-control" ID="txtDescripcionN7" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                        <asp:LinkButton runat="server" ID="btnAgregarNivel7" CssClass="fa fa-plus-circle" OnClick="btnAgregarNivel_OnClick" CommandArgument="7" Enabled="False" />
                                    </div>--%>
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
                                    <%--<div class="form-group">
                                        Consultas Especiales<br />
                                        <asp:ListBox ID="lstGrupoEspecialConsultaServicios" runat="server" SelectionMode="Multiple"></asp:ListBox>
                                    </div>--%>

                                    <div class="form-group">
                                        Responsable de Categoría<br />
                                        <asp:DropDownList runat="server" ID="ddlGrupoDuenoServicio" CssClass="form-control" />
                                    </div>


                                    <div class="form-group">
                                        Agente Universal<br />
                                        <asp:ListBox  ID="lstGruposAu" SelectionMode="Multiple" runat="server"/>
                                        <%--<asp:ListBox ID="lbxGruposAgenteUniversal" runat="server" SelectionMode="Multiple" ></asp:ListBox>--%>
                                    </div>

                                </div>
                                <div runat="server" id="divStep4Data" visible="false">
                                    <div class="form-group">
                                        SLAs
                                    </div>
                                    <hr />
                                    <div class="form-group">
                                        Tiempo total<br />
                                        <asp:TextBox runat="server" ID="txtTiempoTotal" CssClass="form-control" MaxLength="3" onkeypress="return ValidaCampo(this, 2)" />
                                        <asp:DropDownList runat="server" ID="ddlTiempoTotal" CssClass="form-control" />
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
                                    <hr />
                                    <div class="form-group">
                                        <div style="width: 50%; float: left">
                                            <div class="form-group">
                                                Prioridad<br />
                                                <asp:DropDownList runat="server" ID="ddlPrioridad" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlImpacto_OnSelectedIndexChanged" />
                                            </div>
                                            <div class="form-group">
                                                Impacto<br />
                                                <asp:DropDownList runat="server" ID="ddlUrgencia" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlUrgencia_OnSelectedIndexChanged" />
                                            </div>
                                        </div>
                                        <div style="width: 50%; float: left">
                                            <div class="form-group center-content-div" runat="server" id="divImpacto" visible="False">
                                                Impacto<br />
                                                <br />
                                                <asp:Image runat="server" ID="imgImpacto" Style="width: 50px" />
                                                <br />
                                                <br />
                                                <strong>
                                                    <asp:Label runat="server" ID="lblImpacto" /></strong>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </div>
                                <div runat="server" id="divStep5Data" visible="False">
                                    <div class="form-group">
                                        Notificaciones
                                    </div>
                                    <hr />
                                    <div class="form-inline">
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionDueno" AutoPostBack="True" OnCheckedChanged="chkNotificacionDueno_OnCheckedChanged" CssClass="col-sm-12 col-md-12 col-lg-12" Text="RESPONSABLE CATEGORÍA" />
                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" CssClass="form-control col-md-2" Enabled="False" ID="txtTiempoNotificacionDueno" />
                                            <asp:DropDownList runat="server" ID="ddlNotificacionGrupoDueño" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:DropDownList runat="server" ID="ddlCanalDueño" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:CheckBox runat="server" ID="chkVencimientoDueño" Checked="False" Text="Despues de vencimiento" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionMtto" AutoPostBack="True" OnCheckedChanged="chkNotificacionMtto_OnCheckedChanged" CssClass="col-md-12" Text="RESPONSABLE DE CONTENIDO." />
                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" Style="width: initial" CssClass="form-control col-md-3" Enabled="False" ID="txtTiempoNotificacionMtto" />
                                            <asp:DropDownList runat="server" ID="ddlNotificacionGrupoMtto" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:DropDownList runat="server" ID="ddlCanalMtto" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:CheckBox runat="server" ID="chkVencimientoMtto" Checked="False" Text="Despues de vencimiento" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionDesarrollo" AutoPostBack="True" OnCheckedChanged="chkNotificacionDesarrollo_OnCheckedChanged" CssClass="col-md-12" Text="RESPONSABLE DESARROLLO" />
                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" Style="width: initial" CssClass="form-control col-md-4" Enabled="False" ID="txtTiempoNotificacionDev" />
                                            <asp:DropDownList runat="server" ID="ddlNotificacionGrupoDev" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:DropDownList runat="server" ID="ddlCanalDev" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:CheckBox runat="server" ID="chkVencimientoDev" Checked="False" Text="Despues de vencimiento" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                        <div class="form-group">
                                            <asp:CheckBox runat="server" ID="chkNotificacionOperacion" AutoPostBack="True" OnCheckedChanged="chkNotificacionOperacion_OnCheckedChanged" CssClass="col-md-12" Text="Responsable de operacion" />
                                            <asp:TextBox runat="server" Text="0" onkeypress="return ValidaCampo(this, 2)" MaxLength="3" onkeydown="return (event.keyCode!=13);" Style="width: initial" CssClass="form-control col-md-5" Enabled="False" ID="txtTiempoNotificacionOperacion" />
                                            <asp:DropDownList runat="server" ID="ddlNotificacionGrupoOperacion" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:DropDownList runat="server" ID="ddlCanalOperacion" CssClass="form-control col-md-3" Enabled="False" Style="margin-left: 5px" />
                                            <asp:CheckBox runat="server" ID="chkVencimientoOperacion" Checked="False" Text="Despues de vencimiento" CssClass="col-sm-12 col-md-12 col-lg-12" />
                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="divStep6Data" visible="False">
                                    Encuesta<br />
                                    <hr />
                                    <label class="col-lg-10">Activo</label>
                                    <asp:CheckBox runat="server" ID="chkEncuestaActiva" CssClass="chkIphone" Width="30px" Text="Activo" Checked="False" OnCheckedChanged="chkEncuestaActiva_OnCheckedChanged" AutoPostBack="True" />
                                    <asp:DropDownList runat="server" ID="ddlEncuesta" CssClass="form-control" Style="width: 100%" Enabled="False" />
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
