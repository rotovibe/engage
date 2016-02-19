/**	
*	gulpfile - node js based build configuration for Engage Assets (scss/css) 
*		first time: you need to have node.js and gulp installed,
*		then run >npm install from the Nightingale.Assets folder (the package.json will guide npm to install dependencies).
*		
*		building the css:
*			from the Nightingale.Assets folder >gulp buildcss
*		
*		output application.css and application.css.min will be created inside Nightingale.Assets\Styles\CSS
*    
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
	path             = require('path');

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

//styles
gulp.task('styles', function() {
	return gulp.src(['Styles/**/*.scss', '!Styles/compass/**/*.*'])
		.pipe(plumber(plumberErrorHandler))
		.pipe(compass({
			css: 'Styles/CSS',	//output the generated css here
			sass: 'Styles',		//sources
			image: 'Images'
		}))
		.pipe(gulp.dest('Styles/css'))
		.pipe(rename({ suffix: '.min' }))
		.pipe(minifycss())
		.pipe(gulp.dest('Styles/css'));
	
});

//synchronized execute cleancss then styles then run parallel: prepend copyright text to css and mini:

gulp.task('buildcss', function(d){				
	sequence('cleancss', 'styles', ['copyrightCss', 'copyrightMiniCss'] );	
});

gulp.task('cleancss', function() {
    gulp.src('Styles/CSS/*.*').pipe(rm());
	return setTimeout(function(){ return; }, 2000);
});

gulp.task('copyrightCss', function(){
	return gulp.src(['copyright.js', 'Styles/CSS/application.css'])
		.pipe(concat('Styles/CSS/application.css'))
		.pipe(gulp.dest('./'));
});

gulp.task('copyrightMiniCss', function(){
	return gulp.src(['copyright.js', 'Styles/CSS/application.min.css'])
		.pipe(concat('Styles/CSS/application.min.css'))
		.pipe(gulp.dest('./'));
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