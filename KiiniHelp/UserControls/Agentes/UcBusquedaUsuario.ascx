<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcBusquedaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Agentes.UcBusquedaUsuario" %>

<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .RadSearchBox {
        width: 100% !important;
       
    }

    .RadAutoCompleteBoxPopup {
        margin-bottom: 15px;
        /*background: red !important;*/
        /*width: 400px !important;*/
    }

    .racSlide {
        /*background: violet !important;*/
        width: 100px !important;
         margin-bottom: 15px;
    }


    .RadAutoCompleteBoxPopup .racList {
        /*background: blue;*/
        /*width: 90% !important;*/
    }
</style>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div class="row">
            <div class="col-lg-12 col-md-9 col-sm-9 no-padding-left no-padding-right">
                <div class="form-group">
                    <asp:Label runat="server" Text="Buscar Usuario Registrado" CssClass="col-xs-12 col-sm-12 col-md-12 col-lg-12 no-padding-left" />
                    <tc:RadAutoCompleteBox runat="server" ID="rtxtUsuario" RenderMode="Lightweight" MinFilterLength="1" TextSettings-SelectionMode="Single" MaxResultCount="3" DataSourceID="SourceAutocomplete"
                        DataTextField="NombreCompleto" DataValueField="Id" InputType="Text" Width="420px">
                        <TextSettings SelectionMode="Single"></TextSettings>
                    </tc:RadAutoCompleteBox>
                </div>
            </div>
        </div>


        <asp:ObjectDataSource runat="server" ID="SourceAutocomplete" TypeName="KiiniHelp.UserControls.Agentes.BusquedaAgentes" SelectMethod="GetUsers"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
