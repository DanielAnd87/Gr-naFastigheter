console.log("This is service worker talking!");
var CACHE_NAME = 'grona-fast-cache';
var urlsToCachebck = [
    './',
    //Html and css files
    './index.html',
    './css/app.dev.css',
    './css/bootstrap/bootstrap.min.css',
    './css/open-iconic/font/css/open-iconic-bootstrap.min.css',
    './css/open-iconic/font/fonts/open-iconic.woff',
    //Blazor framework
    './_framework/blazor.webassembly.js',
    './_framework/blazor.boot.json',
    //Our additional files
    './manifest.json',
    './LinusServiceWorker.js',
    //'./icons/icon-192x192.png',
    //'./icons/icon-512x512.png',
    //The web assembly/.net dll's
    './_framework/wasm/dotnet.js',
    './_framework/wasm/dotnet.wasm',
    './_framework/_bin/WebAssembly.Net.Http.dll',
    './_framework/_bin/Microsoft.AspNetCore.Blazor.HttpClient.dll',
    './_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
    './_framework/_bin/Microsoft.AspNetCore.Components.dll',
    './_framework/_bin/Microsoft.AspNetCore.Components.Web.dll',
    './_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
    './_framework/_bin/Microsoft.JSInterop.dll',
    './_framework/_bin/mscorlib.dll',
    './_framework/_bin/System.Net.Http.dll',
    './_framework/_bin/Mono.WebAssembly.Interop.dll',
    './_framework/_bin/System.dll',
    './_framework/_bin/System.Core.dll',
    './_framework/_bin/Microsoft.Bcl.AsyncInterfaces.dll',
    './_framework/_bin/Microsoft.Extensions.Configuration.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll',
    './_framework/_bin/Microsoft.Extensions.Primitives.dll',
    './_framework/_bin/Microsoft.Extensions.Configuration.dll',
    './_framework/_bin/System.Text.Encodings.Web.dll',
    './_framework/_bin/System.Text.Json.dll',
    './_framework/_bin/WebAssembly.Bindings.dll',
    './_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll',
    //The compiled project .dll's
    './_framework/_bin/DotnetPwaSample.dll'
];
var urlsToCache = [
    '/',
    'https://mockapi-gronafastigheter.herokuapp.com/api/RealEstates?skip=2&take=5',
    //Html and css files
    '/index.html',
    '/css/app.dev.css',
    '/css/bootstrap/bootstrap.min.css',
    '/css/open-iconic/font/css/open-iconic-bootstrap.min.css',
    '/css/open-iconic/font/fonts/open-iconic.woff',
    '/css/custom-css/StarSheet.css',
    '/css/custom-css/CommentSheet.css',
    //Blazor framework
    '/_framework/blazor.webassembly.js',
    '/_framework/blazor.boot.json',
    //Our additional files
    '/manifest.json',
    '/LinusServiceWorker.js',
    '/favicon.ico',
    '/icon-512.png',
    '/appsettings.json',
    '/appsettings.Development.json',
    '/_framework/wasm/dotnet.3.2.0.js',
    //'/_framework/wasm/dotnet.wasm'
    //'/icons/icon-192x192.png',
    //'/icons/icon-512x512.png',
    //The web assembly/.net dll's
    '/_framework/wasm/dotnet.wasm',
    //'/_framework/_bin/WebAssembly.Net.Http.dll',
    //'/_framework/_bin/Microsoft.AspNetCore.Blazor.HttpClient.dll',
    //'/_framework/_bin/Microsoft.AspNetCore.Blazor.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Components.dll',
    '/_framework/_bin/Microsoft.AspNetCore.Components.Web.dll',
    '/_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.DependencyInjection.dll',
    '/_framework/_bin/Microsoft.JSInterop.dll',
    '/_framework/_bin/mscorlib.dll',
    '/_framework/_bin/System.Net.Http.dll',
    //'/_framework/_bin/Mono.WebAssembly.Interop.dll',
    '/_framework/_bin/System.dll',
    '/_framework/_bin/System.Core.dll',
    '/_framework/_bin/Microsoft.Bcl.AsyncInterfaces.dll',
    '/_framework/_bin/Microsoft.Extensions.Configuration.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll',
    '/_framework/_bin/Microsoft.Extensions.Primitives.dll',
    '/_framework/_bin/Microsoft.Extensions.Configuration.dll',
    '/_framework/_bin/System.Text.Encodings.Web.dll',
    '/_framework/_bin/System.Text.Json.dll',
    '/_framework/_bin/WebAssembly.Bindings.dll',
    '/_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll',
    //The compiled project .dll's
    //'/_framework/_bin/DotnetPwaSample.dll'
];
self.addEventListener('install', function (event) {
    // Perform install steps
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(function (cache) {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
    );
});
self.addEventListener('activate', function (event) {

    var cacheAllowlist = ['grona-fast-cache'];

    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (cacheName) {
                    if (cacheAllowlist.indexOf(cacheName) === -1) {
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

self.addEventListener('fetch', function (event) {
    event.respondWith(
        caches.match(event.request)
            .then(function (response) {
                // Cache hit - return response
                if (response) {
                    return response;
                }
                return fetch(event.request);
            }
            )
    );
});