console.log("This is service worker talking!");
const version = 'v3';
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
        caches.open(version)
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
        //caches.match(event.request, { cacheName: version, ignoreVary:true })
        caches.match(event.request)
            .then(function (res) {
                if (res) {
                    return res;
                }
                if (!navigator.onLine) {

                    var urlString = event.request.url;
                    if (urlString.includes('/RealEstates/')) {

                        
                        


                        // testing with short url whithout options
                        var shortRequestNoOptions = new Request('https://localhost:44373/offlineEstate.json');
                        var matchShortNoOptions= caches.match(shortRequestNoOptions);

                        // testing with short url
                        var shortRequest= new Request('https://localhost:44373/offlineEstate.json', { headers: { 'Content-Type': 'application/json' } });

                        var matchShort         = caches.match(shortRequest);
                        // testing with full url and options
                        var offlineRequest = new Request('https://localhost:44373/offlineEstate.json', { headers: { 'Content-Type': 'application/json' } });
                        var matchLong = caches.match(offlineRequest);


                        offlineMatch = caches.match(offlineRequest);
                        
                        caches.open(version).then(function (cache) {
                            cache.matchAll('/offlineEstate.json').then(function (response) {
                                response.forEach(function (element, index, array) {
                                    console.log(element.url);
                                    console.log(element.body);
                                });
                            });
                        });
                        return offlineMatch;
                    }
                    //event.request.url
                    //return new Response('<h1> Offline :( </h1>', { headers: { 'Content-Type': 'text/html' } });
                    return caches.match(new Request('/Content/offline'));
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
                })
                .catch(error => {
                    console.log(error);
                });
        }
    });
}