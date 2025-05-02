// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function clearSearchTerm() {
    // Get current URL and remove the 'searchterm' query parameter
    var currentUrl = new URL(window.location.href);
    currentUrl.searchParams.delete("searchterm");

    // Get the index URL from the data attribute
    var indexUrl = document.getElementById('resetBtn').getAttribute('data-index-url');

    // Redirect to the Index action with the searchterm removed (i.e., without search term)
    window.location.href = indexUrl;
};
