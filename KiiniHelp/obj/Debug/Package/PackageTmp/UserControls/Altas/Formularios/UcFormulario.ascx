﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcFormulario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Formularios.UcFormulario" %>
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
            / Captura formulario </h3>
        <hr />
        <asp:HiddenField runat="server" ID="hfTicketGenerado"/>  
        <asp:HiddenField runat="server" ID="hfRandomGenerado"/> 
        <section class="module">
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="module-inner">
                        <asp:Label runat="server" ID="lblDescripcionMascara" class="col-lg-10 col-md-10 col-sm-10" />
                        <asp:Button type="button" class="btn btn-default" runat="server" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick"/>
                        <asp:Button type="button" class="btn btn-primary" runat="server" Text="Crear ticket" ID="btnGuardar"  OnClick="btnGuardar_OnClick"/>
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

<div class="modal fade" id="modalExitoTicket" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <asp:LinkButton class="close" runat="server" ID="btnCerrarExito" OnClick="btnCerrarExito_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                <h3 class="modal-title" id="myModalLabel">
                   <img class="img-responsive margin-left" src="assets/images/icons/ok.png" alt="" /><br> 
                    Tu ticket se creo con éxito</h3>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <hr />
                        <p class="h4">
                            <strong>Tu no. de ticket:
                                        <asp:Label runat="server" ID="lblNoTicket" /></strong><br>
                        </p>
                        <p class="h4">
                            <strong>Clave de registro:
                                    <asp:Label runat="server" ID="lblRandom" /></strong>
                        </p>
                        <hr />
                        En breve recibirás un correo con los datos de tu ticket para que puedas dar seguimiento.
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer"></div>
        </div>
    </div>
</div>

