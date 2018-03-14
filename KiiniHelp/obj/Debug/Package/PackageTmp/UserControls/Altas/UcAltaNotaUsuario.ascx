<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UcAltaNotaUsuario.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.UcAltaNotaUsuario" %>
<%@ Register TagPrefix="ctrlExterno" Namespace="Winthusiasm.HtmlEditor" Assembly="Winthusiasm.HtmlEditor" %>
<asp:UpdatePanel runat="server" ID="updateAltaNotaUsuario">
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
                    <asp:HiddenField runat="server" ID="hfIdNotaGeneralUsuario" />
                    <div class="form-group">
                        <asp:Label runat="server" Text="Tipo de Nota" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" AutoPostBack="True" CssClass="DropSelect" ID="ddlTipoNota"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Nombre" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtNombreNotaGralUsuario" CssClass="form-control obligatorio" onkeydown="return (event.keyCode!=13);" autofocus="autofocus" MaxLength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Contenido" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <ctrlExterno:HtmlEditor runat="Server" ID="txtEditor" Toolbars="Select#Format,Select#Font,Select#Size:ForeColor,BackColor;Bold,Italic,Underline|Left,Center,Right,Justify|OrderedList,BulletedList|
                                    Outdent,Indent|Rule|Subscript,Superscript:Link,Image"
                                Width="700px" />
                        </div>
                    </div>

                </div>
            </div>
            <div class="panel-footer" style="text-align: center">
                <asp:Button runat="server" CssClass="btn btn-success" Text="Guardar" ID="btnGuardarNota" OnClick="btnGuardar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Limpiar" ID="btnLimpiarNota" OnClick="btnLimpiar_OnClick" />
                <asp:Button runat="server" CssClass="btn btn-danger" Text="Cancelar" ID="btnCancelar" OnClick="btnCancelar_OnClick" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
