// Original JavaScript code by Chirp Internet: www.chirp.com.au
// Please acknowledge use of this code by including this header.

var CardGame = function (targetId) {
    // private variables
    var cards = []
    var card_value = ["card_ac_middle_alive", "card_2c_middle_alive", "card_3c_middle_alive", "card_4c_middle_alive", "card_5c_middle_alive", "card_6c_middle_alive", "card_7c_middle_alive", "card_8c_middle_alive", "card_ah_middle_alive", "card_2h_middle_alive", "card_3h_middle_alive", "card_4h_middle_alive", "card_5h_middle_alive", "card_6h_middle_alive", "card_7h_middle_alive", "card_8h_middle_alive"];

    var started = false;
    var matches_found = 0;
    var card1 = false, card2 = false;

    var hideCard = function (id) // turn card face down
    {
        cards[id].firstChild.src = "../resources/cards/card__back_blue1_middle.png";
        with (cards[id].style) {
            WebkitTransform = MozTransform = OTransform = msTransform = "scale(1.0) rotate(180deg)";
        }
    };

    var moveToPack = function (id) // move card to pack
    {
        hideCard(id);
        cards[id].matched = true;
        with (cards[id].style) {
            zIndex = "1000";
            top = "100px";
            left = "-140px";
            WebkitTransform = MozTransform = OTransform = msTransform = "rotate(0deg)";
            zIndex = "0";
        }
    };

    var moveToPlace = function (id) // deal card
    {
        cards[id].matched = false;
        with (cards[id].style) {
            zIndex = "1000";
            top = cards[id].fromtop + "px";
            left = cards[id].fromleft + "px";
            WebkitTransform = MozTransform = OTransform = msTransform = "rotate(180deg)";
            zIndex = "0";
        }
    };

    var showCard = function (id) // turn card face up, check for match
    {
        if (id === card1) return;
        if (cards[id].matched) return;

        cards[id].firstChild.src = "../resources/cards/" + card_value[id] + ".png";
        with (cards[id].style) {
            WebkitTransform = MozTransform = OTransform = msTransform = "scale(1.2) rotate(185deg)";
        }

        if (card1 !== false) {
            card2 = id;
            if (parseInt(card_value[card1]) == parseInt(card_value[card2])) { // match found
                (function (card1, card2) {
                    setTimeout(function () { moveToPack(card1); moveToPack(card2); }, 1000);
                })(card1, card2);
                if (++matches_found == 8) { // game over, reset
                    matches_found = 0;
                    started = false;
                }
            } else { // no match
                (function (card1, card2) {
                    setTimeout(function () { hideCard(card1); hideCard(card2); }, 800);
                })(card1, card2);
            }
            card1 = card2 = false;
        } else { // first card turned over
            card1 = id;
        }
    };

    var cardClick = function (id) {
        if (started) {
            showCard(id);
        } else {
            // shuffle and deal cards
            card_value.sort(function () { return Math.round(Math.random()) - 0.5; });
            for (i = 0; i < 16; i++) {
                (function (idx) {
                    setTimeout(function () { moveToPlace(idx); }, idx * 100);
                })(i);
            }
            started = true;
        }
    };

    var cardresetClick = function (id) {
        //  alert('hi');
        matches_found = 0;
        started = false;
        moveToPack(id);
    };

    // initialise

    var stage = document.getElementById(targetId);
    var felt = document.createElement("div");
    felt.id = "felt";
    stage.appendChild(felt);

    // template for card
    var card = document.createElement("div");
    card.innerHTML = "<img src=\"../resources/cards/card__back_blue1_middle.png\">";

    for (var i = 0; i < 16; i++) {
        var newCard = card.cloneNode(true);

        newCard.fromtop = 15 + 120 * Math.floor(i / 4);
        newCard.fromleft = 70 + 100 * (i % 4);
        (function (idx) {
            newCard.addEventListener("click", function () { cardClick(idx); }, false);
            deal.addEventListener("click", function () { cardClick(idx); }, false);
            reset.addEventListener("click", function () { cardresetClick(idx); }, false);
        })(i);

        felt.appendChild(newCard);
        cards.push(newCard);

    }

}
