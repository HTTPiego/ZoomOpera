function Alert(message) {
    alert(message);
};

function GetWidth() {
    var img = document.getElementById('image');
    return img.clientWidth;
};

function GetHeight() {
    var img = document.getElementById('image');
    return img.clientHeight;
};


//var height = img.clientHeight;

//function relMouseCoords(event) {
//    var totalOffsetX = 0;
//    var totalOffsetY = 0;
//    var canvasX = 0;
//    var canvasY = 0;
//    var currentElement = this;

//    do {
//        totalOffsetX += currentElement.offsetLeft - currentElement.scrollLeft;
//        totalOffsetY += currentElement.offsetTop - currentElement.scrollTop;
//    }
//    while (currentElement = currentElement.offsetParent)

//    canvasX = event.pageX - totalOffsetX;
//    canvasY = event.pageY - totalOffsetY;

//    return { X: canvasX, Y: canvasY }
//};

















