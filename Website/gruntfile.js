module.exports = function (grunt) {

    grunt.initConfig({

       stylus: {
            files: []
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
            src: []
        },

       watch: {
            js: {
                files: ["*.js"],
                tasks: ['jshint', 'karma:unit:run', 'stylus']
            },
            stylus: {
                files: ['**/*.styl'],
                tasks: ['stylus'],

            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-watch');
  
    grunt.registerTask('default', ['jshint']);
};
