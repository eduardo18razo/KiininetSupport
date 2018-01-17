﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAsignacionUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Agentes.UcAsignacionUsuario" %>

<%@ Register Src="~/UserControls/Altas/Usuarios/UcAltaUsuarioRapida.ascx" TagPrefix="uc1" TagName="UcAltaUsuarioRapida" %>
<%@ Register Src="~/UserControls/Agentes/UcBusquedaUsuario.ascx" TagPrefix="uc1" TagName="UcBusquedaUsuario" %>


<style>
    .RadSearchBox .rsbInner input {
        width: 90% !important;
    }
</style>
<script>    
</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfSelected" ClientIDMode="Static" Value="1"/>
        <div class="modal-header">
            <asp:LinkButton CssClass="close" runat="server" OnClick="btnCancelar_OnClick" Text='&times' />
            <div class="modal-title" id="modal-new-ticket-label">
                <asp:Label runat="server" Text="Asignación de usuario" /></div>
        </div>
        <div class="modal-body" style="height:250px">
            <div class="row" style="padding-left: 15px;">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs" role="tablist">
                    <li role="presentation" class="active"><a href="#usuarioRegistrado" aria-controls="home" role="tab" data-toggle="tab" onclick='document.getElementById("hfSelected").value = 1'>Usuario Registrado</a></li>
                    <li role="presentation"><a href="#nuevoUsuario" aria-controls="profile" role="tab" data-toggle="tab" onclick='document.getElementById("hfSelected").value = 2'>Nuevo usuario</a></li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div role="tabpanel" class="tab-pane active" id="usuarioRegistrado">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <uc1:UcBusquedaUsuario runat="server" id="ucBusquedaUsuario" />
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="nuevoUsuario">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <uc1:UcAltaUsuarioRapida runat="server" ID="ucAltaUsuarioRapida" />
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="modal-footer">
            <asp:Button runat="server" ID="btnSeleccionarUsuario" style="margin-right: 16px;" Text="Asignar" CssClass="btn btn-success" OnClick="btnSeleccionarUsuario_OnClick" />
        </div>
        
    </ContentTemplate>
</asp:UpdatePanel>