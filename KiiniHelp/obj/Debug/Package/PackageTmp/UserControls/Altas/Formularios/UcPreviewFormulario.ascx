<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcPreviewFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcPreviewFormulario" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfPreview" />

        <ol class="breadcrumb">
            <li class="breadcrumb-item">
                <asp:HyperLink runat="server" NavigateUrl="~/Users/DashBoard.aspx">Home</asp:HyperLink></li>
            <li class="breadcrumb-item">Help Center</li>
            <li class="breadcrumb-item">Formularios</li>
            <li class="breadcrumb-item">Nuevo Formulario</li>
            <li class="breadcrumb-item active">Previsualizar</li>

        </ol>

        <section class="module">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <div class="row">
                            <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-10 col-md-10 col-sm-10 TitulosAzul" />
                            <div class="col-lg-2 col-md-2 col-sm-2 text-right no-padding-right" >
                                <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" />
                                <asp:Button type="button" class="btn btn-success" runat="server" Text="Crear ticket" ID="btnGuardar" />
                            </div>
                        </div>
                        <hr />
                        <div runat="server" id="divControles" class="margin-top-40">
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </ContentTemplate>
</asp:UpdatePanel>

