<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchUI.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server"/><asp:Button ID="Button1" runat="server" Text="Upload" OnClick="Upload"/>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox><asp:Button ID="Button2" runat="server" Text="Search" OnClick="SearchText"/>
        <asp:Label ID="lblMessgae" runat="server" Text=""></asp:Label>
    <div>
        <asp:Image ID="imgDemo" runat="server" />        
    </div>
    </form>
</body>
</html>
