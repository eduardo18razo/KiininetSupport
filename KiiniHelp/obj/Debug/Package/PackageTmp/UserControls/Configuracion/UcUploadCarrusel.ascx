<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcUploadCarrusel.ascx.cs" Inherits="KiiniHelp.UserControls.Configuracion.UcUploadCarrusel" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<script>
    function uploadComplete() {
        $get("<%=ReloadThePanel.ClientID %>").click();
    }
</script>


<asp:UpdatePanel runat="server">
    <ContentTemplate>

        <ajax:AsyncFileUpload ID="afDosnload" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afDosnload_OnUploadedComplete" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" />

        <br />
        <div class="row">
            <asp:Repeater runat="server" ID="rptImages">
                <ItemTemplate>
                    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3">
                        <asp:LinkButton class="close" runat="server" ID="btnDelete" CommandArgument='<%# Container.DataItem %>' OnClientUploadComplete="uploadComplete" OnClick="btnDelete_OnClick"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <asp:Image CssClass="col-lg-3" runat="server" ID="img" ImageUrl='<%# Container.DataItem %>' Width="100%" Height="180px" Style="max-height: 180px" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Button ID="ReloadThePanel" runat="server" Text="hidden" OnClick="ReloadThePanel_OnClick" Style="visibility: hidden" />
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
