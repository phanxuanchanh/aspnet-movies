
function Video(id){
    var video = document.getElementById(id);


    this.getAudioState = function(){
        return (video.muted === true) ? "disable" : "enable";
    }

    this.stop = function(){
        if(video.played)
            video.pause();
    }

    this.start = function(){
        if(video.paused)
            video.play();
    }

    this.duration = function(){
        return video.duration;
    }

    this.disableAudio = function(){
        video.muted = true;
    }

    this.enableAudio = function(){
        video.muted = false;
    }

    this.setVolume = function(level){
        video.volume = level;
    }

    this.disableLoop = function(){
        video.loop = false;
    }

    this.enableLoop = function(){
        video.loop = true;
    }

    this.disableAutoplay = function(){
        video.autoplay = false;
    }

    this.enableAutoplay = function(){
        video.autoplay = true;
    }
}
