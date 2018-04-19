<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcFormulario" %>
<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upMascara">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdMascara" />
        <asp:HiddenField runat="server" ID="hfComandoInsertar" />
        <asp:HiddenField runat="server" ID="hfComandoActualizar" />
        <asp:HiddenField runat="server" ID="hfRandom" />
        <asp:HiddenField runat="server" ID="hfPreview" />
        <br>
        <asp:HiddenField runat="server" ID="hfTicketGenerado" />
        <asp:HiddenField runat="server" ID="hfRandomGenerado" />
        <section class="module no-border">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner no-padding-top">
                        <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-12 col-md-12 col-sm-12" />
                        <div class="text-right">
                            <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
                            <asp:Button type="button" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                        </div>
                        <br />
                        <div runat="server" id="divControles">
                        </div>
                        <div runat="server" id="divRegistraUsuario">
                            <div class="margin-bottom-10">
                                <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>




    </ContentTemplate>
</asp:UpdatePanel>

<div class="modal fade" id="modalExitoTicket" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h6 class="modal-title" id="myModalLabel">
                    <asp:Label runat="server" Text="Tu ticket se creo con éxito" />
                </h6>
            </div>
            <div class="modal-body">
                <hr />
                <p class="h4">
                    <strong>Tu no. de ticket:
                                       
                                <asp:Label runat="server" ID="lblNoTicket" /></strong><br />
                </p>
                <p class="h4">
                    <strong>Clave de registro:
                                   
                                <asp:Label runat="server" ID="lblRandom" /></strong>
                </p>
                <hr />
                En breve recibirás un correo con los datos de tu ticket para que puedas dar seguimiento.
            </div>
        </div>
    </div>
</div>
