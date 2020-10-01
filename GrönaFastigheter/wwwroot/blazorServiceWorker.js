console.log("This is service worker talking!");
const version = 'v3';
var cacheName = 'app-bzcl';
var filesToCache = [
    './',
       //Html and css files
    './index.html',
    './css/app.css',
    './css/bootstrap/bootstrap.min.css',
    './css/custom-css/StarSheet.css',
    './css/custom-css/CommentSheet.css',


    //    //Our additional files
    './favicon.ico',
    './manifest.json',
    './blazorserviceworker.js',
    './offlineEstate.json',
    './icon-512.png'
];

self.addEventListener('install', function (e) {
    console.log('[ServiceWorker] Install');
    e.waitUntil(
        caches.open(cacheName)
            .then(function (cache) {
            console.log('[ServiceWorker] Caching app shell');
            return cache.addAll(filesToCache);
            })
            .catch((error) => {
                console.log(error)
            })
    );
});

self.addEventListener('activate', event => {
    console.log('[ServiceWorker] waiting for activate');
    event.waitUntil(
        caches.keys()
            .then(function (keys) {
                return Promise.all(keys.filter(function (key) {
                    return key !== version;
                }).map(function (key) {
                    // todo: This deletes all other cashes and clones it in a new one, wich is good, but blazor therefore has to recach despite that we has made a copy.
                    return caches.delete(key);
                }));
            }));
});

self.addEventListener('fetch', event => {
    console.log('fetching ' + (event.request.url));
    testLogging(event);
    event.respondWith(
        caches.match(event.request)
            .then(function (res) {
                if (res) {
                    return res;
                }
                if (!navigator.onLine) {

                    var urlString = event.request.url;
                    if (urlString.includes('/RealEstates/')) {
                        var offlineRequest = new Request('https://mockapi-gronafastigheter.herokuapp.com/api/RealEstates/1');
                        return caches.match(offlineRequest);
                    }
                    //event.request.url
                    //return new Response('<h1> Offline :( </h1>', { headers: { 'Content-Type': 'text/html' } });
                    return caches.match(new Request('./Content/offline'));
                }
                return fetchAndUpdate(event.request);
            }));
});

function testLogging(event) {
    var urlString = event.request.url;
    console.log(urlString.includes('localhost'));
}

function fetchAndUpdate(request) {
    return fetch(request)
    .then(function (res) {
        if (res) {
            return caches.open(version)
            .then(function (cache) {
                return cache.put(request, res.clone())
                    .then(function () {
                        return res;
                    })
            });
        }
    });
}