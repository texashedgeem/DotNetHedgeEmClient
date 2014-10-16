<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frm_user_profile_edit.aspx.cs"
    Inherits="resources_javascript_frm_user_profile_edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../resources/css/online/profile_edit.css" rel="stylesheet" type="text/css" />
    <script src="//code.jquery.com/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showpreview(input) {

            if (input.files && input.files[0]) {

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#profile_image').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }

        }

    </script>
    <script type="text/javascript">
        function SetSession(src) {

            $.ajax({
                type: "POST",
                url: "frm_user_profile_edit.aspx/SetSession",
                data: '{value: "' + src + '" }',

                dataType: "json",
                cache: false,
                async: true,
             
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function OnSuccess(response) {
            alert(response.d);
        }
    </script>
</head>
<body>
    <article>
<h1>Edit Your Profile</h1></article>
    <form id="form1" runat="server">
    <ul>
        <li>
            <label for="email">
                Username:</label>
            <asp:Label ID="lbl_username" runat="server"></asp:Label></li>
        <li>
            <label for="name">
                Display Name:</label>
            <asp:Label ID="lbl_name" runat="server"></asp:Label></li>
        <li>
            <label for="file">
                Profile Picture:</label>
            <div class="uploader" id="fileupload">
                <input type="file" style="opacity: 0;">
                <span class="filename" style="-moz-user-select: none;">No file selected</span> <span
                    class="action" style="-moz-user-select: none;">Choose File</span>
                <asp:FileUpload ID="file_profile_picture" runat="server" onchange="document.getElementById('avitar_dl').style.display = 'none'; document.getElementById('profile_image').style.display = 'block'; showpreview(this);" /></div>
            <asp:Image ID="profile_image" runat="server" Height="50px" Width="50px" /></li>
        <li id="avitar_dl">
            <label for="avitar">
                Or Select any avitar:</label>
            <asp:Repeater ID="rp_available_avitars" runat="server">
                <ItemTemplate>
                    <img src="<%# Eval("Name","resources/avitars/{0}") %>" alt="img" width="50px" height="50px"
                        onclick="document.getElementById('profile_image').src=this.src;document.getElementById('profile_image').style.display = 'block';document.getElementById('fileupload').style.display = 'none';SetSession(this.src);" />
                </ItemTemplate>
            </asp:Repeater>
        </li>
        <li>
            <label for="theme">
                Select Table Theme:</label>
            <asp:DropDownList ID="ddl_table_themes" runat="server">
            </asp:DropDownList>
        </li>
        <li id="update_button">
            <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" />
        </li>
    </ul>
    </form>
</body>
</html>
