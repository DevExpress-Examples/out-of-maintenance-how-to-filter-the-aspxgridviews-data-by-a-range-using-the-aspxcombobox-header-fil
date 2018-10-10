<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
            OnDataBinding="grid_DataBinding"
            OnAutoFilterCellEditorCreate="grid_AutoFilterCellEditorCreate"
            OnAutoFilterCellEditorInitialize="grid_AutoFilterCellEditorInitialize" OnProcessColumnAutoFilter="grid_ProcessColumnAutoFilter">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="0">
                    <Settings AllowAutoFilter="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="1">
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" />
        </dx:ASPxGridView>
    </form>
</body>
</html>