<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcRegistroCatalogo.ascx.cs" Inherits="KiiniHelp.UserControls.Operacion.UcRegistroCatalogo" %>
<asp:UpdatePanel ID="upNivel" runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
        <asp:HiddenField runat="server" ID="hfIdCatalogo" />
        <asp:HiddenField runat="server" ID="hfIdRegistro" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>

            <h6 class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" ID="lblTitle" Text="Agregar Registro" />
            </h6>

        </div>
        <div class="modal-body">
            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div>                           
                            <div class="form-group margin-top">
                                Descripción<br />
                                <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50" />
                            </div>
                            <p class="margin-top-20">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardarArea" OnClick="btnGuardar_OnClick" />
                            </p>
                        </div>
                        <p class="text-right margin-top-20">
                            <asp:Button runat="server" CssClass="btn btn-success" Text="Terminar" ID="btnTerminar" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
