function loginResource(language) {
    if (connectionServer) {
        
        $.ajax({
            url: urlPreffix + "/api/ParamsApi/GetResource/" + language,
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'GET',
            success: function (listObject) {
                if (listObject) {
                    $(".Resource").remove();
    
                    if ($(".Resource").length == 0) {
                        appendDevice($("<div class='Resource hide'></div>"), $("body"));
                    }
    
                    $(".Resource").attr("language", language);
    
                    listObject.forEach(function (self) {
                        appendDevice("<div res='" + self._key + "'>" + self._value + "</div>", $(".Resource"));
                    });
                    _writeFile("Resource.txt", $(".Resource").html());
                } else {
                    console.log("Error: Resource " + language + " not found");
                }
            },
            error: function (e) {
                loadListsReprocessos();
            }
        });
    }       
}
function loadResource(){
    _readFile("Resource.txt", function (r) {
        if ($(".Resource").length == 0) {
            appendDevice($("<div class='Resource hide'></div>"), $("body"));
        }
        $(".Resource").empty();
        appendDevice(r, $(".Resource"));
        $(".Resource").attr("language", getResource("_language"));
    });
}

function getResource(value) {
    if (value == "") {
        console.log('Resource error: empty value.');
    }
    if ($(".Resource div[res='" + value + "']").length == 0) {
        console.log('Resource error: ' + value + ".");
    }
    return $(".Resource div[res='" + value + "']").text();
}