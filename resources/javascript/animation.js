//$(document).ready(function () {
//    var dealhole = document.getElementById('btnDealHole');
//    alert(dealhole);
//    dealhole.addEventListener("click", function () { animate(); }, false);
//});

function animate_Hand() {
   

    $(".card_left").each(function () {

        $(".card_left").addClass("animated flip");
        //            var leftID = $(this).attr("id");

        //            move(leftID);
    });
    $(".card_right").each(function () {

        $(".card_right").addClass("animated flip");
        //            var rightID = $(this).attr("id");
        //            move(rightID);
    });
}
//***********************************************************************************
function animate_card_flop() {
    $(".card_left_flop").each(function () {

        $(".card_left_flop").addClass("animated flip");
        //            var leftflop = $(this).attr("id");
        //            move(leftflop);
    });

    $(".card_middle_flop").each(function () {

        $(".card_middle_flop").addClass("animated flip");
        //            var middleflop = $(this).attr("id");
        //            move(middleflop);
    });

    $(".card_right_flop").each(function () {

        $(".card_right_flop").addClass("animated flip");
        //            var rightflop = $(this).attr("id");
        //            move(rightflop);
    });
}
//***********************************************************************************
function animate_card_middle_turn() {

    $(".card_middle_turn").each(function () {

        $(".card_middle_turn").addClass("animated flip");
        //            var middleturn = $(this).attr("id");
        //            move(middleturn);
    });
}
//***********************************************************************************

function animate_card_middle_river() {
    $(".card_middle_river").each(function () {

        $(".card_middle_river").addClass("animated flip");
        //            var middleriver = $(this).attr("id");
        //            move(middleriver);
    });
}

//***********************************************************************************


function animate_card_next_game() {

    $(".card_left").each(function () {

        $(".card_left").addClass("animated flip");

    });
    $(".card_right").each(function () {

        $(".card_right").addClass("animated flip");

    });

    $(".card_left_flop").each(function () {

        $(".card_left_flop").addClass("animated flip");

    });

    $(".card_middle_flop").each(function () {

        $(".card_middle_flop").addClass("animated flip");

    });

    $(".card_right_flop").each(function () {

        $(".card_right_flop").addClass("animated flip");

    });
    $(".card_middle_turn").each(function () {

        $(".card_middle_turn").addClass("animated flip");

    });

    $(".card_middle_river").each(function () {

        $(".card_middle_river").addClass("animated flip");

    });

}



//***********************************************************************************
//        function move(elementid) {
//          
//            with (document.getElementById(elementid).style) {
//                zIndex = "1000";
//                WebkitTransform = MozTransform = OTransform = msTransform = "rotate(720deg)";
//                zIndex = "0";
//            }
//        }
//    







//});
// ************************************************************************************************************************************
// code to show deck of cards animating
//$(document).ready(function () {
//debugger
//    var cards = new Array();
//    var CardGame = function (targetId, card_value) {

//        // private variables


//        var started = false;

//        var moveToPlace = function (id) // deal card
//        {
//            if (card_value != null) {
//                cards[id].matched = false;
//                //   cards[id].firstChild.src = "images/cards/" + card_value[id] + ".png";
//                cards[id].firstChild.src = card_value[id];

//                with (cards[id].style) {
//                    zIndex = "1000";
//                    bottom = cards[id].fromtop + "px";
//                    right = cards[id].fromleft + "px";
//                    WebkitTransform = MozTransform = OTransform = msTransform = "rotate(180deg)";
//                    zIndex = "0";
//                }
//            }

//        };

//        var cardClick = function (id) {

//            for (i = 0; i < 4; i++) {
//                (function (idx) {
//                    setTimeout(function () { moveToPlace(idx); }, idx * 1000);
//                })(i);
//            }
//            started = true;
//        };

//        // initialise

//        //        var stage = document.getElementById(targetId);
//        //        var felt = document.createElement("div");
//        //        felt.id = "felt";
//        //        stage.appendChild(felt);

//        // template for card

//        //        $(".card_left").each(function () {

//        //            var card = $(this).attr("id");
//        // alert(card);
//        // var sid = document.getElementById('buttons');
//        // card.innerHTML = "<img src=\"images/cards/back.png\">";

//        for (var i = 0; i < 4; i++) {
//            // var newCard = card.cloneNode(true);

//            // newCard.fromtop = 185 + 120 * Math.floor(i / 4);
//            // newCard.fromleft = 120 + 200 * (i % 4);
//            (function (idx) {

//                window.addEventListener("load", function () { cardClick(idx); }, false);

//            })(i);

//        }
//        // felt.appendChild(newCard);
//        //  cards.push(newCard);

//        // }
//        //        });
//    }

//    //        $(document).ready(function () {
//    // var card_value4 = ["1C", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H"];
//    //var game = new CardGame("stage", card_value4);
//    var a = new Array();
//    var card_value = new Array();
//    $.ajax({
//        type: "POST",
//        contentType: "application/json; charset=utf-8",
//        url: "frm_hedgeem_table.aspx/f_update_hedgeem_control_hand_panels",

//        dataType: "json",
//        success: function (data) {
//            for (var i = 0; i < data.d.length; i++) {

//                a = data.d[i].toString();
//                card_value.push(a);
//                cards.push(a);
//            }

//        }

//    });
//  alert(cards);
//    var game = new CardGame("hand_panels_container", card_value);



//});








//var stage = document.getElementById("hand_panels_container");
//var cards = []
//var moveToPlace = function (id, elementid) // deal card
//{

//    with (document.getElementById('h0c1').style) {
//        zIndex = "1000";
//        bottom = cards[id].fromtop + "px";
//        right = cards[id].fromleft + "px";
//        WebkitTransform = MozTransform = OTransform = msTransform = "rotate(180deg)";
//        zIndex = "0";
//    }

//};


//var cardClick = function (id) {

//    for (i = 0; i < 8; i++) {
//        (function (idx) {
//            setTimeout(function () { moveToPlace(idx); }, idx * 1000);
//        })(i);
//    }
//    started = true;

//};

//// initialise


//var felt = document.createElement("div");
//felt.id = "felt";
//stage.appendChild(felt);




//// template for card
////    var card = document.getElementById('hedgeem_control_hand_panel_hand_0');
////    var sid = document.getElementById('buttons');
////    card.innerHTML = "<img src=\"images/cards/back.png\">";

//for (var i = 0; i < 8; i++) {
//    var newCard = card.cloneNode(true);

//    newCard.fromtop = 185 + 120 * Math.floor(i / 4);
//    newCard.fromleft = 120 + 200 * (i % 4);
//    (function (idx) {
//        window.addEventListener("load", function () { cardClick(idx); }, false);
//    })(i);




//    //        felt.appendChild(newCard);
//    //        cards.push(newCard);
//}
