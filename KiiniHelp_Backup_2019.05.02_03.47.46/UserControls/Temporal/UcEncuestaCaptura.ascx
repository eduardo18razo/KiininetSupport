<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcEncuestaCaptura.ascx.cs" Inherits="KiiniHelp.UserControls.Temporal.UcEncuestaCaptura" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upEncuestas">
    <ContentTemplate>
        <br />
        <br />
        <section class="module">

            <asp:HiddenField runat="server" ID="hfIdEncuesta" />
            <asp:HiddenField runat="server" ID="hfIdTicket" />
            <asp:HiddenField runat="server" ID="hfIdTipoServicio" />
            <div class="row">
                <div class="row col-xs-12 col-sm-12 col-md-12 col-lg-12 ">
                    <div class="module-heading">
                        <h3 class="module-title">
                            <asp:Label runat="server" ID="lbltitulo"></asp:Label></h3>
                    </div>
                </div>
                <div class="module-inner">

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
                    <div class="module-inner text-right">
                        <asp:Button runat="server" Text="Cancelar" ID="btnCancelar" CssClass="btn btn-default" OnClick="btnCancelar_OnClick" />
                        <asp:Button runat="server" Text="Aceptar" ID="btnAceptar" CssClass="btn btn-primary" OnClick="btnAceptar_OnClick" />
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAceptar" EventName="click" />
    </Triggers>
</asp:UpdatePanel>
