        $(function(){
            var drivers = new Array();
            var infoWindow;
            var map;
            var driverHub;
            var driver;
            var updateInterval;


            var addDriver = function (id, connId, lat, lng, name) {
                var image;
                if (myId == id) {
                    image = "./Content/DriverIcons/cabRed.png";
                }
                else
                    image = "./Content/DriverIcons/cab.png";

                driver = {
                    conId : connId,
                    marker : new google.maps.Marker({
                        map:map,
                        position: { lat: lat, lng: lng},
                        animation: google.maps.Animation.BOUNCE,
                        icon: image,
                        draggable: true,
                        title: name
                    })
                };
               // if (drivers[id] != undefined)
                    //removeDriver(drivers[id].conId);
                drivers[id] = driver;

                console.log(drivers);
            };

            var removeDriver = function (id) {
                drivers.forEach(function (e, index, array) {
                    if (e && e.conId == id) {
                        array[index].marker.setMap(null);
                        array[index] = undefined;
                    }
                })

            };

            var updateCoord = function (id, connID, lat, lng, name) {
                console.log("inside update")
                if (drivers[id]!= undefined)
                 removeDriver(drivers[id].conId);
                addDriver(id, connID, lat, lng, name);

            };

            var getPossition = function () {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        var pos = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };

                        //markers[myId].setMap(map);
                        //  infoWindow.setPosition(pos);

                        // infoWindow.setContent();
                        map.setCenter(pos);
                        driverHub.server.addDriver(myId, pos.lat, pos.lng, myName);
                    }, function () {
                        handleLocationError(true, infoWindow, map.getCenter());
                    });
                }

            }


            var initHub = function (doneHandler) {
                console.log("inside inithub");

                driverHub = $.connection.driverLocationHub;


                driverHub.client.addDriver =  addDriver;
                driverHub.client.updateDriverCoord = updateCoord;

                driverHub.client.remove = removeDriver;

                $.connection.hub.start().done(doneHandler);
            }





            function initMap() {
                map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 15,
                    center: { lat: 48.290718, lng: 25.934960 }
                });

            //    infoWindow = new google.maps.InfoWindow({ map: map });
                initHub(function() {
                    getPossition();
                    initHubDriverMessage();
                    updateInterval = setInterval(collServerUpdate, 5000);
               
                });
            }

            var collServerUpdate = function () {
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        var pos = {
                            lat: position.coords.latitude,
                            lng: position.coords.longitude
                        };
                        driverHub.server.updateDriverCoord(myId, 1, pos.lat, pos.lng, myName)
                    })
                };

              
            }

            google.maps.event.addDomListener(window, 'load', initMap);
        });

 