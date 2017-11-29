<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaArea.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaArea" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:UpdatePanel runat="server" ID="updateAltaAreas">
    <ContentTemplate>
        <header id="panelAlerta" runat="server" visible="false">
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
        <div class="panel panel-primary">
            <div class="panel-heading">
                Agregar Área de Atención
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <asp:HiddenField runat="server" ID="hfEsAlta" />
                    <asp:HiddenField runat="server" ID="hfIdArea" />
                    <asp:HiddenField runat="server" ID="hfFileName" />
                    <div class="form-group">
                        <asp:Label runat="server" Text="Descripcion" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtDescripcionAreas" CssClass="form-control obligatorio" onkeydown = "return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50"/>
                        </div>
                    </div>
                    <ajax:AsyncFileUpload ID="afDosnload" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afDosnload_OnUploadedComplete" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled" Visible="False"/>
                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar" ID="btnGuardarArea" OnClick="btnGuardar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiarArea" OnClick="btnLimpiar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
