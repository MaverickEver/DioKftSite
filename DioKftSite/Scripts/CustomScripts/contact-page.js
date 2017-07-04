$(function () {
    var map;
    function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: 46.406782, lng: 20.362610 },
            zoom: 15
        });
    }
});