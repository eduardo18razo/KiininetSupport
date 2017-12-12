<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioRapida.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioRapida" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .RadSearchBox {
        padding-left: 15px;
        padding-right: 15px;
    }
</style>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario"/>
        <asp:HiddenField runat="server" ID="hfIdTipoUsuario"/>
        <div class="row">
            <div class="form-horizontal">
                <div class="form-group">
                    <label for="txtNombre" class="col-sm-4 col-md-4 col-lg-4">Nombre</label>
                    <label for="txtAp" class="col-sm-4 col-md-4 col-lg-4">Apellido paterno</label>
                    <label for="txtAm" class="col-sm-4 col-md-4 col-lg-4">apellido materno</label>

                </div>
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
                <div class="form-group">
                    <label for="txtCorreoRapido" class="col-sm-4 col-md-4 col-lg-4" >Correo</label>
                </div>
                <div class="form-group">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <asp:TextBox class="form-control" type="emailRapido" ID="txtCorreoRapido" ClientIDMode="Static" runat="server" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="txtCorreoRapido" class="col-sm-4 col-md-4 col-lg-4">Teléfono</label>
                </div>
                <div class="form-group">
                    <tc:RadSearchBox runat="server" EnableAutoComplete="False" ShowSearchButton="False" Style="width: 100%" ID="txtTelefonoRapido" >
                        <SearchContext DataSourceID="SourceFilterPhone" DataTextField="Descripcion" DataKeyField="Id" DropDownCssClass="contextDropDown" ShowDefaultItem="False"/>
                    </tc:RadSearchBox>
                </div>
            </div>
        </div>
        <asp:ObjectDataSource runat="server" ID="SourceFilterPhone" TypeName="KiiniHelp.UserControls.Agentes.BusquedaAgentes" SelectMethod="GetPhoneType"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
