var video = new Video("backgroundVideo");
video.enableAutoplay();
video.enableLoop();

video.enableAudio();
video.setVolume(0.2);
document.getElementById("audioControlTitle").className = "fa fa-volume-up"

function switchAudioState() {
    if (video.getAudioState() === "enable") {
        video.disableAudio();
        document.getElementById("audioControlTitle").className = "fa fa-volume-mute"
    } else {
        video.enableAudio();
        video.setVolume(0.2);
        document.getElementById("audioControlTitle").className = "fa fa-volume-up";
    }
}