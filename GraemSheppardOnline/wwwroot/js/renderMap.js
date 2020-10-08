mapboxgl.accessToken = 'pk.eyJ1IjoiZ3JhZW1zaGVwcGFyZCIsImEiOiJja2RxbWRkYTAwNXliMnNueXI3M202Mm1tIn0.VidXwnyGPsGDzbdR2daSpQ';

let map;
function renderMap(center) {
    map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/dark-v10',
        center: [center.longitude, center.latitude],
        zoom: 4
    });
}



function renderPoints(points) {
    
    var features = [];
    points.forEach(point => {
        features.push({
            'type': 'Feature',
            'properties': {},
            'geometry': {
                'type': 'Point',
                'coordinates': [point.longitude, point.latitude]
            }
        });
    });

    map.center = features[0].geometry.coordinates;

    map.addSource('pointSource', {
        type: 'geojson',
        data: {
            'type': 'FeatureCollection',
            'features': features
        }
    });

    map.addLayer({
        id: 'points',
        source: 'pointSource',
        type: 'circle',
        paint: {
            'circle-radius': 6,
            'circle-color': 'orange',
            'circle-opacity': 0.8
        }
    });


}
