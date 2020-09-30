console.log("This is service worker talking!");
var cacheName = 'app-bzcl';
var filesToCache = [
    './',
       //Html and css files
    './index.html',
    './css/app.css',
    //'./lib/bootstrap/dist/css/bootstrap.min.css',
    
    // js libraries
    //'./lib/jquery/jquery.min.js',
    //'./lib/bootstrap/dist/js/bootstrap.bundle.min.js',
    './css/bootstrap/bootstrap.min.css',
    './css/custom-css/StarSheet.css',
    './css/custom-css/CommentSheet.css',

    ////blazor framework
    //'./_framework/wasm/mono.js',
    //'./_framework/wasm/mono.wasm',
    //'./_framework/blazor.boot.json',
    //'./_framework/blazor.webassembly.js',

    //    //Our additional files
    './favicon.ico',
    './manifest.json',
    './blazorserviceworker.js',
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

    event.waitUntil(self.clients.claim());
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request, { ignoreSearch: true }).then(response => {
            return response || fetch(event.request);
        })
    );
});




//var filesToCache = [
//    './',
//    //Html and css files
//    './index.html',
//    './css/app.css',
//    './lib/bootstrap/dist/css/bootstrap.min.css',
//    //'./css/open-iconic/font/css/open-iconic-bootstrap.min.css',
//    './css/open-iconic/font/fonts/open-iconic.woff',
//    //'./styles/style.min.css',
//    //'./scripts/application.min.js',

//    // js libraries
//    './lib/jquery/jquery.min.js',
//    './lib/bootstrap/dist/js/bootstrap.bundle.min.js',

//    //Blazor framework
//    //'./_framework/wasm/mono.js',
//    //'./_framework/wasm/mono.wasm',
//    //'./_framework/blazor.boot.json',
//    //'./_framework/blazor.webassembly.js',

//    //Our additional files
//    './favicon.ico',
//    './manifest.json',
//    './blazorServiceWorker.js',
//    //'./icons/icon-192x192.png',
//    './icon-512.png'

//    //The web assembly/.net dll's
//    //'./_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
//    //'./_framework/_bin/Microsoft.AspNetCore.Components.Browser.dll',
//    //'./_framework/_bin/Microsoft.AspNetCore.Components.dll',
//    //'./_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
//    //'./_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
//    //'./_framework/_bin/Microsoft.JSInterop.dll',
//    //'./_framework/_bin/Mono.Security.dll',
//    //'./_framework/_bin/Mono.WebAssembly.Interop.dll',
//    //'./_framework/_bin/mscorlib.dll',
//    //'./_framework/_bin/System.ComponentModel.Annotations.dll',
//    //'./_framework/_bin/System.Core.dll',
//    //'./_framework/_bin/System.dll',
//    //'./_framework/_bin/System.Net.Http.dll'

//    //The compiled project .dll's
//    //'./_framework/_bin/App.BzCl.dll'
//];
