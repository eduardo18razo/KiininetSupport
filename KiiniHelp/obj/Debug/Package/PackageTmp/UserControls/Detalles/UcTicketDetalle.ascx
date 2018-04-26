<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcTicketDetalle.ascx.cs" Inherits="KiiniHelp.UserControls.Detalles.UcTicketDetalle" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/Detalles/UcDetalleMascaraCaptura.ascx" TagPrefix="uc1" TagName="UcDetalleMascaraCaptura" %>

<style>
    
</style>
<script type="text/javascript">
    function OnClientEntryAdding(sender, args) {
        var path = args.get_entry().get_fullPath();
        var s = path.split("/");
        if (s.length == 1) {
            args.set_cancel(true);
        }
        else {
            sender.closeDropDown();
        }
    }
</script>

<div style="height: 100%;">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="hfTicketActivo" />
            <asp:HiddenField runat="server" ID="hfIdEstatusAsignacion" />
            <asp:HiddenField runat="server" ID="hfIdEstatusTicket" />
            <asp:HiddenField runat="server" ID="hfEsPropietario" />
            <asp:HiddenField runat="server" ID="hfGrupoAsignado" />
            <asp:HiddenField runat="server" ID="hfGrupoConSupervisor" Value="false" />
            <asp:HiddenField runat="server" ID="hfNivelAsignacion" />
            <asp:HiddenField runat="server" ID="hfAsigna" />
            <asp:HiddenField runat="server" ID="hfUsuarioLevanto" />
            <asp:HiddenField runat="server" ID="hfSubRolActual" />

            <asp:HiddenField runat="server" ID="hfIdUsuarioAsignacion" />
            <asp:HiddenField runat="server" ID="hdIdRolAsignacionPertenece" />


            <div class="row">
                <div class="col-lg-9 col-md-9 col-sm-12">

                    <section class="module">
                        <div class="module-inner">
                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom">
                                    <span class="fa fa-envelope fa-30x padding-10 " />
                                </div>
                                <div class="col-lg-5 col-md-6 col-sm-12">
                                    <div class="row">
                                        <label class="module-title titulosBandeja">
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

                                <div class="row col-lg-6 col-md-3 col-sm-12">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <table class="tableHeadTicket">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Asignación" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Agente" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Prioridad" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="SLA" /></td>
                                                <td>
                                                    <asp:Label runat="server" CssClass="fontbold" Text="Estatus" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div class="row">
                                                        <asp:Label runat="server" ID="lblAsignacion" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row ocultaTexto" style="min-width: 86px; max-width: 86px;">
                                                        <asp:Label runat="server" ID="lblAgenteAsignado" CssClass="ocultaTexto" />
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row">
                                                        <i runat="server" id="iPrioridad" class="fa fa-exclamation fontRed iconoFont margin-top-5"></i>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="row text-center">
                                                        <i runat="server" id="iSLA" class="fa fa-bomb iconoFont margin-top-5"></i>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="text-center" style="margin: auto; width: 100%; color: white; border-radius: 5px" runat="server" id="divEstatus">
                                                        <asp:Label runat="server" ID="lblEstatus" Text="Abierto" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                    <section class="module">
                        <div class="module-inner">
                            <div class="row" runat="server" id="divMovimientos">
                                <div class="">
                                    <div class="col-lg-1 col-md-1 col-sm-1 col-lg-1Custom"></div>
                                    <div>
                                        <div class="form-group col-lg-3 col-md-2 col-sm-2 minWidth225">
                                            <asp:Label runat="server" Text="Estatus" class="col-lg-12 col-md-12 col-sm-12 no-padding-left widht235"/>
                                            <div class="col-lg-12 col-md-12 col-sm-12 no-padding-left">
                                                <asp:DropDownList runat="server" CssClass="form-control widht200" ID="ddlCambiarEstatus" />
                                            </div>
                                        </div>

                                        <div class="form-group col-lg-3 col-md-2 col-sm-2 minWidth225">
                                            <asp:Label runat="server" Text="Asignación" class="col-lg-12 col-md-12 col-sm-12 no-padding-left widht235"/>
                                            <div class="col-lg-12 col-md-12 col-sm-12 no-padding-left widht235">
                                                <asp:DropDownList runat="server" CssClass="form-control widht200" ID="ddlCambiarAsignar" AutoPostBack="True" OnSelectedIndexChanged="ddlCambiarAsignar_OnSelectedIndexChanged" Style="width: 120px; display: inline-block" />
                                                <asp:LinkButton runat="server" ID="lnkBtndeshacer" CssClass="icon-cancel" OnClick="lnkBtndeshacer_OnClick" />
                                            </div>

                                        </div>
                                        <div class="form-group col-lg-3 col-md-3 col-sm-3 minWidth225">
                                            <div class="form-group" runat="server" id="divUsuariosAsignacion" visible="False">
                                                <asp:Label runat="server" Text="Usuario" class="col-lg-12 col-md-12 col-sm-12 no-padding-left widht235"/>
                                                <div class="col-lg-12 col-md-12 col-sm-12 no-padding-left">
                                                    <tc:RadDropDownTree runat="server" CssClass="form-control no-border no-padding-left widht200" ID="ddlUsuarioAsignacion" AutoPostBack="True" RenderMode="Lightweight" ExpandNodeOnSingleClick="true" CheckNodeOnClick="False"
                                                        DefaultMessage="-" EnableFiltering="True" OnClientEntryAdding="OnClientEntryAdding" OnEntriesAdded="ddlUsuarioAsignacion_OnEntriesAdded">
                                                        <DropDownSettings CloseDropDownOnSelection="True" />
                                                        <ButtonSettings ShowClear="False"></ButtonSettings>
                                                        <FilterSettings Filter="Contains" Highlight="Matches" EmptyMessage="Escribir nombre" FilterTemplate="ByContent"></FilterSettings>
                                                    </tc:RadDropDownTree>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-1 col-md-2 col-sm-12 col-lg-1Custom">
                                    <asp:Image runat="server" ID="imgProfileNewComment" CssClass="imageCircle image40" ImageUrl="~/assets/images/profiles/profile-1.png" />
                                </div>
                                <div class="col-lg-11 col-md-10 col-sm-12">

                                    <div class="row no-padding-bottom">
                                        <asp:RadioButton runat="server" ID="rbtnPublics" GroupName="tipoConversacion" AutoPostBack="True" Text="Respuesta pública" CssClass="selectConver" Checked="True" OnCheckedChanged="rbtnPublics_OnCheckedChanged" />
                                        <asp:RadioButton runat="server" ID="rbtnPrivate" GroupName="tipoConversacion" AutoPostBack="True" Text="Respuesta privada" CssClass="selectConver" Style="margin-bottom: 0" OnCheckedChanged="rbtnPrivate_OnCheckedChanged" />
                                    </div>

                                    <div class="row no-padding-top margin-top0" runat="server">
                                        <div class="wrapperResponse col-lg-12 col-md-12 col-sm-12 no-padding-top margin-top0">
                                            <asp:TextBox ID="txtConversacion" runat="server" TextMode="MultiLine" Rows="10" CssClass="form-control no-padding-top no-margin-top" MaxLength="999" />
                                        </div>
                                        <div class="wrapperResponse col-lg-12 col-md-12 col-sm-12 text-right padding-10-top padding-10-bottom">
                                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-guardar" OnClick="btnEnviar_OnClick" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <hr />

                            <div class="row margin-top-5 padding-10-bottom">
                                <div class="col-lg-2 col-md-3 col-sm-3" style="border-color: #000000">
                                    <asp:DropDownList runat="server" AutoPostBack="True" CssClass="form-control" ID="ddlHistorial" OnSelectedIndexChanged="ddlHistorial_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Historial" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Eventos" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div runat="server" id="divtabHistorial">
                                    <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionTodos" Text="Todas" CssClass="selectConver" OnCheckedChanged="rbtnConversacionTodos_OnCheckedChanged" Checked="True" />
                                    <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionPublico" Text="Pública" CssClass="selectConver" OnCheckedChanged="rbtnConversacionPublico_OnCheckedChanged" />
                                    <asp:RadioButton runat="server" GroupName="tipoConversacionPreview" AutoPostBack="True" ID="rbtnConversacionPrivado" Text="Interna" CssClass="selectConver" OnCheckedChanged="rbtnConversacionPrivado_OnCheckedChanged" />
                                </div>
                            </div>

                            <div class="row margin-top-5" runat="server" id="divHistorial">
                                <asp:Repeater runat="server" ID="rptConversaciones" OnItemDataBound="rptConversaciones_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="row" style="border-top: 1px solid #f3f3f7; border-bottom: 1px solid #f3f3f7">
                                            <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom">
                                                <asp:Image runat="server" ImageUrl='<%# Eval("Foto") != null ? "~/DisplayImages.ashx?id=" + Eval("IdUsuario") : "~/assets/images/profiles/profile-1.png"%>' ID="imgAgente" CssClass="imageCircle image40" />
                                            </div>
                                            <div class='<%# (bool) Eval("Privado") ? "col-lg-11 col-md-10 col-sm-10 private" : "col-lg-11 col-md-10 col-sm-10 public" %>'>
                                                <div class="col-lg-12 col-md-12 col-sm-12 no-margin-bottom no-padding-bottom">
                                                    <h4 class="no-margin-bottom no-padding-bottom titulosBandeja14">
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

                            <div class="row margin-top-5" runat="server" id="divEventos" visible="False">
                                <asp:Repeater runat="server" ID="rptEventos" OnItemDataBound="rptEventos_OnItemDataBound">
                                    <ItemTemplate>
                                        <div class="row" style="border-top: 1px solid #f3f3f7; border-bottom: 1px solid #f3f3f7">
                                            <div class="col-lg-2 col-md-2 col-sm-2 col-lg-1Custom ">
                                                <asp:Image runat="server" ImageUrl='<%# Eval("Foto") != null ? "~/DisplayImages.ashx?id=" + Eval("IdUsuarioGenero") : "~/assets/images/profiles/profile-1.png"%>' ID="imgAgente" CssClass="imageCircle image40" />
                                            </div>
                                            <div class="col-lg-11 col-md-12 col-sm-12">
                                                <h4 class="no-margin-bottom no-padding-bottom titulosBandeja14">
                                                    <asp:Label runat="server" ID="lblNombre" Text='<%# Eval("NombreUsuario") %>'></asp:Label></h4>
                                            </div>

                                            <div class="col-lg-11 col-md-10 col-sm-10 col-lg-offset-1Custom">
                                                <asp:Label runat="server" ID="lblFecha" Text='<%# Eval("FechaHoraEventoFormato") %>'></asp:Label>
                                            </div>
                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                <asp:Repeater runat="server" ID="rptMovimientos">
                                                    <ItemTemplate>
                                                        <div runat="server" visible='<%# Eval("EsMovimientoConversacion") %>' class='<%# (bool) Eval("ComentarioPublico") ? "col-lg-11 col-md-10 col-sm-10 col-lg-offset-1Custom private" : "col-lg-11 col-md-10 col-sm-10 col-lg-offset-1Custom public" %>'>
                                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                                <asp:Label runat="server" ID="Label5" Text='<%# Eval("Conversacion") %>'></asp:Label>
                                                            </div>
                                                        </div>
                                                        
                                                        <div runat="server" visible='<%# Eval("EsMovimientoEstatusTicket") %>' class="col-lg-11 col-md-10 col-sm-10 gray margin-top-10 col-lg-offset-1Custom">
                                                            <div class="col-lg-11 col-md-12 col-sm-12">
                                                                <asp:Label runat="server" Text="Estatus" CssClass="fontbold"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("DescripcionEstatus") %>' />
                                                                <asp:Label runat="server" Text='<%# Eval("DescripcionEstatusAnterior") %>' CssClass="text-decoration-line-through" />
                                                            </div>
                                                            
                                                            <div class="col-lg-11 col-md-12 col-sm-12">
                                                                <asp:Label runat="server" Text="Usuario asignado" CssClass="fontbold" />
                                                                <asp:Label runat="server" Text='<%# Eval("NombreCambioEstatus") %>' />
                                                            </div>
                                                        </div>

                                                        <div runat="server" visible='<%# Eval("EsMovimientoAsignacion") %>' class="col-lg-11 col-md-10 col-sm-10 gray margin-top-10 col-lg-offset-1Custom">

                                                            <div class="col-lg-11 col-md-12 col-sm-12 ">
                                                                <asp:Label runat="server" Text="Asignación" CssClass="fontbold" />
                                                                <asp:Label runat="server" Text='<%# Eval("DescripcionEstatus") %>' />
                                                                <asp:Label runat="server" Text='<%# Eval("DescripcionEstatusAnterior") %>' CssClass="text-decoration-line-through" />
                                                            </div>
                                                            
                                                            <div class="col-lg-11 col-md-12 col-sm-12">
                                                                <asp:Label runat="server" Text="Usuario asignado"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("NombreUsuarioAsignado") %>'></asp:Label>
                                                            </div>
                                                            <div class="col-lg-11 col-md-12 col-sm-12" runat="server">
                                                                <asp:Label runat="server" Text="Comentario" CssClass="col-lg-4 col-md-4 col-sm-4"></asp:Label>
                                                                <asp:Label runat="server" Text='<%# Eval("Comentarios") %>' CssClass="col-lg-4 col-md-4 col-sm-4" />
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="row">
                                <div class="row" style="border-bottom: 1px solid #f3f3f7">
                                    <div class="col-lg-1 col-md-2 col-sm-2 col-lg-1Custom ">
                                        <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-1.png" ID="imgUsuarioTicket" CssClass="imageCircle image40" />
                                    </div>
                                    <div class="col-lg-11 col-md-10 col-sm-10">
                                        <div class="col-lg-12 col-md-12 col-sm-12 no-margin-bottom no-padding-bottom">
                                            <h4 class="no-margin-bottom no-padding-bottom titulosBandeja14">
                                                <asp:Label runat="server" ID="lblNombreU" Text="" /></h4>
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <asp:Label runat="server" ID="lblFecha" Text='<%# Eval("FechaHora") %>' />
                                        </div>
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <uc1:UcDetalleMascaraCaptura runat="server" ID="UcDetalleMascaraCaptura" />
                                        </div>
                                    </div>
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
                                    <asp:Image runat="server" ImageUrl="~/assets/images/profiles/profile-square-1.png" CssClass="imageCircle image40"
                                        ID="imgUsuarioDetalle" />
                                </div>
                                <div class="col-lg-10 col-md-12 col-sm-12">
                                    <div class="col-lg-12 col-md-12 col-sm-12">
                                        <asp:Label runat="server" ID="lblNombreDetalle" Text="" CssClass="titulosBandeja" /><br />
                                    </div>
                                    <div class="col-lg-12 col-md-12 col-sm-12 no-padding-top">
                                        <span class="btn btn-default-alt btn-square-usuario" style="margin-left: 2px; top: 0;" runat="server" ID="spanTu">
                                            <asp:Label runat="server" ID="lblTipoUsuarioDetalle" CssClass="lbl" />
                                        </span>
                                        <i id="iVip" runat="server" class="fa fa-star iconoFont20" style="margin-top: 1px; margin-left: 10px; vertical-align: middle"></i>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <label class="col-lg-2 col-md-2 col-sm-2 icon-eye margin-top-7" style="font-size: 24px"></label>
                                    <div class="col-lg-10 col-md-10 col-sm-10 margin-top-10">
                                        <asp:Label runat="server" Text="6 de abril 2017" ID="lblFechaUltimaconexion" />
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <label class="col-lg-2 col-md-2 col-sm-2 icon-ticket margin-top-7" style="font-size: 24px"></label>
                                    <div class="col-lg-10 col-md-10 col-sm-10">

                                        <tc:RadDropDownList runat="server" ID="rddConcentradoTicketsUsuario" DataTextField="Tipificacion" DropDownWidth="190px" AutoPostBack="True" OnSelectedIndexChanged="rddConcentradoTicketsUsuario_OnSelectedIndexChanged">
                                            <ItemTemplate>
                                                <div>
                                                    <div class="margin-top-10" style="font-size: 11px;">
                                                        <asp:Label runat="server" Text='<%# Eval("IdTicket").ToString().Trim() == "0" ? string.Empty : Eval("IdTicket") %>' ID="lblIdTicket" />
                                                        <asp:Label runat="server" Text="-" Width="10px" CssClass="text-center" Visible='<%# Eval("IdTicket").ToString().Trim() != "0"%>' />
                                                        <asp:Label runat="server" Text='<%# Eval("Tipificacion") %>' Font-Bold="true" ID="lblTitulo" />
                                                    </div>
                                                    <div class="meta margin-top-5 borderbootom " style="font-size: 11px">
                                                        <asp:Label runat="server" ID="lblEstatusTicketConcentrado" Text='<%# Eval("DescripcionEstatusTicket") %>' />
                                                        <asp:Label runat="server" ID="Label1" Text=" - " />
                                                        <asp:Label runat="server" Text='<%#Eval("FechaCreacionFormato") %>' CssClass="text-right margin-bottom-10" />
                                                        <asp:Label runat="server" ID="lblAcceso" Text='<%#Eval("PuedeVer") %>' Visible="False" />
                                                        <asp:Label runat="server" ID="lblAsigna" Text='<%#Eval("PuedeVer") %>' Visible="False" />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </tc:RadDropDownList>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12 heigth40px">
                                    <asp:Label runat="server" Text="Puesto" CssClass="col-lg-12 col-md-12 col-sm-12" Font-Bold="true" /><%--</strong>--%>
                                    <asp:Label runat="server" ID="lblPuesto" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Correo electrónico principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblCorreoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Teléfono principal" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblTelefonoPrincipal" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row ocultar heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Facebook" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblFacebook" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>

                            <div class="row ocultar heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="twitter" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lbltwitter" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>

                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Organización" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblOrganizacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Ubicación" CssClass="col-lg-12 col-md-12 col-sm-12"></asp:Label></strong>
                                    <asp:Label runat="server" ID="lblUbicacion" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
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
                            <div class="row heigth45px">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <strong>
                                        <asp:Label runat="server" Text="Creado" CssClass="col-lg-12 col-md-12 col-sm-12" /></strong>
                                </div>
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Label runat="server" ID="lblFechaAltaDetalle" CssClass="col-lg-12 col-md-12 col-sm-12" />
                                </div>
                            </div>
                            <hr />
                            <div class="row heigth45px">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="modal fade" id="modalComentarioObligado" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCerrarModalComentarios_OnClick" Text='&times' />
                            <h6 class="modal-title">
                                <asp:Label runat="server" ID="lblTitleCatalogo" />
                            </h6>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <asp:Label runat="server" Text="Comentarios" CssClass="col-lg-3 margin-top-9" />
                                <div class="col-lg-9 no-padding-right">
                                    <asp:TextBox runat="server" ID="txtComentarioAsignacion" CssClass="form-control" TextMode="MultiLine" Height="100px" />
                                </div>
                            </div>
                        </div>
                        <div class="panel-footer">
                            <asp:Button runat="server" ID="btnCerrarComentarios" Text="Aceptar" CssClass="btn btn-guardar" OnClick="btnCerrarComentarios_OnClick" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
