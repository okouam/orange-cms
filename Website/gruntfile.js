module.exports = function (grunt) {

    grunt.initConfig({

        autoprefixer: {
          compile: {
              files: [{
                  src: 'Modules/**/*.css',
                  dest: './',
                  expand: true
              }]
          }
        },

        sass: {
            compile: {
                files: [{
                    src: 'Modules/**/*.scss',
                    dest: './',
                    expand: true,
                    ext: ".css"
                }]
            }
        },

        jshint: {
            options: {
                browser: true,
                jquery: true,
                nonstandard: true,
                bitwise: true,
                curly: true,
                eqeqeq: true,
                freeze: true,
                immed: true,
                latedef: true,
                newcap: true,
                noempty: true,
                nonew: false,
                plusplus: true,
                undef: true,
                unused: true,
                camelcase: true,
                laxbreak: true,
                globals: {
                    "jasmine": true,
                    "alert": true,
                    "confirm": true,
                    "it": true,
                    "beforeEach": true,
                    "inject": true,
                    "afterEach": true,
                    "describe": true,
                    "expect": true,
                    "module": true,
                    "angular": true,
                    "_": true
                }
            },
            src: ["Modules/**/*.js"]
        },

        watch: {
            js: {
                files: ["*.js"],
                tasks: ['jshint']
            },
            stylus: {
                tasks: ['stylus']
            }
        }
    });

    grunt.loadNpmTasks('grunt-autoprefixer');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-sass');
    grunt.loadNpmTasks('grunt-contrib-stylus');
    grunt.registerTask('default', ['sass', 'autoprefixer']);
};
