<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_read_text_logs.aspx.cs" Inherits="frm_read_text_logs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>hedgeem.com</h2>
Please Select the Text File: <asp:DropDownList runat="server" ID="ddl_files" 
            onselectedindexchanged="ddl_files_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList> 
          Timestamp : <asp:Label ID="lbl_timestamp" runat="server"></asp:Label>
            <asp:Button ID="btn_refresh" runat="server" Text="Refresh" onclick="btn_refresh_Click"/>
<br /><asp:Label ID="lbl_msg" runat="server" ForeColor="Red"></asp:Label>
<br />
<asp:TextBox TextMode="MultiLine" ID="txt_logs_data" runat="server" Width="100%" Height="550px"></asp:TextBox>
    </div>
    </form>
</body>
</html>
