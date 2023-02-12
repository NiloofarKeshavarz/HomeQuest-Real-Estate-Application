// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// [START maps_places_autocomplete]
// This example requires the Places library. Include the libraries=places
// parameter when you first load the API. For example:
// <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places">
// function initMap() {
//     const map = new google.maps.Map(document.getElementById("map"), {
//       center: { lat: 45.508888, lng: -73.561668 },
//       zoom: 13,
//       mapTypeControl: false,
//     });
//     // const card = document.getElementById("pac-card");
//     const input = document.getElementById("pac-input");
//     const biasInputElement = document.getElementById("use-location-bias");
//     const strictBoundsInputElement = document.getElementById("use-strict-bounds");
//     const options = {
//       fields: ["formatted_address", "geometry", "name", "postal_code"],
//       strictBounds: false,
//       types: ["address"],
//     };
  
//     map.controls[google.maps.ControlPosition.TOP_LEFT].push(card);
  
//     const autocomplete = new google.maps.places.Autocomplete(input, options);
  
//     // Bind the map's bounds (viewport) property to the autocomplete object,
//     // so that the autocomplete requests use the current map bounds for the
//     // bounds option in the request.
//     // [START maps_places_autocomplete_bind]
//     autocomplete.bindTo("bounds", map);
  
//     // [END maps_places_autocomplete_bind]
//     const infowindow = new google.maps.InfoWindow();
//     const infowindowContent = document.getElementById("infowindow-content");
  
//     infowindow.setContent(infowindowContent);
  
//     const marker = new google.maps.Marker({
//       map,
//       anchorPoint: new google.maps.Point(0, -29),
//     });
  
//     autocomplete.addListener("place_changed", () => {
//       infowindow.close();
//       marker.setVisible(false);
  
//       const place = autocomplete.getPlace();
  
//       if (!place.geometry || !place.geometry.location) {
//         // User entered the name of a Place that was not suggested and
//         // pressed the Enter key, or the Place Details request failed.
//         //window.alert("No details available for input: '" + place.name + "'");
//         return;
//       }
  
//       // If the place has a geometry, then present it on a map.
//       if (place.geometry.viewport) {
//         map.fitBounds(place.geometry.viewport);
//       } else {
//         map.setCenter(place.geometry.location);
//         map.setZoom(17);
//       }
  
//       marker.setPosition(place.geometry.location);
//       marker.setVisible(true);
//       infowindowContent.children["place-name"].textContent = place.name;
//       infowindowContent.children["place-address"].textContent =
//         place.formatted_address;
//       infowindow.open(map, marker);
//     });
  
//     // Sets a listener on a radio button to change the filter type on Places
//     // Autocomplete.
//     function setupClickListener(id, types) {
//       const radioButton = document.getElementById(id);
  
//       radioButton.addEventListener("click", () => {
//         autocomplete.setTypes(types);
//         input.value = "";
//       });
//     }
  
//     setupClickListener("changetype-all", []);
//     setupClickListener("changetype-address", ["address"]);
//     setupClickListener("changetype-establishment", ["establishment"]);
//     setupClickListener("changetype-geocode", ["geocode"]);
//     setupClickListener("changetype-cities", ["(cities)"]);
//     setupClickListener("changetype-regions", ["(regions)"]);
//     biasInputElement.addEventListener("change", () => {
//       if (biasInputElement.checked) {
//         autocomplete.bindTo("bounds", map);
//       } else {
//         // User wants to turn off location bias, so three things need to happen:
//         // 1. Unbind from map
//         // 2. Reset the bounds to whole world
//         // 3. Uncheck the strict bounds checkbox UI (which also disables strict bounds)
//         // [START maps_places_autocomplete_unbind]
//         autocomplete.unbind("bounds");
//         autocomplete.setBounds({ east: 180, west: -180, north: 90, south: -90 });
//         // [END maps_places_autocomplete_unbind]
//         strictBoundsInputElement.checked = biasInputElement.checked;
//       }
  
//       input.value = "";
//     });
//     strictBoundsInputElement.addEventListener("change", () => {
//       autocomplete.setOptions({
//         strictBounds: strictBoundsInputElement.checked,
//       });
//       if (strictBoundsInputElement.checked) {
//         biasInputElement.checked = strictBoundsInputElement.checked;
//         autocomplete.bindTo("bounds", map);
//       }
  
//       input.value = "";
//     });
//   }
  
  // window.initMap = initMap;
  // [END maps_places_autocomplete]


   // // autofill address
   function initMap() {
    // const CONFIGURATION = {
    //     "ctaTitle": "Checkout",
    //     "mapOptions": { "center": { "lat": 56.1304, "lng": -106.3468 }, "fullscreenControl": true, "mapTypeControl": false, "streetViewControl": true, "zoom": 11, "zoomControl": true, "maxZoom": 22, "mapId": "" },
    //     "mapsApiKey": "YOUR_API_KEY_HER",
    //     "capabilities": { "addressAutocompleteControl": true, "mapDisplayControl": true, "ctaControl": true }
    // };
    // addresses in the US and Canada.
    const componentForm = [
        'location',
        // 'locality',
        // 'administrative_area_level_1',
        // 'country',
        'postal_code',
    ];


    const getFormInputElement = (component) => document.getElementById(component + '-input');
    // const map = new google.maps.Map(document.getElementById("map"), {
    //     zoom: CONFIGURATION.mapOptions.zoom,
    //     center: { lat: 56.1304, lng: -106.3468 },
    //     mapTypeControl: false,
    //     fullscreenControl: CONFIGURATION.mapOptions.fullscreenControl,
    //     zoomControl: CONFIGURATION.mapOptions.zoomControl,
    //     streetViewControl: CONFIGURATION.mapOptions.streetViewControl
    // });
    const marker = new google.maps.Marker({ map: map, draggable: false });
    const autocompleteInput = getFormInputElement('location');
    const autocomplete = new google.maps.places.Autocomplete(autocompleteInput, {
        componentRestrictions: { country: ["us", "ca"] },
        fields: ["address_components", "geometry", "name"],
        types: ["address"],
    });
    autocomplete.addListener("place_changed", function () {
        marker.setVisible(false);
        // Get the place details from the autocomplete object.
        const place = autocomplete.getPlace();
        if (!place.geometry) {
            // User entered the name of a Place that was not suggested and
            // pressed the Enter key, or the Place Details request failed.
            window.alert('No details available for input: \'' + place.name + '\'');
            return;
        }
        fillInAddress(place);
        // renderAddress(place);

    });

    function fillInAddress(place) {  // optional parameter
        const addressNameFormat = {
            'street_number': 'short_name',
            'route': 'long_name',
            'locality': 'long_name',
            'administrative_area_level_1': 'short_name',
            'country': 'long_name',
            'postal_code': 'short_name',
        };
        const getAddressComp = function (type) {
            for (const component of place.address_components) {
                if (component.types[0] === type) {
                    return component[addressNameFormat[type]];
                }
            }
            return '';
        };
        getFormInputElement('location').value = getAddressComp('street_number') + ' '
            + getAddressComp('route')+ ', ' +getAddressComp('locality') + ', '+ getAddressComp('administrative_area_level_1') + ', ' +getAddressComp('country');
        for (const component of componentForm) {
            // Location field is handled separately above as it has different logic.
            if (component !== 'location') {
                getFormInputElement(component).value = getAddressComp(component);
            }
        }
    }

    // function renderAddress(place) {
    //     map.setCenter(place.geometry.location);
    //     marker.setPosition(place.geometry.location);
    //     marker.setVisible(true);
    // }

}

