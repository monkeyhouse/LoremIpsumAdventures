/// <binding Clean='clean, sass:watch' ProjectOpened='sass:watch' />
var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass"),
    //bundler = require('aurelia-bundler'),
    project = require("./project.json");

var paths = {
    webroot: "./" + project.webroot + "/"
}; 

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.sass = paths.webroot + "style/sass/*.scss";
paths.spaSass = paths.webroot + "style/sass/spa/**/*.scss";
paths.css = paths.webroot + "style/css/";
paths.spaCss = paths.webroot + "style/css/spa/";
paths.minCss = paths.webroot + "style/css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "style/css/site.min.css";

gulp.task("sass", function () {
    gulp.src(paths.sass)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.css));

    gulp.src(paths.spaSass)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.spaCss));
});

gulp.task('sass:watch', function () {
    gulp.watch(paths.sass, ['sass']);
    gulp.watch(paths.spaSass, ['sass']);
});

gulp.task("clean:sass", function () {
    rimraf(paths.styleDest, cb);
});

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);


///* bundler config */
//var config = {
//    force: true,
//    packagePath: paths.webroot,
//    bundles: {
//        "spa/playAppClone": {
//            includes: [
//              paths.webroot + "spa/playAppClone/" + '*',
//              paths.webroot + "spa/playAppClone/" + '*.html!text',
//              paths.webroot + "spa/playAppClone/" + '*.css!text'
//            ],
//            options: {
//                inject: true,
//                minify: true
//            }
//        },
//        "dist/aurelia": {
//            includes: [
//              'aurelia-bootstrapper',
//              'aurelia-fetch-client',
//              'aurelia-router',
//              'aurelia-animator-css',
//              'github:aurelia/templating-binding',
//              'github:aurelia/templating-resources',
//              'github:aurelia/templating-router',
//              'github:aurelia/loader-default',
//              'github:aurelia/history-browser',
//              'github:aurelia/logging-console'
//            ],
//            options: {
//                inject: true,
//                minify: true
//            }
//        }
//    }
//};


//gulp.task('bundle', function () {
//    return bundler.bundle(config);
//});

//gulp.task('unbundle', function () {
//    return bundler.unbundle(config);
//});