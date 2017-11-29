<%@ Page Title="Mi Actividad" Language="C#" MasterPageFile="~/Usuarios.Master" AutoEventWireup="true" CodeBehind="FrmMiInformacion.aspx.cs" Inherits="KiiniHelp.Users.General.FrmMiInformacion" %>

<%@ Register Src="~/UserControls/Detalles/UcDetalleUsuario.ascx" TagPrefix="uc1" TagName="UcDetalleUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $('#myTabs a').click(function (e) {
            e.preventDefault();
            $(this).tab('show');
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <uc1:UcDetalleUsuario runat="server" ID="UcDetalleUsuario" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<div id="exTab2" class="container">
        <ul class="nav nav-tabs">
            <li class="active">
                <a href="#1" data-toggle="tab">Mis tickets</a>
            </li>
            <li>
                <a href="#2" data-toggle="tab">Perfil</a>
            </li>
            <li>
                <a href="#3" data-toggle="tab">Encuestas Pendientes</a>
            </li>
            
        </ul>

        <div class="tab-content " style="background: #fff">
            <div class="tab-pane " id="1">
                <h6>
                    <uc1:UcConsultaTicketUsuario runat="server" ID="UcConsultaTicketUsuario" />
                </h6>
            </div>
            <div class="tab-pane active" id="2">
                <h6>
                    
                </h6>
            </div>
            
            <div class="tab-pane" id="3">
                <h6>
                    <uc1:UcConsultaEncuestaPendiente runat="server" ID="UcConsultaEncuestaPendiente" />
                </h6>
            </div>

        </div>
    </div>--%>
</asp:Content>
