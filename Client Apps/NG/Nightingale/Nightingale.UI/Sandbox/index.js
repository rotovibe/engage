define([], function () {

    var sandbox = {
        activate: activate
    }
    return sandbox;

    function activate() {
        console.log('Sandbox activated');
    }
});