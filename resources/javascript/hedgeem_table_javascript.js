
// Function to give the play a visual indication that the 'Deal Hole' button is clickable.
// This funtion just swaps-out the 'Deal Hole' button image with a slightly bigger one
// When the user hovers over the image with thier mouse.
function ChangeImage(imageType) {
    if (imageType == 'mouseoverImage') {
        document.getElementById('btnDealHole').src = '../resources/buttons/btn_deal_hole_hover.png';
    } else {
        document.getElementById('btnDealHole').src = '../resources/buttons/btn_deal_hole.png';
    }
}


function ChangeImageTurn(imageType) {
    if (imageType == 'mouseoverImage') {
        document.getElementById('btnDealTurn').src = '../resources/buttons/btn_deal_turn_hover.png';
    }
    else {
        document.getElementById('btnDealTurn').src = '../resources/buttons/btn_deal_turn.png';
    }
}

function ChangeImageRiver(imageType) {
    if (imageType == 'mouseoverImage') {
        document.getElementById('btnDealRiver').src = '../resources/buttons/btn_deal_river_hover.png';
    }
    else {
        document.getElementById('btnDealRiver').src = '../resources/buttons/btn_deal_river.png';
    }
}

/*
f_placebet(_click_hand_index)
-----------------------------

Function overview
-----------------
This function is called when a player clicks on a GUI object (Hand Panel) or (Betting Panel) to indicate 
they want to place a bet on a given Hedgeem Hand. 

Arguments
---------
_click_hand_index: identifies which hand on the HedgeEmTable the player wants to place a bet on.

What this function does
-----------------------
The function uses this argument to:
1) set the value of the 'mytext' element    (I HAVE NO IDEA WHY! Rohin / Kiran please comment)
2) Invokes the '.click' method on the hidden web page button called 'btn_Get_Clicked_Hand_Value' 

Notes.
------
In HedgeEMTable.aspx you will see the following button declaration
<asp:Button ID="btn_Get_Clicked_Hand_Value" Style="display: none;" runat="server"
Text="Get_Hand_Value" OnClick="_hedgeem_betting_panels" />

In HedgeEmTable.aspx.cs you will see the function ...
protected void btn_Get_Clicked_Hand_Value_Click(object sender, EventArgs e)
This is used to call the HedgeEmServer to place the player's bet        

*/
/*function f_placebet(_click_hand_index) {
    document.getElementById('btn_hidden_control_temp_store_for_hand_index').value = _click_hand_index;
    if (_click_hand_index == 0) {
        Place_Bet_1();
    }
    else if (_click_hand_index == 1) {
        Place_Bet_2()
    }
    else if (_click_hand_index == 2) {
        Place_Bet_3();
    }
    else {
        Place_Bet_4();
    }
     document.getElementById('btn_hidden_control_to_place_bet').click();
    javascript: play_multi_sound('sound_bet');

}*/

// Placeholder function - does not do anything yet Sep 15 2014
function f_get_player_id(seat_id, player_id) {
    window.alert("Seat " + seat_id + " Player " + player_id);
    document.getElementById('btn_hidden_control_temp_store_for_hand_index').value = _click_hand_index;
    document.getElementById('btn_hidden_control_to_place_bet').click();
    //document.getElementById('btnLogout').click();

} 


function f_placebet(_click_hand_index) {
    document.getElementById('btn_hidden_control_temp_store_for_hand_index').value = _click_hand_index;
    document.getElementById('btn_hidden_control_to_place_bet').click();
    javascript: play_multi_sound('sound_bet');

}

function Place_Bet_1() {
    show_bet_popup1();
    var str_bet_amount = document.getElementById('player_balance_xxx_should_be_seat_id').innerHTML;
    var split_bet_amount = str_bet_amount.split("£");
    var bet_amount = parseInt(split_bet_amount[1]);

    var set_payout_value = document.getElementById('payout_value_h0').innerHTML;
    var split_payout_value = set_payout_value.split("£");
    var payout_value = parseInt(split_payout_value[1]);

    $(function () {
        $('#slider-range-min1').slider({ range: 'min', value: 1, min: 1, max: bet_amount, step: 1, slide:
function (event, ui) {
    $('#amount1').val(ui.value); $('#derived_payout_header1').val((ui.value * payout_value).toFixed
(2));
}
        }); $('#amount1').val($('#slider-range-min1').slider('value')); $('#derived_payout_header1').val(($('#slider-range-min1').slider('value') * payout_value).toFixed(2));
    });
}

function Place_Bet_2() {
    show_bet_popup2();

    var str_bet_amount = document.getElementById('player_balance_xxx_should_be_seat_id').innerHTML;
    var split_bet_amount = str_bet_amount.split("£");
    var bet_amount = parseInt(split_bet_amount[1]);

    var set_payout_value = document.getElementById('payout_value_h1').innerHTML;
    var split_payout_value = set_payout_value.split("£");
    var payout_value = parseInt(split_payout_value[1]);


    $(function () {
        $('#slider-range-min2').slider({ range: 'min', value: 1, min: 1, max: bet_amount, step: 1, slide:
function (event, ui) {
    $('#amount2').val(ui.value); $('#derived_payout_header2').val((ui.value * payout_value).toFixed
(2));
}
        }); $('#amount2').val($('#slider-range-min2').slider('value')); $('#derived_payout_header2').val(($('#slider-range-min2').slider('value') * payout_value).toFixed(2));
    });
}

function Place_Bet_3() {
    show_bet_popup3();

    var str_bet_amount = document.getElementById('player_balance_xxx_should_be_seat_id').innerHTML;
    var split_bet_amount = str_bet_amount.split("£");
    var bet_amount = parseInt(split_bet_amount[1]);

    var set_payout_value = document.getElementById('payout_value_h2').innerHTML;
    var split_payout_value = set_payout_value.split("£");
    var payout_value = parseInt(split_payout_value[1]);


    $(function () {
        $('#slider-range-min3').slider({ range: 'min', value: 1, min: 1, max: bet_amount, step: 1, slide:
function (event, ui) {
    $('#amount3').val(ui.value); $('#derived_payout_header3').val((ui.value * payout_value
).toFixed(2));
}
        }); $('#amount3').val($('#slider-range-min3').slider('value')); $('#derived_payout_header3').val(($('#slider-range-min3').slider('value') * payout_value).toFixed(2));
    });
}


function Place_Bet_4() {
    show_bet_popup4();
    var str = document.getElementById('player_balance_xxx_should_be_seat_id').innerHTML;
    var split = str.split("£");
    var bet_amount = parseInt(split[1]);

    var str1 = document.getElementById('payout_value_h3').innerHTML;
    var split1 = str1.split("£");
    var payout_value = parseInt(split1[1]);

    jQuery(function () {
        jQuery('#slider-range-min4').slider({ range: 'min', value: 1, min: 1, max: bet_amount, step: 1, slide:
function (event, ui) {
    jQuery('#amount4').val(ui.value); jQuery('#derived_payout_header4').val((ui.value * payout_value
).toFixed(2));
}
        }); jQuery('#amount4').val($('#slider-range-min4').slider('value')); jQuery('#derived_payout_header4').val((jQuery('#slider-range-min4').slider('value') * payout_value).toFixed(2));
    });

}




//http://stackoverflow.com/questions/723290/in-an-onclick-handler-how-can-i-detect-whether-shift-was-pressed
window.addEventListener("click",
    function (e) {
        if (e.shiftKey) alert("Shift, yay!");
    },
false);

function f_cancel_bet(_click_hand_index) {
    document.getElementById('btn_hidden_control_temp_store_for_hand_index').value = _click_hand_index;
    document.getElementById('btn_hidden_control_to_cancel_bet').click();
   // alert("Cancelling bet!");
    javascript: play_multi_sound('sound_deal');

}

/* Function to close the div pop-up */
function closediv() {
    document.getElementById('error_message').style.display = 'none';
    document.getElementById('fade').style.display = 'none';
}


/* ####################### Sound ######################
The following two refernces were used to help find coding for playing sound on click
    http://www.codeproject.com/Questions/229577/Calling-a-Javascript-function-from-a-image-button 
    http://www.storiesinflight.com/html5/audio.html
*/
var channel_max = 10;										// number of channels
audiochannels = new Array();
for (a = 0; a < channel_max; a++) {									// prepare the channels
    audiochannels[a] = new Array();
    audiochannels[a]['channel'] = new Audio();						// create a new audio object
    audiochannels[a]['finished'] = -1;							// expected end time for this channel
}
function play_multi_sound(s) {
    for (a = 0; a < audiochannels.length; a++) {
        thistime = new Date();
        if (audiochannels[a]['finished'] < thistime.getTime()) {			// is this channel finished?
            audiochannels[a]['finished'] = thistime.getTime() + document.getElementById(s).duration * 1000;
            audiochannels[a]['channel'].src = document.getElementById(s).src;
            audiochannels[a]['channel'].load();
            audiochannels[a]['channel'].play();
            break;
        }
    }
}

function play_single_sound() {
    document.getElementById('sound_deal').play();

}

function show_rules() {
    document.getElementById('content').style.display = 'block';
    document.getElementById('fade_rules').style.display = 'block';
    return;
}

function hide_rules() {
    document.getElementById('content').style.display = 'none';
    document.getElementById('fade_rules').style.display = 'none';
    return;
}


function show_get_chips() {
    document.getElementById('get_chips').style.display = 'block';
    document.getElementById('fade_get_chips').style.display = 'block';
    return;
}

function hide_get_chips() {
    document.getElementById('get_chips').style.display = 'none';
    document.getElementById('fade_get_chips').style.display = 'none';
    return;
}



function show_play_for_real_text() {
    document.getElementById('play_for_real_text').style.display = 'block';
    document.getElementById('fade_play_for_real').style.display = 'block';
    return;
}

function hide_play_for_real_text() {
    document.getElementById('play_for_real_text').style.display = 'none';
    document.getElementById('fade_play_for_real').style.display = 'none';
    return;
}


function show_session_timeout_message() {
    document.getElementById('session_timeout_message').style.display = 'block';
    document.getElementById('fade_session_timeout_message').style.display = 'block';
    return;
}

function show_bet_popup1() {
    document.getElementById('hedgeem_bet_slider1').style.display = 'block';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'block';
    return;
}
function hide_bet_popup1() {
    document.getElementById('hedgeem_bet_slider1').style.display = 'none';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'none';
    return;
} 


function show_bet_popup2() {
    document.getElementById('hedgeem_bet_slider2').style.display = 'block';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'block';
    return;
}
function hide_bet_popup2() {
    document.getElementById('hedgeem_bet_slider2').style.display = 'none';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'none';
    return;
} 


function show_bet_popup3() {
    document.getElementById('hedgeem_bet_slider3').style.display = 'block';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'block';
    return;
}
function hide_bet_popup3() {
    document.getElementById('hedgeem_bet_slider3').style.display = 'none';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'none';
    return;
} 


function show_bet_popup4() {
    document.getElementById('hedgeem_bet_slider4').style.display = 'block';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'block';
    return;
}

function hide_bet_popup4() {
    document.getElementById('hedgeem_bet_slider4').style.display = 'none';
    document.getElementById('fade_hedgeem_bet_slider').style.display = 'none';
    return;
}
function btn_cashier_Click() {
}
function btn_cashier_Clicking() {
    var request = $.ajax({
        url: "frm_cashier.aspx",
        cache: false,
        success: function (html) {
            $("#cashier_div").append(html);
        }
    });
    request.fail(function (jqXHR, textStatus) {
        alert("Request failed: " + textStatus);
    });
  //  window.open('frm_cashier.aspx', 'popUpWindow', 'height=300,width=550,left=300,top=200,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes');
}

function ai_fultilt_click() {
    window.open("http://www.fulltiltpoker.com/", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400");
    hide_get_chips();
    document.getElementById('btn_get_chips_add').click();
    InitializeRequest("fultilt");
}

function InitializeRequest(path) {
    // call server side method
    PageMethods.get_clicked_name(path);
}

function ai_betfair_click() {
    window.open("http://www.betfair.com/GBR/en/", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400");
    self.focus();
    hide_get_chips();
    document.getElementById('btn_get_chips_add').click();
    InitializeRequest("betfair");
}

function ai_paypal_click() {
    //window.open("http://www.paypal.com", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400");
    hide_get_chips();
    document.getElementById('btn_get_chips_add').click();
    InitializeRequest("paypal");
}

function ai_fb_recommend_click() {
    window.open("http://www.facebook.com", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400");
    hide_get_chips();
    document.getElementById('btn_get_chips_add').click();
    InitializeRequest("facebook_recommend");
}

function ai_fb_share_click() {
    window.open("http://www.facebook.com", "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=500, width=400, height=400");
    hide_get_chips();
    document.getElementById('btn_get_chips_add').click();
    InitializeRequest("facebook_share");
}


