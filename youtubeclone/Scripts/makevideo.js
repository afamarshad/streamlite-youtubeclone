(function () {
    var video = document.getElementById('video'),
        vendorUrl = window.URL || window.webkitURL;

    navigator.getMedia = navigator.getUserMedia ||
        navigator.webkitGetUserMedia ||
        navigator.mozGetUserMedia ||
        navigator.msGetUserMedia;

    //Capture Video
    navigator.getMedia({
        video: true,
        audio: true,
    }, function (stream) {
        
        console.log(stream);
        video.srcObject = stream;
        video.play();
        }, function (error) {


        });

    var canvas = document.querySelector('canvas');
    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
    var rafId;
    var frames = [];
    var CANVAS_WIDTH = canvas.width;
    var CANVAS_HEIGHT = canvas.height;

    function drawVideoFrame(time) {
        rafId = requestAnimationFrame(drawVideoFrame);
        ctx.drawImage(video, 0, 0, CANVAS_WIDTH, CANVAS_HEIGHT);
        frames.push(canvas.toDataURL('image/webp', 1));
    };

    rafId = requestAnimationFrame(drawVideoFrame); // Note: not using vendor prefixes!

    function stop() {
        cancelAnimationFrame(rafId);  // Note: not using vendor prefixes!

        // 2nd param: framerate for the video file.
        var webmBlob = Whammy.fromImageArray(frames, 1000 / 60);

        var video = document.createElement('video');
        video.src = window.URL.createObjectURL(webmBlob);

        document.body.appendChild(video);
    }
})();