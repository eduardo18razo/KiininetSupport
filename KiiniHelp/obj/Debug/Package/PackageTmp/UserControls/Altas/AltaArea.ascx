<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaArea.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaArea" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=18.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfEsAlta" />
                    <asp:HiddenField runat="server" ID="hfIdArea" />
                    <asp:HiddenField runat="server" ID="hfFileName" />
        <div class="modal-header">
            <asp:LinkButton class="close" ID="btnClose" OnClick="btnCancelar_OnClick" runat="server"><span aria-hidden="true">&times;</span></asp:LinkButton>
            <h6 class="modal-title" id="modal-new-ticket-label">                
                <asp:Label runat="server"  ID="lblOperacion" Font-Bold="true"/>
            </h6>
        </div>
        <div class="modal-body">
            <div class="row">
                <div>
                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                        <div>
                            <div class="form-group margin-top">
                                Escribe el nombre de la Categoría<br />
                                <asp:TextBox runat="server" ID="txtDescripcionAreas" CssClass="form-control" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50" />
                            </div>
                            <ajax:AsyncFileUpload ID="afDosnload" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afDosnload_OnUploadedComplete" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" Visible="False" />
                            <p class="margin-top-20">
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Guardar" ID="btnGuardarArea" OnClick="btnGuardar_OnClick" />
                            </p>
                        </div>
                        <p class="text-right margin-top-40">
                            <asp:Button runat="server" CssClass="btn btn-success" Text="Terminar" ID="btnTerminar" OnClick="btnTerminar_OnClick"></asp:Button>
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
