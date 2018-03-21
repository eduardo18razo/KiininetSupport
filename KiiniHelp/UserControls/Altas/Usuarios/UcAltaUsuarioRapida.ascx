<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioRapida.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioRapida" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario" />
        <asp:HiddenField runat="server" ID="hfIdTipoUsuario" />
        <div class="row">
            <div>
                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtNombre" class="col-sm-12 col-md-12 col-lg-12">Nombre</label>
                            <asp:TextBox class="form-control" ID="txtNombreRapido" ClientIDMode="Static" runat="server" />
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtAp" class="col-sm-12 col-md-12 col-lg-12">Apellido Paterno</label>
                            <asp:TextBox class="form-control" ID="txtApRapido" ClientIDMode="Static" runat="server" />
                        </div>
                        <div class="col-sm-12 col-md-4 col-lg-4">
                            <label for="txtAm" class="col-sm-12 col-md-12 col-lg-12">Apellido Materno</label>
                            <asp:TextBox class="form-control" ID="txtAmRapido" ClientIDMode="Static" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label for="txtCorreoRapido" class="col-sm-12 col-md-12 col-lg-12">Correo</label>
                    <asp:TextBox class="form-control" type="email" ID="txtCorreoRapido" ClientIDMode="Static" runat="server" />
                </div>
                
                <div class="row">
                    <label for="txtTelefonoCelularRapido" class="col-sm-12 col-md-12 col-lg-12">Teléfono (Celular)</label>
                        <asp:TextBox class="form-control" ID="txtTelefonoCelularRapido" ClientIDMode="Static" runat="server" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                </div>
           
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
