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
    <div>
        <table cellspacing="6" cellpadding="6" width="40%" id="tbl_transfer_funds" border="2">
            <tr>
                <td colspan="2">
                    <h3>
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
            <tr align="center">
                <td colspan="2">
                    <asp:Button ID="btn_transfer" runat="server" Text="Transfer Balance" 
                        OnClick="btn_transfer_Click" />
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table cellspacing="6" cellpadding="6" width="40%" id="tbl_deposit_funds" border="2">
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
            <tr align="center">
                <td colspan="2">
                    <asp:Button ID="btn_deposit" runat="server" Text="Deposit" OnClick="btn_deposit_Click"
                       />
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
