<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusquedaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Agentes.UcBusquedaUsuario" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .RadAutoCompleteBox {
        width: 100% !important;
        padding-left: 15px !important;
        padding-right: 15px !important;
    }
    .racSlide {
        width: 100% !important;
        padding-left: 15px !important;
        padding-right: 15px !important;
    }
    
    .RadAutoCompleteBoxPopup {
        width: 100% !important;
    }
</style>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="row">
            <div class="form-horizontal">
                <div class="form-group">
                    <asp:Label runat="server" Text="Buscar usuario registrado" CssClass="col-xs-12 col-sm-12 col-md-12 col-lg-12" />
                    <tc:RadAutoCompleteBox runat="server" ID="rtxtUsuario" RenderMode="Lightweight" MinFilterLength="1" TextSettings-SelectionMode="Single" MaxResultCount="3" DataSourceID="SourceAutocomplete"
                        DataTextField="NombreCompleto" DataValueField="Id" InputType="Text">
                        <TextSettings SelectionMode="Single" ></TextSettings>
                    </tc:RadAutoCompleteBox>
                </div>
            </div>
        </div>
        
        <asp:ObjectDataSource runat="server" ID="SourceAutocomplete" TypeName="KiiniHelp.UserControls.Agentes.BusquedaAgentes" SelectMethod="GetUsers"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
