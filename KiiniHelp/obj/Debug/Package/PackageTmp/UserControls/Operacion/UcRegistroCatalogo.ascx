<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcRegistroCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcRegistroCatalogo" %>
<asp:UpdatePanel ID="upNivel" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <asp:HiddenField runat="server" ID="hfIdRegistro" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h2 class="modal-title" id="modal-new-ticket-label">
                <br />
                <asp:Label runat="server" ID="lblBranding" /></h2>
            <p class="text-center">
                <asp:Label runat="server" ID="lblTitle" />
            </p>
        </div>
        <div class="modal-body">
            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div class="bg-grey">
                            <h3 class="text-left">Editar Area</h3>
                            <hr />

                            <div class="form-group margin-top">
                                Descripcion*<br />
                                <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50" />
                            </div>
                            <p class="margin-top-40">
                                <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar" ID="btnGuardarArea" OnClick="btnGuardar_OnClick" />
                            </p>
                        </div>
                        <p class="text-right margin-top-40">
                            <asp:Button runat="server" CssClass="btn btn-success" Text="Terminar" ID="btnTerminar" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
