﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaUsuarioRapida.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.Usuarios.UcAltaUsuarioRapida" %>
<%@ Register TagPrefix="tc" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<style>
    .RadSearchBox {
        padding-left: 3px;
        padding-right: 3px;
    }

    /*label {
        margin-bottom: 0px;
        padding-bottom: 0px;
    }*/
</style>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfIdUsuario" />
        <div class="row">
            <%--<div class="form-horizontal">--%>
            <div>
                <div>
                    <label for="txtNombre" class="col-sm-4 col-md-4 col-lg-4">Nombre</label>
                    <label for="txtAp" class="col-sm-4 col-md-4 col-lg-4">Apellido paterno</label>
                    <label for="txtAm" class="col-sm-4 col-md-4 col-lg-4">apellido materno</label>
                </div>

                <div class="form-group">
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:TextBox class="form-control" ID="txtNombre" ClientIDMode="Static" runat="server" />
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:TextBox class="form-control" ID="txtAp" ClientIDMode="Static" runat="server" />
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4">
                        <asp:TextBox class="form-control" ID="txtAm" ClientIDMode="Static" runat="server" />
                    </div>
                </div>
            </div>


            <div>
                <label for="txtCorreo" class="col-sm-4 col-md-4 col-lg-4">Correo</label>
            </div>
            <div class="padding-10-bottom">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:TextBox class="form-control" type="email" ID="txtCorreo" ClientIDMode="Static" runat="server" />
                </div>
            </div>


            <div>
                <label for="txtCorreo" class="col-sm-4 col-md-4 col-lg-4">Teléfono</label>
            </div>
            <div>
                <tc:RadSearchBox runat="server" EnableAutoComplete="False" ShowSearchButton="False" Style="width: 100%" ID="txtTelefono">
                    <SearchContext DataSourceID="SourceFilterPhone" DataTextField="Descripcion" DataKeyField="Id" DropDownCssClass="contextDropDown" ShowDefaultItem="False" />
                </tc:RadSearchBox>
            </div>
            <%-- </div>--%>
        </div>
        <asp:ObjectDataSource runat="server" ID="SourceFilterPhone" TypeName="KiiniHelp.UserControls.Agentes.BusquedaAgentes" SelectMethod="GetPhoneType"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
