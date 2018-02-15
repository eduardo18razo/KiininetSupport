<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaCatalogo" %>
<asp:UpdatePanel runat="server" ID="updateAltaCatalogo">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <section class="module no-border no-margin-right no-margin-bottom" style="border: none">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Nombre del catálogo" CssClass="col-lg-3 col-md-3 col-sm-3 no-padding-left no-padding-right padding-10-top" />
                        <div class="col-lg-6 col-md-6 col-sm-6 padding-10-left padding-31-right">
                            <asp:TextBox runat="server" ID="txtDescripcionCatalogo" CssClass="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Opción:" CssClass="col-lg-3 col-md-3 col-sm-3 no-padding-left no-padding-right padding-10-top"></asp:Label>
                        <div class="col-lg-9 col-md-9 col-sm-9 no-padding-left">
                            <asp:Repeater runat="server" ID="rptRegistros">
                                <ItemTemplate>
                                    <div class="row">
                                        <div class="col-lg-9 col-md-9 col-sm-9 padding-10-left no-padding-top">
                                            <asp:Label runat="server" ID="lblIdRegistro" Visible="False" Text='<%# Eval("Id") %>' />
                                            <div class="col-lg-10 col-md-10 col-sm-10 no-padding-left no-padding-top">
                                                <asp:TextBox runat="server" ID="TextBox1" Text='<%# Eval("Descripcion") %>' CssClass="form-control" Width="95%" MaxLength="50" onkeydown="return (event.keyCode!=13);" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2 no-padding">
                                                <asp:LinkButton runat="server" CssClass="lnkBtn margin-top-9 col-lg-3 col-md-3 col-sm-3" Text="Borrar" ID="btnBorrarRegistro" CommandArgument='<%# Container.ItemIndex %>' OnClick="btnBorrarRegistro_OnClick" />
                                            </div>
                                        </div>
                                    </div>

                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="row">
                                        <div class="col-lg-9 col-md-9 col-sm-9 padding-10-left no-padding-top">
                                            <div class="col-lg-10 col-md-10 col-sm-10 no-padding-left no-padding-top">
                                                <asp:TextBox runat="server" ID="txtRegistroNew" CssClass="form-control" MaxLength="50" Width="95%" onkeydown="return (event.keyCode!=13);" />
                                            </div>
                                            <div class="col-lg-2 col-md-2 col-sm-2 no-padding">
                                                <asp:LinkButton runat="server" CssClass="fa fa-plus-circle margin-top-6" ID="btnAgregarRegistro" OnClick="btnAgregarRegistro_OnClick" CommandArgument='<%# Container.ItemIndex %>' />
                                            </div>
                                        </div>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <p class="text-right no-padding-right">
                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                        <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" Visible="False" />
                        <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                    </p>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
