requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions'
    },
    waitSeconds: 15,
    // Bump this version to have users loading from server instead of cache
    urlArgs: 'v=0.1.7.7'
});

// TODO: Remove this
jQuery.support.cors = true;

define('jquery', function () { return jQuery; });
define('knockout', ko);



define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'durandal/composition'],  function (system, app, viewLocator, composition) {
    //>>excludeStart("build", true);
    system.debug(true);
    //>>excludeEnd("build");

    app.title = 'Nightingale';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: {
            kinds: ['singleselect', 'multiselect', 'chsnsingle', 'chsnsingledark']	//, 'datetimepicker'
        }
    });

    app.start().then(function () {
        //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
        //Look for partial views in a 'views' folder in the root.
        viewLocator.useConvention();
		composition.addBindingHandler('hasFocus');	//fix for durandal bug missing KO binding.
        //Show the app by setting the root view model for our application with a transition.
        app.setRoot('viewmodels/shell/shell');
    });
});