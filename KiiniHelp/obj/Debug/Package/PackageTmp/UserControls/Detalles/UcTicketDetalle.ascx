<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketDetalle.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcTicketDetalle" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleMascaraCaptura.ascx" TagPrefix="uc1" TagName="UcDetalleMascaraCaptura" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusTicket.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusTicket" %>
<%@ Register Src="~/UserControls/Operacion/UcCambiarEstatusAsignacion.ascx" TagPrefix="uc1" TagName="UcCambiarEstatusAsignacion" %>

<style>
    .form-control select {
        content: '<>';
    }

    .wrapperResponse {
        border: none;
        display: inline-block;
    }

    .wrapperbutton {
        position: absolute;
        bottom: 30px;
        right: 50px;
    }

    .selectConver {
        cursor: pointer;
    }

        .selectConver [type=radio] {
            display: none;
        }

            .selectConver [type=radio] + label {
                margin-left: 15px;
                padding: 6px 12px;
                cursor: pointer;
            }

            .selectConver [type=radio]:checked + label {
                padding: 6px 12px;
                border-bottom: 1px solid #6cc049;
            }
</style>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfTicketActivo" />
            <asp:HiddenField runat="server" ID="hfIdEstatusAsignacion" />
            <asp:HiddenField runat="server" ID="hfIdEstatusTicket" />
            <asp:HiddenField runat="server" ID="hfEsPropietario" />
            <asp:HiddenField runat="server" ID="hfGrupoAsignado" />
            <asp:HiddenField runat="server" ID="hfNivelAsignacion" />

            <%-- <section class="module">--%>
            <div class="row">
                <div class="col-lg-9 col-md-9 col-sm-12">

                    <section class="module">
                        <%-- <div class="row">--%>
                        <div class="module-inner">
                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-2">
                                    <span class="fa fa-envelope fa-30x padding-10 " style="" /><%--margin-top-5   border-rounded--%>
                                </div>
                                <div class="col-lg-8 col-md-6 col-sm-12">
                                    <div class="row">
                                        <label class="module-title TitulosAzul">
                                            <asp:Label runat="server" ID="lblNoticket" />:
                                                <asp:Label runat="server" ID="lblTituloTicket"></asp:Label></label>
                                        <br />
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblNombreCorreo" /><br />
                                    </div>
                                    <div class="row">
                                        <asp:Label runat="server" ID="lblFechaAlta"></asp:Label>
                                    </div>
                                </div>

                                <div class="row col-lg-3 col-md-3 col-sm-12">
                                    <div class="col-lg-4 col-md-4 col-sm-4 text-center">
                                        <div class="row">
                                            <asp:Label runat="server" Text="Prioridad" />
                                        </div>
                                        <div class="row">
                                            <asp:Image runat="server" ID="imgPrioridad" Width="29px" />
                                        </div>
                                        <div class="row">
                                            <asp:Label runat="server" ID="lblDescripcionPrioridad" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4 text-center">
                                        <div class="row">
                                            <asp:Label runat="server" Text="SLA" />
                                        </div>
                                        <div class="row">
                                            <asp:Image runat="server" ID="imgSLA" Width="29px" />
                                        </div>
                                        <div class="row">
                                            <asp:Label runat="server" ID="lblTiempoRestanteSLa" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4 text-center">
                                        <div class="row">
                                            <asp:Label runat="server" Text="Estatus" />
                                        </div>
                                        <div>
                                            <div class="text-center" style="margin: auto; width: 100%;" runat="server" id="divEstatus">
                                                <asp:Label runat="server" CssClass="btn" ID="lblEstatus" Text="Abierto" />
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>
                        <%--</div>--%>
                    </section>

                    <section class="module">
                        <div class="module-inner">

                            <div class="row padding-10-top padding-10-bottom text-right">
                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-bottom-10" ID="btnAsignar" OnClick="btnAsignar_OnClick">
                                        <i class="fa fa-long-arrow-down"></i>Cambiar Asignacion
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CssClass="btn btnManejoTickets margin-left-10 margin-bottom-10" ID="btnCambiaEstatus" OnClick="btnCambiarEstatus_OnClick">
                                        <i class="fa fa-long-arrow-right"></i>Cambiar Estatus
                                </asp:LinkButton>
                            </div>

                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-12">
                                    <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-1.png" />
                                </div>
                                <div class="col-lg-11 col-md-10 col-sm-12">

                                    <div class="row">
                                        <asp:RadioButton runat="server" ID="rbtnPublics" GroupName="tipoConversacion" AutoPostBack="True" Text="Respuesta pública" CssClass="selectConver" Checked="True" OnCheckedChanged="rbtnPublics_OnCheckedChanged" />
                                        <asp:RadioButton runat="server" ID="rbtnPrivate" GroupName="tipoConversacion" AutoPostBack="True" Text="Respuesta privada" CssClass="selectConver" OnCheckedChanged="rbtnPrivate_OnCheckedChanged" />
                                    </div>

                                    <div class="row padding-10-bottom padding-10-top" runat="server">
                                        <div class="wrapperResponse col-lg-12 col-md-12 col-sm-12">
                                            <asp:TextBox ID="txtConversacion" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control" MaxLength="999" />
                                        </div>
                                        <div class="wrapperResponse col-lg-12 col-md-12 col-sm-12 text-right padding-10-top padding-10-bottom">
                                            <asp:Button ID="btnSendPublic" runat="server" Text="Enviar" CssClass="btn btn-guardar" OnClick="btnSendPublic_OnClick" />
                                        </div>
                                    </div>

                                </div>

                            </div>

                            <hr />

                            <div class="row margin-top-5 padding-10-bottom">
                                <div class="col-lg-2 col-md-3 col-sm-3" style="border-color: #000000">
                                    <asp:DropDownList runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Historial" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionTodos" Text="Todas" CssClass="selectConver" OnCheckedChanged="rbtnConversacionTodos_OnCheckedChanged" Checked="True" />
                                <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionPublico" Text="Pública" CssClass="selectConver" OnCheckedChanged="rbtnConversacionPublico_OnCheckedChanged" />
                                <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionPrivado" Text="Interna" CssClass="selectConver" OnCheckedChanged="rbtnConversacionPrivado_OnCheckedChanged" />
                            </div>

                            <div class="row margin-top-5">
                                <asp:Repeater runat="server" ID="rptConversaciones">
                                    <ItemTemplate>
                                        <div class="row" style="border-top: 1px solid #f3f3f7; border-bottom: 1px solid #f3f3f7">
                                            <div class="col-lg-1 col-md-2 col-sm-2">
                                                <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-1.png" CssClass="padding-10-top" />
                                            </div>
                                            <div class='<%# (bool) Eval("Privado") ? "col-lg-11 col-md-10 col-sm-10 private" : "col-lg-11 col-md-10 col-sm-10 public" %>'>
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <h4>
                                                        <asp:Label runat="server" ID="lblNombre" Text='<%# Eval("Nombre") %>'></asp:Label></h4>
                                                </div>
                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Label runat="server" ID="lblFecha" Text='<%# Eval("FechaHora") %>'></asp:Label>
                                                </div>

                                                <div class="col-lg-12 col-md-12 col-sm-12">
                                                    <asp:Label runat="server" ID="lblMensaje" Text='<%# Eval("Comentario") %>'></asp:Label>
                                                </div>

                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>
                        <%--</section>

                    <section class="module">--%>
                        <div class="module-inner">
                            <div class="row margin-top-5">
                                <div class="module-inner">
                                    <uc1:UcDetalleMascaraCaptura runat="server" ID="UcDetalleMascaraCaptura" />
                                </div>
                            </div>
                        </div>
                    </section>

                </div>

                <div class="col-lg-3 col-md-3 col-sm-12 no-padding-left">
                    <section class="module">
                        <div class="module-inner">
                            <div class="row">
                                <div class="col-lg-2 col-md-12 col-sm-12">
                                    <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-square-1.png" />
                                </div>
                                <div class="col-lg-10 col-md-12 col-sm-12">
                                    <asp:Label runat="server" ID="lblNombreDetalle" Text="gabriela Vega" CssClass="TitulosAzul"></asp:Label><br />
                                    <span class="btn btn-square-usuario empleado btn-circle" style="padding-top: 6px;" ><%-- style="padding-top: 6px; width: 23px; height: 23px;"--%>
                                        <asp:Label runat="server" ID="lblTipoUsuarioDetalle"  ></asp:Label></span>
                                    <asp:Image runat="server" ImageUrl="~/assets/images/icons/vip.png" Width="25px" ID="imgVip" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Image runat="server" ImageUrl="~/assets/images/icons/ojo.png" Width="25px" CssClass="margin-right-15" />
                                    <asp:Label runat="server" Text="6 de abril 2017" ID="lblFechaUltimaconexion"></asp:Label>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <%--padding-10-top padding-10-bottom--%>
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <span class="col-lg-2 col-md-2 col-sm-2 pe-icon pe-7s-ticket icon" style="font-size: 24px"></span>
                                    <div class="col-lg-10 col-md-10 col-sm-10">
                                        <asp:DropDownList runat="server" CssClass="form-control" ID="ddlTicketUsuario" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <%-- <strong>--%>
                                    <asp:Label runat="server" Text="Puesto" CssClass="col-lg-12 col-md-12 col-sm-12" Font-Bold="true" /><%--</strong>--%>
                                    <asp:Label runat="server" ID="lblPuesto" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Correo electrónico principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblCorreoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Teléfono principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblTelefonoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row" style="display: none">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Facebook" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblFacebook" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>

                            <div class="row" style="display: none">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="twitter" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lbltwitter" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Organización" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblOrganizacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Ubicación" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblUbicacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Etiquetas" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Repeater runat="server" ID="rptEtiquetas">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEtiquetas" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Creado" CssClass="col-lg-12 col-md-12 col-sm-12" /></strong>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label runat="server" ID="lblFechaAltaDetalle" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>

                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Última Actualizacion" CssClass="col-lg-12 col-md-12 col-sm-12" /></strong>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label runat="server" ID="lblfechaUltimaActualizacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>

                        </div>
                    </section>
                </div>


            </div>
            <%--</section>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="modalAsignacionCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusAsignacion runat="server" ID="ucCambiarEstatusAsignacion" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div class="modal fade" id="modalEstatusCambio" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <uc1:UcCambiarEstatusTicket runat="server" ID="UcCambiarEstatusTicket" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
