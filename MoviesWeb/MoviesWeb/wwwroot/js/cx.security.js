var apiToken = '';
var apiUrl = '';
var currentUser = '';
var userId = ''; // Add user ID

// Иницијализација на патеката и токенот за CodexIntraAPI за користење во js
function cxInit(inApiUrl, inApiToken, inCurrentUser) {
    apiUrl = inApiUrl;
    apiToken = inApiToken;
    currentUser = inCurrentUser;
}

// Додавање на токенот во header-от на request-от при повик на CodexIntraAPI од Devextreme грид
function addTokenToHeader(r, s) {
    s.headers = { Authorization: apiToken };
}

function stringSanitizeXss(str) {
    return String(str).replace(/[^\w. ]/gi, function (c) {
        return '&#' + c.charCodeAt(0) + ';';
    });
}
