<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Encuestas.UcAltaEncuesta" %>
<asp:UpdatePanel ID="upEncuesta" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfAlta" Value="true" />
        <asp:HiddenField runat="server" ID="hfIdGrupo" />
        <asp:HiddenField runat="server" ID="hfIdEncuesta" Value="0" />
        <asp:HiddenField runat="server" ID="hfModalPadre" />
        <asp:HiddenField runat="server" ID="hfTotalPonderacion" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnClose_OnClick" runat="server" Text='&times;' />
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBrandingModal" style="font-weight:bold;" Text="Nueva Encuesta" /></h6>

        </div>
        <div class="modal-body">
            <div class="row">
                <div class="col-lg-12 col-md-12" style="margin-top: -5px;">
                    <div class="module-inner">
                        <div class="title">Tipo de encuesta</div>
                        <div class="form-group">
                            <asp:DropDownList runat="server" ID="ddlTipoEncuesta" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoEncuesta_OnSelectedIndexChanged" />
                            <%--<hr class="bordercolor" />--%>
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtTitulo" placeholder="Título de la encuesta" MaxLength="50" onkeydown="return (event.keyCode!=13);" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <b>CONFIGURACIÓN PARA EL CLIENTE</b>
                        </div>
                        <div class="form-group">
                            Asigna a esta encuesta que será visible para los clientes. Pueden agregar una breve descripción o instrucciones para facilitar su llenado.
                       
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtTituloCliente" placeholder="Titúlo de la encuesta" MaxLength="50" onkeydown="return (event.keyCode!=13);" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox runat="server" ID="txtDescripcion" placeholder="Descripción" MaxLength="250" onkeydown="return (event.keyCode!=13);" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <b>PREGUNTAS</b>
                        </div>
                        <div class="form-group">
                            Agrega las preguntas y su ponderación. Recuerda que la ponderación debe sumar 100.
                       
                        </div>
                        <div class="form-group" runat="server" id="divPreguntas">
                            <asp:Repeater runat="server" ID="rptPreguntas" OnItemDataBound="rptPreguntas_OnItemDataBound">
                                <HeaderTemplate>
                                    <div class="row">
                                        <div class="col-xs-6 col-sm-5">Pregunta</div>
                                        <div class="col-xs-6 col-sm-1">Ponderacion</div>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="margen-arriba">
                                            <div class="col-xs-6 col-md-5">
                                                <asp:TextBox runat="server" ID="txtPregunta" CssClass="form-control" Text='<%# Eval("Pregunta") %>' MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                            </div>
                                            <div class="col-xs-5 col-md-2">
                                                <asp:TextBox runat="server" ID="txtPonderacion" CssClass="form-control" Text='<%# Decimal.ToInt32(decimal.Parse(Eval("Ponderacion").ToString())) %>' MaxLength="6" onkeypress="return ValidaCampo(this,2)" onkeydown="return (event.keyCode!=13);" OnTextChanged="txtPonderacion_OnTextChanged" />
                                            </div>
                                            <asp:LinkButton runat="server" ID="btnSubir" CssClass="fa fa-angle-double-up fa-20x" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Container.ItemIndex %>' OnClick="btnSubir_OnClick" /><br />
                                            <asp:LinkButton runat="server" ID="btnBajar" CssClass="fa fa-angle-double-down fa-20x" CommandArgument='<%# Eval("Id") %>' CommandName='<%# Container.ItemIndex %>' OnClick="btnBajar_OnClick" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="row margin-top-5">
                                        <div class="col-xs-6 col-md-5">
                                            <asp:LinkButton runat="server" ID="btnagregarPregunta" CssClass="fa fa-plus-circle" OnClick="btnAddPregunta_OnClick" CommandArgument='<%# Container.ItemIndex %>'></asp:LinkButton>
                                        </div>
                                        <div class="col-xs-5 col-md-2">
                                            <asp:TextBox runat="server" ID="txtTotal" ReadOnly="True" CssClass="form-control" />
                                        </div>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="form-group" runat="server" id="divNps" visible="False">
                            <div class="col-xs-6 col-md-5">
                                <asp:TextBox runat="server" ID="txtPregunta" CssClass="form-control" Text='<%# Eval("Pregunta") %>' MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                            </div>
                        </div>
                        <div class="form-group text-right">
                            <asp:Button ID="btnLimpiarEncuesta" runat="server" CssClass="btn btn-lg btn-danger" Text="Limpiar" OnClick="btnLimpiarEncuesta_OnClick" Visible="False" />
                            <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-lg btn-danger" Text="Cancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                            <asp:Button runat="server" ID="btnPreview" Text="Previsualizar" CssClass="btn btn-default" OnClick="btnPreview_OnClick" />
                            <asp:Button runat="server" ID="btnGuardarEncuesta" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardarEncuesta_OnClick" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
