<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaCatalogos.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Catalogo.UcAltaCatalogos" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:UpdatePanel runat="server" ID="updateAltaCatalogo">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server" Text='&times;' />
            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblBrandingModal" /></h6>
        </div>

        <div class="modal-body">
            <section class="module no-border">
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div>
                            <asp:Label runat="server" Text="Nombre del Catálogo"></asp:Label>
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtNombreCatalogo" MaxLength="50" onkeydown="return (event.keyCode!=13);" CssClass="form-control"></asp:TextBox>
                            </div>

                            <asp:Label runat="server" Text="Descripción del Catálogo"></asp:Label>
                            <div class="form-group">
                                <asp:TextBox runat="server" ID="txtDescripcionCatalogo" TextMode="MultiLine" Rows="3" MaxLength="250" onkeydown="return (event.keyCode!=13);" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="row form-group">
                                <asp:RadioButton runat="server" Text="Agregar registros manualmente" AutoPostBack="True" GroupName="TipoCatalogo" ID="rbtnManual" OnCheckedChanged="rbtnTipoCatalogo_OnCheckedChanged" />
                            </div>

                            <div class="form-group" runat="server" id="divManual" visible="False">
                                <asp:Label runat="server" Text="Opción de campo" CssClass="col-lg-4 col-md-4 col-sm-4 no-padding-top margin-top-20" />
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:Repeater runat="server" ID="rptRegistros">
                                        <ItemTemplate>
                                            <div class="row margin-top-5">
                                                <asp:Label runat="server" ID="lblIdRegistro" Visible="False" Text='<%# Eval("Id") %>' />
                                                <div class="col-lg-10 col-md-10 col-sm-10">
                                                    <asp:TextBox runat="server" ID="txtDescripcionRegistro" Text='<%# Eval("Descripcion") %>' CssClass="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                                </div>
                                                <div class="col-lg-2 col-md-2 col-sm-2 padding-12-top">
                                                    <asp:LinkButton runat="server" Text="Borrar" ID="btnBorrarRegistro" CommandArgument='<%# Container.ItemIndex %>' OnClick="btnBorrarRegistro_OnClick"></asp:LinkButton>
                                                </div>
                                            </div>

                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <div class="row margin-top-5">
                                                <div class="col-lg-10 col-md-10 col-sm-10">
                                                    <asp:TextBox runat="server" ID="txtRegistroNew" CssClass="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                                </div>
                                                <div class="col-lg-2 col-md-2 col-sm-2">
                                                    <asp:LinkButton runat="server" CssClass="fa fa-plus-circle margin-top-6" ID="btnAgregarRegistro" OnClick="btnAgregarRegistro_OnClick" CommandArgument='<%# Container.ItemIndex %>' />
                                                </div>
                                            </div>
                                        </FooterTemplate>
                                    </asp:Repeater>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div> <%--class="module-inner"--%>
                            <div class="row form-group padding-10-bottom">
                                <asp:RadioButton runat="server" Text="Agregar campos desde archivo" AutoPostBack="True" GroupName="TipoCatalogo" ID="rbtnArchivo" OnCheckedChanged="rbtnTipoCatalogo_OnCheckedChanged" />
                            </div>
                            <div runat="server" id="divArchivo" visible="False">
                                <div class="form-group">
                                    <asp:HiddenField runat="server" ID="hfFileName" />
                                    <ajax:AsyncFileUpload ID="afuArchivo" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afuArchivo_OnUploadedComplete" PersistFile="True" />
                                </div>
                                <div class="form-group">
                                    <asp:LinkButton runat="server" Text="Obtener páginas" ID="btnLeerArchivo" OnClick="btnLeerArchivo_OnClick"></asp:LinkButton>
                                </div>

                                <div class="form-group">
                                    <asp:RadioButtonList runat="server" ID="rbtnHojas" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <p class="text-right">
                            <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                            <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" Visible="False" />
                            <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                        </p>
                    </div>
                </div>
            </section>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
