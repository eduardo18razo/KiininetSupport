<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioRapida.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioRapida" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario" />
        <asp:HiddenField runat="server" ID="hfIdTipoUsuario" Value="2" />
        <div class="row">
            <div>
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtNombreRapido" class="col-sm-12 col-md-12 col-lg-12">Nombre <span style="color: red">*</span></label>
                            <asp:TextBox class="form-control" ID="txtNombreRapido" ClientIDMode="Static" runat="server" onkeydown="return (event.keyCode!=13);"/>
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtApRapido" class="col-sm-12 col-md-12 col-lg-12">Apellido Paterno <span style="color: red">*</span></label>
                            <asp:TextBox class="form-control" ID="txtApRapido" ClientIDMode="Static" runat="server" onkeydown="return (event.keyCode!=13);"/>
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtAmRapido" class="col-sm-12 col-md-12 col-lg-12">Apellido Materno</label>
                            <asp:TextBox class="form-control" ID="txtAmRapido" ClientIDMode="Static" runat="server" onkeydown="return (event.keyCode!=13);" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label for="txtCorreoRapido" class="col-sm-12 col-md-12 col-lg-12">Correo <span style="color: red">*</span></label>
                    <asp:TextBox class="form-control" type="email" ID="txtCorreoRapido" ClientIDMode="Static" runat="server" onkeydown="return (event.keyCode!=13);" />
                </div>
                <div class="row">
                    <label for="txtCorreoRapido" class="col-sm-12 col-md-12 col-lg-12">Confirmar Correo <span style="color: red">*</span></label>
                    <asp:TextBox class="form-control" type="email" ID="txtCorreoRapidoConfirmacion" ClientIDMode="Static" runat="server" onkeydown="return (event.keyCode!=13);" autocomplete="off"/>
                </div>
                
                <div class="row" runat="server">
                    <label for="txtTelefonoCelularRapido" class="col-sm-12 col-md-12 col-lg-12">Teléfono Celular <asp:Label runat="server" ID="lblObligatorio" /></label>
                        <asp:TextBox class="form-control" ID="txtTelefonoCelularRapido" ClientIDMode="Static" runat="server" onkeypress="return (event.keyCode!=13) && ValidaCampo(this,2)" MaxLength="10" />
                </div>
           
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
