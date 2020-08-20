$(function () {
    var availableTags = [];

    $.ajax({
        type: 'GET',
        url: 'Home/getChannelTitle',
        success: function (response) {
            //availableTags.push(response.video_title);
            $.each(response, function (key, item) {
                availableTags.push(item.channel_name);

            });
        }
    });

    $.ajax({
        type: 'GET',
        url: 'Home/getVideoTitle',
        success: function (response) {
            //availableTags.push(response.video_title);
            $.each(response, function (key, item) {
                availableTags.push(item.video_title);

            });
        }
    });

    

    $("#tags").autocomplete({
        source: availableTags,
        minLength: 1,
        position: { my: "center top", at: "center center", collision: "none" }
    });
});