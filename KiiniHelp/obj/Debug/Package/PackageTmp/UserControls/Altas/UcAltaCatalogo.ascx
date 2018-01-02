<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaCatalogo" %>
<asp:UpdatePanel runat="server" ID="updateAltaCatalogo">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <section class="module no-border" style="border: none">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Catálogo" CssClass="col-lg-3 col-md-3 col-sm-3"></asp:Label>
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="row col-lg-8 col-md-8 col-sm-8" style="width: 72.6%">
                                    <asp:TextBox runat="server" ID="txtDescripcionCatalogo" CssClass="form-control" placeholder="Nombre del catálogo" MaxLength="50" onkeydown="return (event.keyCode!=13);"/>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Opción de campo" CssClass="col-lg-3 col-md-3 col-sm-3"></asp:Label>
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <asp:Repeater runat="server" ID="rptRegistros">
                                    <ItemTemplate>
                                         <div class="row margin-top-5">
                                            <asp:Label runat="server" ID="lblIdRegistro" Visible="False" Text='<%# Eval("Id") %>'/>
                                            <div class="col-lg-10 col-md-10 col-sm-10">
                                                <%--<asp:TextBox runat="server" ID="txtRegistro" Text='<%# Container.DataItem.ToString() %>' CssClass="form-control" />--%>
                                                <asp:TextBox runat="server" ID="TextBox1" Text='<%# Eval("Descripcion") %>' CssClass="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);"/>
                                            </div>
                                            <asp:LinkButton runat="server" Text="Borrar" ID="btnBorrarRegistro" CommandArgument='<%# Container.ItemIndex %>' OnClick="btnBorrarRegistro_OnClick"></asp:LinkButton>
                                        </div>

                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="row margin-top-5">
                                            <div class="col-lg-8 col-md-8 col-sm-8">
                                                <asp:TextBox runat="server" ID="txtRegistroNew" CssClass="form-control" MaxLength="50" onkeydown="return (event.keyCode!=13);"/>
                                            </div>
                                            <asp:LinkButton runat="server" CssClass="fa fa-plus-circle" ID="btnAgregarRegistro" OnClick="btnAgregarRegistro_OnClick" CommandArgument='<%# Container.ItemIndex %>' />

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
                    <p class="text-right margin-top-40">
                        <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick"/>
                        <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" Visible="False" />
                        <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" Visible="False" />
                    </p>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
