
function show_rules() {
    document.getElementById('content').style.display = 'block';
    document.getElementById('fade').style.display = 'block';
    return;
}

function hide_rules() {
    document.getElementById('content').style.display = 'none';
    document.getElementById('fade').style.display = 'none';
    return;
}


function show_error_message() {
    document.getElementById('error_message').style.display = 'block';
    document.getElementById('fade_error_message').style.display = 'block';
    return;
}

function hide_error_message() {
    document.getElementById('error_message').style.display = 'none';
    document.getElementById('fade_error_message').style.display = 'none';
    return;
}


function ChangeImage(imageType) {
    if (imageType == 'mouseoverImage') {
        document.getElementById('btn_play_now').src = '../resources/buttons/btn_play_hover.png';
    } else {
        document.getElementById('btn_play_now').src = '../resources/buttons/btn_play.png';
    }
}
function show_register() {
    document.getElementById('content_register_user').style.display = 'block';
    document.getElementById('hide_register').style.display = 'block';
    hide_login();
    return;
}


function hide_register() {
    document.getElementById('content_register_user').style.display = 'none';
    document.getElementById('hide_register').style.display = 'none';
    return;
}

function show_fb_register() {
    document.getElementById('content_register_fb_user').style.display = 'block';
    document.getElementById('hide_register').style.display = 'block';
    hide_login();
    return;
}


function hide_fb_register() {
    document.getElementById('content_register_fb_user').style.display = 'none';
    document.getElementById('hide_register').style.display = 'none';
    return;
}
function hide_alertmessage() {
    document.getElementById('alertmessage').style.display = 'none';
    document.getElementById('hide_register_fb_cross').style.display = 'none';
    return;
}

function show_login() {
    document.getElementById('userdetails').style.display = 'block';
    document.getElementById('fade_userdetails').style.display = 'block';
    document.getElementById('hide_login').style.display = 'block';
    return;
}


function hide_login() {
    document.getElementById('userdetails').style.display = 'none';
  
    document.getElementById('fade_userdetails').style.display = 'none';
    return;
}


function show_forgot_password() {
    document.getElementById('forgot_password').style.display = 'block';
    document.getElementById('fade_forgot_password').style.display = 'block';
    hide_login();
    return;
}


function hide_forgot_password() {
    document.getElementById('forgot_password').style.display = 'none';
    document.getElementById('fade_forgot_password').style.display = 'none';
    return;
}

function show_session_timeout_message() {
    document.getElementById('session_timeout_message').style.display = 'block';
    document.getElementById('fade_session_timeout_message').style.display = 'block';
    return;
}

function btn_cashier_Click() {
    window.open('frm_cashier.aspx', 'popUpWindow', 'height=300,width=550,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
}


function go_to_profile_edit() {
    document.getElementById('edit_profile').style.display = 'block';
    document.getElementById('hide_edit_profile').style.display = 'block';
    
 //   window.open('frm_user_profile_edit.aspx', 'popUpWindow', 'height=300,width=550,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
}