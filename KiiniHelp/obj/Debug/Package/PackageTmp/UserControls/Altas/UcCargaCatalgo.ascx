<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcCargaCatalgo.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcCargaCatalgo" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:UpdatePanel runat="server" ID="updateAltaCatalogo">
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
                Carga de Catalogo
            </div>
            <div class="panel-body">
                <asp:HiddenField runat="server" ID="hfFileName"/>
                <asp:HiddenField runat="server" ID="hfEsAlta"/>
                <asp:HiddenField runat="server" ID="hfIdCatalogo"/>
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Descripcion" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtDescripcionCatalogo" CssClass="form-control" onkeydown="return (event.keyCode!=13);" MaxLength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Archivo" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-5">
                            <ajax:asyncfileupload id="afuArchivo" runat="server" uploaderstyle="Traditional" onuploadedcomplete="afuArchivo_OnUploadedComplete" persistfile="True" />
                        </div>
                        <asp:Button runat="server" CssClass="btn btn-success" Text="Obtener Paginas" ID="btnLeer" OnClick="btnLeer_OnClick"/>
                    </div>
                    <asp:RadioButtonList runat="server" ID="rbtnHojas" />
                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar" ID="btnGuardar" OnClick="btnGuardar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiar" OnClick="btnLimpiar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
