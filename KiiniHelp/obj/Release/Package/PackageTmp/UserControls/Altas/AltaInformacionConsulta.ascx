<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaInformacionConsulta.ascx.cs" Inherits="KiiniHelp.UserControls.Altas.AltaInformacionConsulta" %>
<%@ Register Assembly="Winthusiasm.HtmlEditor" Namespace="Winthusiasm.HtmlEditor" TagPrefix="ctrlExterno" %>
<%@ Register TagPrefix="ajax" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=16.1.0.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:UpdatePanel runat="server" ID="upInfo">
    <ContentTemplate>
        <asp:HiddenField runat="server" ID="hfFileName" />
        <header id="panelAlert" runat="server" visible="False">
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
                <asp:Repeater runat="server" ID="rptHeaderError">
                    <ItemTemplate>
                        <%# Container.DataItem %>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </header>
        <div class="panel panel-primary">
            <div class="panel-heading">
                Agregar contenido
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <asp:HiddenField runat="server" ID="hfEsAlta" />
                    <asp:HiddenField runat="server" ID="hfIdInformacionConsulta" />

                    <div class="form-group">
                        <asp:Label runat="server" Text="Nombre del contenido" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control obligatorio" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" Text="Tipo de contenido" CssClass="col-sm-2 control-label" />
                        <div class="col-sm-10">
                            <asp:DropDownList runat="server" ID="ddlTipoInformacion" CssClass="DropSelect" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoInformacion_OnSelectedIndexChanged" />
                        </div>
                    </div>
                </div>
                <div runat="server" id="divPropietrario" visible="False">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Contenido" CssClass="col-sm-2 control-label obligatorio" />
                            <div class="col-sm-10">
                                <ctrlExterno:HtmlEditor runat="Server" ID="txtEditor" Toolbars="Select#Format,Select#Font,Select#Size:ForeColor,BackColor;Bold,Italic,Underline|Left,Center,Right,Justify|OrderedList,BulletedList|
                                    Outdent,Indent|Rule|Subscript,Superscript:Link,Image"
                                    Width="700px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="divDocumento" visible="False">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Tipo de Documento" CssClass="col-sm-2 control-label" />
                            <div class="col-sm-10">
                                <asp:DropDownList runat="server" ID="ddlTipoDocumento" CssClass="DropSelect obligatorio" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoDocumento_OnSelectedIndexChanged" />
                            </div>
                        </div>
                        <div class="form-group" runat="server" id="divUploadDocumento" visible="False">
                            <asp:Label runat="server" Text="Archivo" CssClass="col-sm-2 control-label" />
                            <div class="col-sm-10">
                                <ajax:AsyncFileUpload ID="afuArchivo" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afuArchivo_OnUploadedComplete" PersistFile="True" />
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="divUrl" visible="False">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" Text="Url Pagina Web" CssClass="col-sm-2 control-label" />
                            <div class="col-sm-10">
                                <asp:TextBox runat="server" ID="txtDescripcionUrl" CssClass="form-control obligatorio" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-horizontal">
                        <div class="form-group">
                                                    Archivo adjunto para descarga
                        <asp:Repeater runat="server" ID="rptDonloads">
                            <ItemTemplate>
                                <asp:Label runat="server" Text="" CssClass="col-sm-2 control-label" />
                                <div class="col-sm-10">
                                    <ajax:AsyncFileUpload ID="afDosnload" runat="server" UploaderStyle="Traditional" OnUploadedComplete="afDosnload_OnUploadedComplete" ClientIDMode="AutoID" PersistFile="True" ViewStateMode="Enabled"/>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
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
