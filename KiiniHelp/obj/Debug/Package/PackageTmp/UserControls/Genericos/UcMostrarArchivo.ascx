<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcMostrarArchivo.ascx.cs" Inherits="KiiniHelp.UserControls.Genericos.UcMostrarArchivo" %>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <header class="modal-header" id="pnlAlertaGeneral" runat="server" visible="false">
            <div class="alert alert-danger">
                <div>
                    <div style="float: left">
                        <asp:Image runat="server" ImageUrl="~/Images/error.jpg" />
                    </div>
                    <div style="float: left">
                        <h3>Error</h3>
                    </div>
                    <div class="clearfix clear-fix" />
                </div>
                <hr />
                <asp:Repeater runat="server" ID="rptErrorGeneral">
                    <ItemTemplate>
                        <ul>
                            <li><%# Container.DataItem %></li>
                        </ul>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>
        <div class="well">
            <asp:HiddenField runat="server" ID="hfFile" />
            <asp:HiddenField runat="server" ID="hfTipoInformacion" />

        </div>
    </ContentTemplate>
</asp:UpdatePanel>
