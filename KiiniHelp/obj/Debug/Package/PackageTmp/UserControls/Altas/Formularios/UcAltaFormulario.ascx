<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcAltaFormulario" %>
<%@ Register Src="~/UserControls/Altas/UcAltaCatalogo.ascx" TagPrefix="uc1" TagName="UcAltaCatalogo" %>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" Value="true" />
        <asp:UpdatePanel ID="upControlesMascara" runat="server">
            <ContentTemplate>
                <section class="module">
                    <div class="row">
                        <div class="col-lg-8 col-md-7 col-sm-7">
                            <div class="module-inner">
                                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control no-border" placeholder="Título del Formulario" MaxLength="50" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                <asp:CheckBox runat="server" ID="chkClaveRegistro" CssClass="form-control" Text="Clave de Registro" Visible="False" Checked="True" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-5 col-sm-5">
                            <div class="module-inner">
                                <asp:Button runat="server" CssClass="btn btn-default col-lg-3 col-md-3" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-primary col-lg-4 col-md-4 margin-left-5" Text="Previsualizar" ID="btnPreview" OnClick="btnPreview_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-success col-lg-3 col-md-3 margin-left-5" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8 no-padding-right">
                            <div class="module-inner no-padding-top no-padding-right">
                                <hr style="margin-top: 0; margin-bottom: 0" />
                                <asp:Repeater runat="server" ID="rptControles" OnItemDataBound="rptControles_OnItemDataBound">
                                    <HeaderTemplate>
                                        <div class="row">
                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                <div class="module-inner no-padding-left padding-20 no-padding-left" style="border-bottom: 1px solid #000000; border-bottom: 1px solid rgba(0, 0, 0, .2);">
                                                    <div class="form-group">
                                                        <asp:Label runat="server" Text="Titulo" CssClass="col-lg-5 col-md-5 col-sm-5"></asp:Label>
                                                        <asp:Label runat="server" Text="Tipo" CssClass="col-lg-4 col-md-4 col-sm-4"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="row">
                                            <asp:Label runat="server" ID="lblIdTipoCampoMascara" Text='<%# Eval("IdTipoCampoMascara") %>' Visible="False" />
                                            <asp:Label runat="server" ID="lblRequerido" Text='<%# Eval("Requerido") %>' Visible="False" />
                                            <div class="col-lg-12 col-md-12 col-sm-12">
                                                <div class="module-inner no-padding-left" style="border-bottom: 1px solid #000000; border-bottom: 1px solid rgba(0, 0, 0, .2);">
                                                    <div class="form-inline">
                                                        <asp:Label runat="server" ID="lblDescripcion" Text='<%# Eval("Descripcion") %>' CssClass="col-lg-5 col-md-5 col-sm-5"></asp:Label>
                                                        <asp:Image runat="server" ImageUrl='<%# "~/assets/images/controls/" + Eval("TipoCampoMascara.Image") %>' CssClass="col-lg-2 col-md-2 col-sm-2" Style="height: 22px; width: 38px" />
                                                        <asp:Label runat="server" Text='<%# Eval("TipoCampoMascara.Descripcion") %>' CssClass="col-lg-4 col-md-4 col-sm-4"></asp:Label>
                                                    </div>

                                                    <div class="form-inline">
                                                        <div class="form-group">
                                                            <asp:LinkButton runat="server" Text="Editar" ID="btnEditarCampo" OnClick="btnEditarCampo_OnClick" /><asp:Label runat="server" ID="lblSeparador" Text="|"></asp:Label>
                                                            <asp:LinkButton runat="server" Text="borrar" ID="btnEliminarCampo" OnClick="btnEliminarCampo_OnClick" />
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:LinkButton runat="server" ID="btnSubir" OnClick="btnSubir_OnClick" CssClass="fa fa-angle-up" />
                                                            <asp:LinkButton runat="server" ID="btnBajar" OnClick="btnBajar_OnClick" CssClass="fa fa-angle-down" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4 no-padding-left" runat="server" id="divAgregarCampos">
                            <div class="module-inner bg-grey">
                                <asp:Repeater runat="server" ID="rptTiposControles" OnItemDataBound="rptTiposControles_OnItemDataBound">
                                    <ItemTemplate>
                                        <div class="row">
                                            <asp:Image runat="server" ImageUrl='<%# "~/assets/images/controls/" + Eval("Image") %>' CssClass="col-lg-2 col-md-2 col-sm-2" Style="height: auto; width: 25px" />
                                            <asp:Label runat="server" Text='<%# Eval("Descripcion") %>' CssClass="col-lg-8 col-md-8 col-sm-8"></asp:Label>
                                            <asp:LinkButton runat="server" Text="Agregar" ID="btnAgregarControl" CommandArgument='<%#Eval("Id") %>' CssClass="col-lg-2 col-md-2 col-sm-2 btn" OnClick="btnAgregarControl_OnClick">
                                                <i class="fa fa-plus"></i>Agregar
                                            </asp:LinkButton>
                                            <br />
                                            <hr />
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </section>
            </ContentTemplate>
        </asp:UpdatePanel>
    </ContentTemplate>
</asp:UpdatePanel>
<%--Modal Alta--%>
<div class="modal fade" id="modalAgregarCampoMascara" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
    <asp:UpdatePanel ID="upAgregarCampo" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">

                    <asp:HiddenField runat="server" ID="hfAltaCampo" Value="true" />
                    <asp:HiddenField runat="server" ID="hfCampoEditado" Value="0" />
                    <asp:HiddenField runat="server" ID="hfTipoCampo" />
                    <div class="modal-header text-left">
                        <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelarModal_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <asp:Image runat="server" ID="imgTitleImage" Style="padding-bottom: 3px; padding-right: 3px;" />
                        <asp:Label runat="server" ID="lblTitleAgregarCampo" Style="font-weight: bold;" /><br />
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-lg-12 col-md-12">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblDescripcion" /><br />
                                        <asp:Label runat="server" Text="Título del campo" /><br />
                                        <asp:TextBox runat="server" ID="txtDescripcionCampo" CssClass="form-control" MaxLength="50" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:CheckBox runat="server" ID="chkRequerido" AutoPostBack="False" Text="Campo obligatorio" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12" runat="server" id="divLongitudMinima" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Longitud mínima" CssClass="col-lg-4 col-md-4 col-sm-4" />
                                        <div class="col-lg-3 col-md-3 col-sm-5">
                                            <asp:TextBox runat="server" ID="txtLongitudMinima" type="number" min="1" max="99" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12" runat="server" id="divLongitudMaxima" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Longitud máxima" CssClass="col-lg-4 col-md-4 col-sm-4" />
                                        <div class="col-lg-3 col-md-3 col-sm-5">
                                            <asp:TextBox runat="server" ID="txtLongitudMaxima" type="number" min="1" max="1000" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%-- ***** --%>
                            <div class="col-lg-12 col-md-12" runat="server" id="divCatalgo" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <div class="form-inline">
                                            <asp:Label runat="server" Text="Selecciona el catálogo que contiene las opciones que se mostrarán" Font-Bold="true" />
                                            <br />
                                            <br />
                                            <div class="form-group">
                                                Catálogos
                                                <asp:DropDownList runat="server" ID="ddlCatalogosCampo" CssClass="form-control" />
                                                <asp:Button runat="server" Text="Seleccionar" CssClass="btn btn-primary" ID="btnSeleccionarCatalogo" OnClick="btnAgregarCampo_OnClick" />
                                            </div>
                                        </div>
                                        <br />
                                        <hr />
                                        <div class="form-group">
                                            <asp:Label runat="server" Text="Agregar opciones manualmente:" Font-Bold="true"/> <%--CssClass="padding-20-bottom"--%> 
                                        </div>

                                        <uc1:UcAltaCatalogo runat="server" ID="ucAltaCatalogo" />
                                    </div>
                                </div>
                            </div>


                            <div class="col-lg-12 col-md-12" runat="server" id="divValorMaximo" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <div class="form-inline">
                                            <asp:Label runat="server" Text="Valor" CssClass="col-lg-2 col-md-2 col-sm-3" />
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="form-inline margin-top-5">
                                            <asp:Label runat="server" Text="Min." CssClass="col-lg-2 col-md-2 col-sm-3" />
                                            <div class="col-lg-3 col-md-3 col-sm-5">
                                                <asp:TextBox runat="server" ID="txtValorMinimo" type="number" min="1" max="2147483646" CssClass="form-control" onkeydown="return (event.keyCode!=13);"
                                                    Style="text-transform: none" />
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>

                                        <div class="form-inline margin-top-5">
                                            <asp:Label runat="server" Text="Max." CssClass="col-lg-2 col-md-2 col-sm-3" />
                                            <div class="col-lg-3 col-md-3 col-sm-5">
                                                <asp:TextBox runat="server" ID="txtValorMaximo" type="number" min="1" max="2147483646" step="any" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12" runat="server" id="divMascara" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Formato del campo" />
                                        <asp:TextBox runat="server" ID="txtMascara" CssClass="form-control" onkeydown="return (event.keyCode!=13);" data-toggle="tooltip" data-html="true" data-placement="right" ToolTip="None – No validation <br>Number – Number validation<br>Date – Date validation<br>Time – Time validation<br>DateTime – Date and time validation<br>Mask Characters and Delimiters<br><br>9 – Only a numeric character<br>L – Only a letter<br>$ – Only a letter or a space<br>C – Only a custom character (case sensitive)<br>A – Only a letter or a custom character<br>N – Only a numeric or custom character<br>? – Any character<br><br>/ – Date separator<br>: – Time separator<br>. – Decimal separator<br>, – Thousand separator<br>\ – Escape character<br>{ – Initial delimiter for repetition of masks<br>} – Final delimiter for repetition of masks'" Style="text-transform: none" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12 col-md-12" runat="server" id="divMoneda" visible="False">
                                <div class="module-inner">
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="Simbolo de moneda" />
                                        <asp:TextBox runat="server" ID="txtSimboloMoneda" Text="MXN" CssClass="form-control" Style="text-transform: none" onkeydown="return (event.keyCode!=13);" />
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <p class="text-right">
                                <asp:Button ID="btnAgregarCampo" runat="server" CssClass="btn btn-primary margin-top-20" Text="Agregar" OnClick="btnAgregarCampo_OnClick" />
                                <asp:Button runat="server" CssClass="btn btn-danger margin-top-20" Text="Cancelar" ID="btnLimpiarCampo" OnClick="btnLimpiarCampo_OnClick" Visible="False" />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

