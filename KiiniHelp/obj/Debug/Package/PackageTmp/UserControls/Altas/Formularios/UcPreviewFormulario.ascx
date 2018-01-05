<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcPreviewFormulario" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfPreview" />
        <br>
        <h3 class="h6">
            <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink>
            / Editor de formulario / Nuevo formulario </h3>
        <hr />
        <section class="module">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-10 col-md-10 col-sm-10" />
                        <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar"/>
                        <asp:Button type="button" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar"  />
                        <br />
                        <hr />
                        <div runat="server" id="divControles">
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>

