<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_tutorial_controls.aspx.cs"
    Inherits="frm_tutorial_controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
    <body>

        <form id="form1" runat="server">

        <!-- This is a container DIV that all other divs will be positioned relative to it. -->
        <div id="hedgeem_container">

            
            <div id="hedgeem_sub_content_container_user_input" class="user_input_container">
                <!-- This is the DIV for user to 'experiement' with control -->
                <div id="user_input">
                    <asp:CheckBox ID="chk_show_chip_icon" runat="server" OnCheckedChanged="chk_show_chip_icon_TextChanged" />
                    <asp:TextBox ID="txt_player_balance" runat="server" OnTextChanged="txt_player_balance_TextChanged"></asp:TextBox>
                </div>
            </div>
        
            
            <div id="hedgeem_sub_content_container_control_output" class="hedgeem_sub_content_container">
                <!-- This is the DIV that shows a HedgeEm Seat -->
                <div id="Player_Info">
                    <asp:PlaceHolder ID="Place_Holder_Player_Info" runat="server" ></asp:PlaceHolder>
                </div>
            </div>
        </div>
  
        </form>
  
    </body>
</html>
