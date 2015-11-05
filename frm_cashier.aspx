<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_cashier.aspx.cs" Inherits="frm_cashier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <script type="text/javascript">
          function RefreshParent() {
              if (window.opener != null && !window.opener.closed) {
                  window.opener.location.reload();
              }
          }
          window.onbeforeunload = RefreshParent;
    </script>
    
    <style>
    #tbl_transfer_funds td {
    padding: 6px; padding-top:0;
} #tbl_deposit_funds td {
    padding: 6px; padding-top:0;
}


    </style>
    <script type="text/javascript">
        function hide_rm() {
            document.getElementById('hide_rm').style.display = 'none';
            document.getElementById('rm').style.display = 'none';
        }
    </script>
    <div id="rm" class="white_cashierdiv" >
    <div class="modal-header">
                            <div id="hide_rm" onclick="hide_rm();" class="close">
                                <span aria-hidden="true">×</span>
                            </div>
    </div>
        <table cellspacing="6" cellpadding="6" width="" id="tbl_transfer_funds" border="0">
            <tr>
                <td colspan="2">
                    <h3 style="margin-top:0;">
                        Transfer Balance</h3>
                    <asp:Label runat="server" ID="lbl_msg" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Account Balance:
                    <asp:Label ID="lbl_account_balance" runat="server" Text="Label"></asp:Label>
                </td>
                <td>
                    Seat Balance :
                    <asp:Label ID="lbl_seat_balance" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Amount to be Transfered:
                    <asp:TextBox ID="txt_tranfer_balance" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr >
                <td colspan="">
                    <asp:Button ID="btn_transfer" CssClass="btn btn-success" runat="server" Text="Transfer Balance" 
                        OnClick="btn_transfer_Click" />
                    &nbsp;
                </td>
            </tr>
        </table>

        <table cellspacing="6" cellpadding="6" width="" id="tbl_deposit_funds" border="0">
            <tr>
                <td colspan="2">
                    <h3>
                        Deposit Money:</h3>
                    <asp:Label runat="server" ID="lbl_deposit_status" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Amount to be Deposited:
                    <asp:TextBox ID="txt_deposit" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="">
                    <asp:Button ID="btn_deposit" CssClass="btn btn-primary" runat="server" Text="Deposit" OnClick="btn_deposit_Click"
                       />
                    &nbsp;
                    <asp:Button ID="btn_sit_here" CssClass="btn btn-success" runat="server" Text="Sit here" OnClick="btn_sit_here_Click"
                       />
                </td>
            </tr>
        </table>
    </div>
    </form>     <div id="popup_message">
                    <asp:PlaceHolder ID="Place_Holder_Popup_Message" runat="server"></asp:PlaceHolder>
                </div>
</body>
</html>
