var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    minifyCss = require('gulp-cssmin');

gulp.task('process-fonts', function () {
    return gulp.src('bower_components/bootstrap/dist/fonts/*')
        .pipe(gulp.dest('Content/fonts'));
});

gulp.task('process-styles', function () {
    return gulp.src([
        'bower_components/bootstrap/dist/css/bootstrap-datepicker3.standalone.css',
        'bower_components/toastr/toastr.css',
        'bower_components/angular-loading-bar/srs/loading-bar.css',
        'bower_components/bootstrap/dist/css/bootstrap.css',
        'Content/css/common.css'])
        .pipe(concat('app.css'))
        .pipe(minifyCss())
        .pipe(gulp.dest('Content/css'));
});

gulp.task('process-scripts', function () {
    return gulp.src([
        'bower_components/jquery/dist/jquery.js',
        'bower_components/bootstrap/dist/js/bootstrap.js',
        'bower_components/toastr/toastr.js',
        'bower_components/angular/angular.js',
        'bower_components/angular-route/angular-route.js',
        'bower_components/angular-loading-bar/build/loading-bar.js',
        'bower_components/bootstrap-datepicker/js/bootstrap-datepicker.js',
        'Scripts/app/app.js',
        'Scripts/app/modules/app.core.module.js',
        'Scripts/app/modules/app.layout.module.js',
        'Scripts/app/layout/top-bar.directive.js',
        'Scripts/app/services/data.service.js',
        'Scripts/app/services/notification.service.js',
        'Scripts/app/services/membership.service.js',
        'Scripts/app/home/root.controller.js',
        'Scripts/app/home/index.controller.js',
        'Scripts/app/account/login.controller.js',
        'Scripts/app/account/register.controller.js',
        'Scripts/app/products/products.controller.js',
        'Scripts/app/products/products-add.controller.js',
        'Scripts/app/products/products-details.controller.js',
        'Scripts/app/products/products-edit.controller.js',
        'Scripts/app/customers/customers.controller.js',
        'Scripts/app/customers/customers-add-juridical-person.controller.js',
        'Scripts/app/customers/customers-add-natural-person.controller.js',
        'Scripts/app/customers/customers-details-juridical-person.controller.js',
        'Scripts/app/customers/customers-details-natural-person.controller.js',
        'Scripts/app/customers/customers-edit-juridical-person.controller.js',
        'Scripts/app/customers/customers-edit-natural-person.controller.js',
        'Scripts/app/orders/orders.controller.js',
        'Scripts/app/orders/orders-add.controller.js',
        'Scripts/app/orders/orders-edit.controller.js',
        'Scripts/app/components/compare-to.directive.js'])
        .pipe(concat('app.js'))
        .pipe(uglify())
        .pipe(gulp.dest('Scripts'));
});

gulp.task('build-spa', ['process-scripts', 'process-styles', 'process-fonts']);
