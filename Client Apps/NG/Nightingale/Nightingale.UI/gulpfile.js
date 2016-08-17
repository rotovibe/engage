/**
*	gulpfile - node js based build configuration for Engage UI js
*	first time: you need to have node.js and gulp installed,
*	then run >npm install from the Nightingale.UI folder (the package.json will guide npm to install dependencies).
*	
*	building the js:
*		1. make sure everything is saved in the app/test folder. (or move it away)
*		2. on folder Nightingale.UI> gulp buildjs
*
*        notes: 
*        1. the build includes a workaround ('movex' task) to exclude (by moving away) the app/test/ from the durandal js build.
*            if the test folder is included, the main-built.js file will be generated but corrupted and wont work!!
*
*        2. the nightingale.UI project also has a weyland build configured. to build with weyland:
*            on folder Nightingale.UI> weyland build
*
*            BUT - the generated main-built.js will not be as compact as the gulp buildjs does, and copyright text wont be prepended.
*/

//load plugins
var gulp             = require('gulp'),
	compass          = require('gulp-compass'),
	autoprefixer     = require('gulp-autoprefixer'),
	minifycss        = require('gulp-cssnano'),
	uglify           = require('gulp-uglify'),
	rename           = require('gulp-rename'),
	concat           = require('gulp-concat'),
	notify           = require('gulp-notify'),
	livereload       = require('gulp-livereload'),
	plumber          = require('gulp-plumber'),
	durandal 		 = require('gulp-durandal'),
	rm 			 	 = require('gulp-rimraf'),
	sequence 		 = require('run-sequence'),
	gutil 			 = require('gulp-util'),
	path             = require('path'),
	jshint			 = require('gulp-jshint'),
	jscs 			 = require('gulp-jscs');

//the title and icon that will be used for the Grunt notifications
var notifyInfo = {
	title: 'Gulp',
	icon: path.join(__dirname, 'gulp.png')
};

//error notification settings for plumber
var plumberErrorHandler = { errorHandler: notify.onError({
		title: notifyInfo.title,
		icon: notifyInfo.icon,
		message: "Error: <%= error.message %>"
	})
};


//////////////////////////////////////
// build js tasks:

gulp.task('buildjs', function(d){	
	sequence('cleanjs', 'movex', 'cleanx', 'lint', 'durandal', ['copyrightJs'], 'moveback', 'cleanback' );	
});

gulp.task('cleanjs', function() {
    return gulp.src('App/main-built*.*').pipe(rm());
});

gulp.task('lint', function(){
    return gulp.src(['App/**/*.js', '!App/main-built*.*'])
        .pipe(jshint())  // enforce jshint
        .pipe(jshint.reporter('jshint-stylish', {verbose: true}))
        .pipe(jscs())  // enforce style guide
		.pipe(jscs.reporter());
});

//durandal
gulp.task('durandal', function() {	
	
	//this task is producing main-built.js but the js will not work if we do not exclude / move the test folder out!!
	var isDev = process.argv.indexOf("--dev") == -1 ? false : true;
    return durandal({
            baseDir: 'App',   //same as default, so not really required.
            main: 'main.js',  //same as default, so not really required.
            output: 'main-built.js', //default is main.js
            almond: true,
            minify: !isDev,
			verbose: false			
        })
        .pipe(gulp.dest('App'));	
});


gulp.task('copyrightJs', function(){
	return gulp.src(['copyright.js', 'App/main-built.js'])
		.pipe(concat('App/main-built.js'))
		.pipe(gulp.dest('./'));
});

//the following is a workaround to exclude the app/test/ from the durandal js build: move it aside:
//	if the test folder is included, the main-built.js file will be generated but corrupted and wont work!!
//(1)
gulp.task('movex', function(){
	//copy the test folder
	return gulp.src(['App/test/**/*'])
	.pipe(gulp.dest('./App_temp_test/'));	
});
//(2)
gulp.task('cleanx', function(){
	//clean the test folder so it wont participate in the js build:
	return gulp.src(['App/test/**/*']).pipe(rm());
});

gulp.task('moveback', function(){
	//copy the test folder back:	
	return gulp.src(['./App_temp_test/**/*'])
	.pipe(gulp.dest('App/test/'));
});

gulp.task('cleanback', function(){
	return gulp.src(['./App_temp_test/**/*']).pipe(rm());
});
////////////////////


//watch -- not yet tested
gulp.task('live', function() {
	livereload.listen();

	//watch .scss files
	gulp.watch('Styles/**/*.scss', ['application']);

	//watch .js files
	gulp.watch('App/**/*.js', ['scripts']);

	//reload when a template file, the minified css, or the minified js file changes
	gulp.watch('App/**/*.html', 'html/css/application.min.css', 'html/js/main-built.min.js', function(event) {
		gulp.src(event.path)
			.pipe(plumber())
			.pipe(livereload())
			.pipe(notify({
				title: notifyInfo.title,
				icon: notifyInfo.icon,
				message: event.path.replace(__dirname, '').replace(/\\/g, '/') + ' was ' + event.type + ' and reloaded'
			})
		);
	});
});