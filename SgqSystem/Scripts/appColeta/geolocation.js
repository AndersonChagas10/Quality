var geolocationOptions = { maximumAge: 3000, timeout: 5000, enableHighAccuracy: true };

function getLatLong(){
    navigator.geolocation.getCurrentPosition(
        geolocationSuccess,
        geolocationError,
        geolocationOptions);
}

function geolocationSuccess(position){    
    console.log("geolocation sucess");

    level3PhotoTemp.latitude = position.coords.latitude;
    level3PhotoTemp.longitude = position.coords.longitude;

	abrirCamera();
}

function abrirCamera(){
    navigator.camera.getPicture(cameraSuccess, cameraError, cameraOptions);
}

function geolocationError(){
    openMessageModal("Para abrir a câmera, o GPS precisa estar ligado.", "");
}
