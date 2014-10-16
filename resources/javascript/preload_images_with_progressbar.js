//url used to display percentage progress - http://www.codingforums.com/archive/index.php/t-42193.html

var timePeriod = 100;
var percent = 1;
var count = "";
function progressBar(image_count) {
    // checks if percent is less than the total image count
    if (percent < image_count) {
        //increments percent by one
        percent = percent + 1;
        // gets the id of the div and writes into it the Percentage of div loaded till now
        document.getElementById('progressbar_content').innerHTML = '<br/><br/> Loading : ' + Math.round((percent / image_count) * 100) + '%';
        window.status = 'Loading : ' + percent + '%';
        // this function holds the previous value of calculated percentage
        setTimeout('progressBar(' + image_count + ')', timePeriod);
        // this function increases the green progress bar
        draw_green_progressbar(image_count, percent);
    }
    else {
        // this code hides the progress when all images are downloaded
        document.getElementById('progressbar_content').innerHTML = '';
        document.getElementById('progressbar').style.display = 'none';
        document.body.style.display = '';
    }

}

//url used to show increasing progress bar - http://ruwix.com/simple-javascript-html-css-slider-progress-bar/ 
function draw_green_progressbar(count, inc_percent) {
    var cal_percent = Math.round((inc_percent * 100) / count);
    document.getElementById('div_progress_bar').style.width = cal_percent + '%';
    document.getElementById('div_percent').innerHTML = cal_percent + '%';
}