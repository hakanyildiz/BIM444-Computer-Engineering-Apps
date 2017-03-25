var gulp = require('gulp');
//var vulcanize = require('gulp-vulcanize');
//gulp.task('copy', function () {
//    return gulp.src([
//		'../webcomponentsjs/webcomponents-lite.js'
//    ], {
//        base: 'app'
//    }).pipe(gulp.dest('dist'));
//})

//gulp.task('vulcanize', function() {
//  return gulp.src('imports/kaldirirmi-master-imports.html')
//    .pipe(vulcanize({
//        excludes: ['../polymer/polymer.html'],
//        //stripExcludes: ['../polymer/polymer.html'],
//        stripComments: true,
//        inlineScripts: true,
//        inlineCss: true
//    }))
//    .pipe(gulp.dest('../vulcanized'));
//});

/* DEFAULT PAGE */
//gulp.task('vulcanize', function () {
//    return gulp.src('imports/kaldirirmi-default-page-imports.html').pipe(vulcanize({
//        excludes: ['../polymer/polymer.html'],
//        stripExcludes: ['../polymer/polymer.html'],
//        stripComments: true,
//        inlineScripts: true,
//        inlineCss: true
//    })).pipe(gulp.dest('../vulcanized'));
//});

/* PROFILE PAGE */
//gulp.task('vulcanize', function () {
//    return gulp.src('imports/kaldirirmi-profile-page-imports.html').pipe(vulcanize({
//        excludes: ['../polymer/polymer.html'],
//        stripExcludes: ['../polymer/polymer.html'],
//        stripComments: true,
//        inlineScripts: true,
//        inlineCss: true
//    })).pipe(gulp.dest('../vulcanized'));
//});

/* GAME PAGE */
//gulp.task('vulcanize', function () {
//    return gulp.src('imports/kaldirirmi-game-page-imports.html').pipe(vulcanize({
//        excludes: ['../polymer/polymer.html'],
//        stripExcludes: ['../polymer/polymer.html'],
//        stripComments: true,
//        inlineScripts: true,
//        inlineCss: true
//    })).pipe(gulp.dest('../vulcanized'));
//});

/*minifier */
//var htmlmin = require('gulp-htmlmin');
//gulp.task('minify', function () {
//    return gulp.src('kaldirirmi/hottags/*.html')
//      .pipe(htmlmin({ collapseWhitespace: true }))
//      .pipe(gulp.dest('kaldirirmi/_minified/hottags'));
//});


//gulp.task('default', ['vulcanize']);
//gulp.task('default', ['minify']);