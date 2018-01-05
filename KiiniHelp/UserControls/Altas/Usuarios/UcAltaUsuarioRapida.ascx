<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioRapida.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioRapida" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario" />
        <asp:HiddenField runat="server" ID="hfIdTipoUsuario" />
        <div class="row">
            <div>
                <div class="row">
                    <label for="txtNombre" class="col-sm-4 col-md-4 col-lg-4">Nombre</label>
                    <label for="txtAp" class="col-sm-4 col-md-4 col-lg-4">Apellido Paterno</label>
                    <label for="txtAm" class="col-sm-4 col-md-4 col-lg-4">Apellido Materno</label>
                </div>

                <div class="row">
                    <div class="form-group">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <asp:TextBox class="form-control" ID="txtNombreRapido" ClientIDMode="Static" runat="server" />
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <asp:TextBox class="form-control" ID="txtApRapido" ClientIDMode="Static" runat="server" />
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <asp:TextBox class="form-control" ID="txtAmRapido" ClientIDMode="Static" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <label for="txtCorreoRapido" class="col-sm-4 col-md-4 col-lg-4">Correo</label>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:TextBox class="form-control" type="emailRapido" ID="txtCorreoRapido" ClientIDMode="Static" runat="server" />
                    </div>
                </div>

                <div class="row">
                    <label for="txtTelefonoRapido" class="col-sm-4 col-md-4 col-lg-4">Teléfono (Celular)</label>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:TextBox class="form-control" ID="txtTelefonoCelularRapido" ClientIDMode="Static" runat="server" onkeypress="return ValidaCampo(this,2)" MaxLength="10" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
