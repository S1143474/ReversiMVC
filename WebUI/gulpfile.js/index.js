const config = require('./config');
const { watch, series } = require('gulp');
// const js = require('./tasks/js').js(config.voornaam);
// js.displayName = 'js';

const sass = require('./tasks/sass').sass(config.localServerProjectPath, config.files.sass);
sass.displayName = 'sass';

const watchFiles = () => {
    watch(['./wwwroot/sass/**/*.scss', './wwwroot/sass/*.scss'], series(sass));
};

const hello = function (done) {
    console.log(`Groeten van ${config.voornaam}`)
    done();
}


exports.default = hello;

// exports.js = js;
exports.watch = watchFiles;