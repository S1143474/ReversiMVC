const { src, dest } = require('gulp');

const fn = (voornaam) => {
    return () => {
        return src('js/*.js').pipe(dest('dist'));
    };
};