<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusquedaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Agentes.UcBusquedaUsuario" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 no-padding-left no-padding-right">
                <div class="form-group">
                    <asp:Label runat="server" Text="Buscar Usuario Nombre" CssClass="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left" />
                    <tc:RadAutoCompleteBox runat="server" ID="rtxtUsuario" RenderMode="Lightweight" MinFilterLength="1" TextSettings-SelectionMode="Single" MaxResultCount="3" DataSourceID="SourceAutocomplete"
                        DataTextField="NombreCompleto" DataValueField="Id" InputType="Text">
                        <TextSettings SelectionMode="Single"></TextSettings>
                    </tc:RadAutoCompleteBox>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 no-padding-left no-padding-right">
                <div class="form-group">
                    <asp:Label runat="server" Text="Buscar Usuario Correo" CssClass="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left" />
                    <tc:RadAutoCompleteBox runat="server" ID="rtxtusuarioCorreo" RenderMode="Lightweight" MinFilterLength="1" TextSettings-SelectionMode="Single" MaxResultCount="3" DataSourceID="SourceAutocomplete"
                        DataTextField="CorreoPrincipal" DataValueField="Id" InputType="Text">
                        <TextSettings SelectionMode="Single"></TextSettings>
                    </tc:RadAutoCompleteBox>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 no-padding-left no-padding-right">
                <div class="form-group">
                    <asp:Label runat="server" Text="Buscar Usuario Telefono" CssClass="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left" />
                    <tc:RadAutoCompleteBox runat="server" ID="rtxtusuarioTelefono" RenderMode="Lightweight" MinFilterLength="1" TextSettings-SelectionMode="Single" MaxResultCount="3" DataSourceID="SourceAutocomplete"
                        DataTextField="TelefonoPrincipal" DataValueField="Id" InputType="Text">
                        <TextSettings SelectionMode="Single"></TextSettings>
                    </tc:RadAutoCompleteBox>
                </div>

            </div>

        </div>
        <asp:ObjectDataSource runat="server" ID="SourceAutocomplete" TypeName="KiiniHelp.UserControls.Agentes.BusquedaAgentes" SelectMethod="GetUsers"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
