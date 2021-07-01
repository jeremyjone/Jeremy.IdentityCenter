window.addEventListener("load", function () {
    var second = 5;
    setText(second);
    setInterval(function() {
            setText(second--);

            if (second === 0) {
                window.location = "/Home";
            }
        },
        1000);
});

function setText(text) {
    $("#second").text(text);
}
