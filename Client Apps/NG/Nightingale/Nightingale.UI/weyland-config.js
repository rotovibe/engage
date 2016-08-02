exports.config = function(weyland) {
  weyland.build('main')
    .task.jshint({
      exclude: ['App/main-built.js', 'App/exclmodules/**/*.*', 'App/test/*.*'],
      include:'App/**/*.js'
    })
    .task.uglifyjs({
      exclude: ['App/main-built.js', 'App/exclmodules/**/*.*', 'App/test/*.*', 'App/main.js'],
      include:['App/**/*.js', 'Scripts/durandal/**/*.js']
    })
    .task.rjs({
      include:['App/**/*.{js,html}', 'Scripts/durandal/**/*.js'],
      exclude: ['App/main-built.js', 'App/exclmodules/**/*.*', 'App/test/*.*' ],
      loaderPluginExtensionMaps:{
        '.html':'text'
      },
      rjs:{
        name:'../Scripts/almond-custom', //to deploy with require.js, use the build's name here instead
        insertRequire:['main'], //not needed for require
        baseUrl : 'App',
        wrap:true, //not needed for require
        paths : {
          'text': '../Scripts/text',
          'durandal': '../Scripts/durandal',
          'plugins': '../Scripts/durandal/plugins',
          'transitions': '../Scripts/durandal/transitions',
          'knockout': 'empty:',
          'jquery': 'empty:'
        },
        waitSeconds: 15,
        urlArgs: 'v=0.1.6.0',
        inlineText: true,
        optimize : 'none',
        pragmas: {
            build: true
        },
        stubModules : ['text'],
        keepBuildDir: true,
        out:'App/main-built.js'
      }
  });
}
