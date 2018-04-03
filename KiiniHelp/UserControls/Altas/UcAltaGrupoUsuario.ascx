<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaGrupoUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaGrupoUsuario" %>

<asp:UpdatePanel ID="upGrupoUsuario" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdGrupo" />
        <asp:HiddenField runat="server" ID="hfFromOpcion" />
        <asp:HiddenField runat="server" ID="hfIdTipoSubGrupo" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lbltitulo" />
            </h6>
            <asp:Label runat="server" Text="Alta Grupo de Usuario" ID="lblTitle" Visible="False"></asp:Label>
        </div>

        <div class="modal-body">
            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="form-group">
                            <label>Selecciona el tipo de usuario</label>
                            <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                        </div>
                        <div class="form-group margin-top">
                            Selecciona el rol 
                            <asp:DropDownList runat="server" ID="ddlTipoGrupo" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoGrupo_OnSelectedIndexChanged" />
                        </div>
                        <div class="module-content-inner">
                            <div class="faq-section text-center margin-bottom-lg">
                                <div class="faqs-tabbed tabpanel" role="tabpanel">

                                    <div class="tab-content text-left">

                                        <div role="tabpanel" class="tab-pane tab-pane fade in active" id="tabAltaGrupo">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="form-group">
                                                            Nombre del Nuevo grupo<br />

                                                            <div class="col-sm-12 no-padding-left no-padding-right">
                                                                <asp:TextBox runat="server" ID="txtDescripcionGrupoUsuario" class="form-control col-sm-3" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <asp:CheckBox runat="server" ID="chkHabilitado" Checked="True" Visible="False" />
                                                    <div class="form-group" runat="server" id="divParametros" visible="False">
                                                        <label class="margin-top-10">
                                                        Selecciona los permisos del grupo:</label>
                                                        <asp:RadioButton runat="server" ID="rbtnLevanta" GroupName="gpoContacCenterParamtro" Text="Levanta Tickets" CssClass="col-sm-12" />
                                                        <asp:RadioButton runat="server" ID="rbtnRecado" GroupName="gpoContacCenterParamtro" Text="Levanta Recados" CssClass="col-sm-12" />
                                                    </div>

                                                    <div class="row">
                                                        <div runat="server" id="divSubRoles" visible="False">
                                                            <div class="form-group margin-top-10">
                                                                Selecciona los niveles del Grupo:<br />
                                                            </div>
                                                            <asp:HiddenField runat="server" ID="hfOperacion" />
                                                            <div class="row">
                                                                <div class="form-horizontal">
                                                                    <div class="col-lg-4 col-md-4 no-padding-left">
                                                                        Nivel
                                                                    </div>
                                                                    <div class="col-lg-4 col-md-4">
                                                                        Horario
                                                                    </div>
                                                                    <div class="col-lg-4 col-md-4">
                                                                        Días Feriados
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <asp:Repeater runat="server" ID="rptSubRoles" OnItemDataBound="rptSubRoles_OnItemDataBound">
                                                                <ItemTemplate>
                                                                    <div class="row">
                                                                        <div class="form-horizontal">
                                                                            <div class="col-lg-4 col-md-4 no-padding-left margin-top-4">
                                                                                <asp:Label runat="server" ID="lblId" Text='<%# Eval("Id") %>' Visible="False" />
                                                                                <asp:CheckBox CssClass="btn btn-seleccione btn-block hideCheck" Style="padding: 0; cursor: default" runat="server" ID="chkSubRol" value='<%# Eval("Id") %>' Text='<%# Eval("Descripcion") %>' AutoPostBack="True" OnCheckedChanged="OnCheckedChanged" />
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4">
                                                                                <asp:DropDownList runat="server" ID="ddlHorario" CssClass="form-control" Enabled="False" />
                                                                            </div>
                                                                            <div class="col-lg-4 col-md-4">
                                                                                <asp:DropDownList runat="server" ID="ddlDiasFeriados" CssClass="form-control" Enabled="False" />
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row text-right padding-20-bottom">
                                                        <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary " Text="Guardar" OnClick="btnGuardar_OnClick" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                    <asp:Label runat="server" ID="lblOperacion"></asp:Label>
                </div>
            </div>
            <asp:Button ID="btnLimpiar" runat="server" CssClass="btn btn-danger" Text="Limpiar" OnClick="btnLimpiar_OnClick" Visible="False" />
            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" Visible="False" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

