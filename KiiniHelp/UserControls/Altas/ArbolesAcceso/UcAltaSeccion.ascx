﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaSeccion.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.ArbolesAcceso.UcAltaSeccion" %>

<asp:UpdatePanel runat="server" ID="upCatlogos">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdArbol" />
        <asp:HiddenField runat="server" ID="hfCatalogo" />
        <asp:HiddenField runat="server" ID="hfAlta" />
        <asp:HiddenField runat="server" ID="hfArbolSeleccionado" />

        <div class="modal-header">
            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCancelarCatalogo_OnClick" Text='&times' />
            <h6 class="modal-title">
                <asp:Label runat="server" ID="lblTitleCatalogo" />
            </h6>
        </div>

        <div class="modal-body">
            <!-- TIPO DE USUARIO-->
            <div class="row">
                <div class="form-group">
                    <label class="col-lg-12 col-md-12 col-sm-12 col-xs-12">Selecciona el tipo de usuario que corresponde a esta Organización</label><br />
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 no-margin-left">
                        <asp:DropDownList runat="server" ID="ddlTipoUsuario" CssClass="form-control" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlTipoUsuario_OnSelectedIndexChanged" />
                    </div>
                </div>
            </div>

            <!--CONTAINER IZQUIERDA-->
            <div class="row" runat="server" id="divData" visible="False">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <label>
                        Crea o selecciona de mayor a menor, los niveles que contiene la Organización
                    </label>
                </div>
                <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                    <!--Step 1-->
                    <div class="margin-bottom-5" runat="server" id="divStep1">
                        <asp:HiddenField runat="server" ID="hfNivel1" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel1" CssClass="labelFor" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel1" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel1" CommandArgument="1" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel1" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 2-->
                    <div class="margin-bottom-5" runat="server" id="divStep2" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel2" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel2" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel2" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel2" CommandArgument="2" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel2" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 3-->
                    <div class="margin-bottom-5" runat="server" id="divStep3" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel3" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel3" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel3" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel3" CommandArgument="3" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel3" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 4-->
                    <div class="margin-bottom-5" runat="server" id="divStep4" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel4" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel4" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel4" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel4" CommandArgument="4" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel4" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 5-->
                    <div class="margin-bottom-5" runat="server" id="divStep5" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel5" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel5" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel5" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel5" CommandArgument="5" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel5" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 6-->
                    <div class="margin-bottom-5" runat="server" id="divStep6" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel6" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel6" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel6" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel6" CommandArgument="6" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel6" />
                        </asp:LinkButton>
                    </div>

                    <!--Step 7-->
                    <div class="margin-bottom-5" runat="server" id="divStep7" visible="False">
                        <asp:HiddenField runat="server" ID="hfNivel7" />
                        <label>
                            <asp:Label runat="server" ID="lblAliasNivel7" />
                            <i class="fa fa-check text-success" aria-hidden="true" runat="server" id="succNivel7" visible="False"></i>
                        </label>
                        <br />
                        <asp:LinkButton runat="server" CssClass="btn btn-seleccione btn-square" ID="btnStatusNivel7" CommandArgument="7" OnClick="btnStatusNivel1_OnClick">
                            <asp:Label runat="server" ID="lblStepNivel7" />
                        </asp:LinkButton>
                    </div>
                </div>
                <!--/CONTAINER IZQUIERDA-->
                <!--CONTAINER DERECHA-->

                <!--Filtro 1 ORGANIZACIÓN-->
                <div>
                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12 ">
                        <section class="module no-margin-right margin-left-10" runat="server" id="seleccionar">
                            <div class="bg-grey">
                                <h4 class="text-left">
                                    <asp:Label runat="server" Text="" /><asp:Label runat="server" ID="lblOperacion" /></h4>
                                <hr />
                                <label>Selecciona existente</label>
                                <!--CAMPO-->
                                <div class="form-group">
                                    <asp:DropDownList runat="server" CssClass="form-control" ID="ddlNivelSeleccionModal" AutoPostBack="True" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlNivelSeleccionModal_OnSelectedIndexChanged" />
                                </div>

                                <!--CAMPO-->
                                <p class="margin-top-20  text-center">
                                    <asp:Button class="btn btn-primary" runat="server" Text="Siguiente" ID="btnSeleccionarModal" OnClick="btnSeleccionarModal_OnClick" />
                                    <asp:Button CssClass="btn btn-success" ID="btnTerminar" runat="server" Text="Terminar" OnClick="btnTerminar_OnClick"></asp:Button>
                                </p>
                            </div>
                        </section>

                        <br />
                        <div runat="server" id="pnlAlta" visible="true">
                            <section class="module no-margin-right margin-left-10">
                                <div class="bg-grey">
                                    <div  class="row">
                                        <div runat="server" id="divClas"></div>
                                        <!--CAMPO-->
                                        <h4>
                                            <asp:Label runat="server" Text="Crea Nuevo:" ID="lblAccion" />
                                        </h4>
                                        <hr />
                                        <div class="form-group margin-top">
                                            <label>
                                                Nombre de
                                            <asp:Label runat="server" ID="lblOperacionDescripcion" />*<br />
                                            </label>
                                            <p>
                                                <asp:TextBox CssClass="form-control no-margin-left" ID="txtDescripcionCatalogo" runat="server" onkeydown="return (event.keyCode!=13 && event.keyCode!=27);" MaxLength="50" autofocus="autofocus" />
                                                <asp:LinkButton runat="server" OnClick="btnGuardarCatalogo_OnClick" ID="btnGuardarCatalogo" class="fa fa-plus-circle col-lg-1 col-md-1 col-sm-2 col-xs-2 col-lg-offset-11 col-md-offset-11 text-right no-padding-right" />
                                            </p>
                                            <p class="text-right">
                                                <asp:LinkButton runat="server" ID="btnGuardar" Text="Guardar" CssClass="text-right btn btn-guardar" OnClick="btnGuardar_Click" />
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </section>
                        </div>

                    </div>
                </div>
                <!--/Filtro 1 ORGANIZACIÓN-->
                <!--/CONTAINER DERECHA-->
            </div>
        </div>
        <asp:CheckBox runat="server" ID="chkHabilitado" Visible="False" Checked="True" />

    </ContentTemplate>
</asp:UpdatePanel>
