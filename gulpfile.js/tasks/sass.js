const { src, dest } = require('gulp');
const gulpSass = require('gulp-sass')(require('sass'));
const concat = require('gulp-concat');

const sass = (localServerProjectPath, sass_files) => {
    return () => {
        return src(sass_files)
            .pipe(gulpSass().on('error', gulpSass.logError))
            // .pipe(dest('./dist/sass'))
            // .pipe(concat('style.min.css'))
            // .pipe(dest('./dist/css'))
            .pipe(dest(localServerProjectPath + 'css'));
    }
};

exports.sass = sass;