var cameraOptions = {
    quality: 100,
    destinationType: Camera.DestinationType.DATA_URL,
    targetWidth: 1280,
    targetHeight: 720,
    correctOrientation: true, 
    sourceType: Camera.PictureSourceType.PHOTOLIBRARY
};

function abrirCamera(cameraSuccess, cameraError, cameraOptions) {
    navigator.camera.getPicture(cameraSuccess, cameraError, cameraOptions);
}

function cameraError(e) {
    console.log("camera error");
}