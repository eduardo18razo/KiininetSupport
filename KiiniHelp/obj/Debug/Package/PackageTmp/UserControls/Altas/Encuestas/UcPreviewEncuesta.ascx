<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewEncuesta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Encuestas.UcPreviewEncuesta" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upEncuestas">
    <ContentTemplate>
        <br/>
        <br/>
        <section class="module">

            <asp:HiddenField runat="server" ID="hfIdEncuesta" />
            <asp:HiddenField runat="server" ID="hfIdTicket" />
            <asp:HiddenField runat="server" ID="hfIdTipoServicio" />
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <div class="form-group">
                            <asp:Label runat="server" ID="lbltitulo"></asp:Label>
                        </div>
                        <hr/>

                        <div class="form-group">
                            <asp:Label runat="server" ID="lblDescripcionCliente"></asp:Label>
                        </div>
                        <div class="form-group">
                            <div runat="server" id="divControles">
                            </div>
                        </div>
                    </div>
                </div>



                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="module-inner text-center">
                            <asp:Button runat="server" Text="Cancelar" ID="btnCancelar" CssClass="btn btn-default" />
                            <asp:Button runat="server" Text="Aceptar" ID="btnAceptar" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>
