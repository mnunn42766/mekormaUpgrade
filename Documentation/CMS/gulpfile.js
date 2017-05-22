var gulp = require('gulp'),
    concat = require('gulp-concat'),
    insert = require('gulp-insert'),
    templateCache = require('gulp-angular-templatecache'),
    htmlify = require('gulp-angular-htmlify'),
    ngAnnotate = require('gulp-ng-annotate'),
    glob = require('glob'),
    jsonfile = require('jsonfile'),
    path = require('path'),
    through = require('through2'),
    slash = require('slash'),
    toSingleQuotes = require('to-single-quotes'),
    fs = require('fs');
    jshint = require('gulp-jshint'),
    _ = require('lodash');

// Specifies root path of all files processed by the gulp
var rootPath = 'CMSScripts/CMSModules/',

    // Collection of angular application obtained from the config files
    angularApps = [];

gulp.task('jsHint', function () {
    if (!angularApps.length) {
        findAngularApps();
    }

    //exclude build result from jshint
    var sourceFiles = ['!./**/build.js'];

    //process all files in angular apps
    angularApps.forEach(function (angularApp) {
        sourceFiles.push(angularApp.appRoot + '**/*.js');
    });

    return gulp.src(sourceFiles)
        .pipe(jshint())
        .pipe(jshint.reporter('gulp-jshint-html-reporter', {
            filename: __dirname + '/jshint-output.html',
            createMissingFolders: false
        }));
});

/**
 * Builds all the angular apps. At first, finds all the angular applications using their congif files,
 * then build the templates and finally concatenates all the angular JS files into a single one.
 */
gulp.task('buildAngular', function () {
    if (!angularApps.length) {
        findAngularApps();
    }

    angularApps.forEach(function (angularApp) {
        buildAngularTemplates(angularApp.appRoot).on('end', function () {
            buildAngularJS(angularApp);
        });
    });
});


/**
 * Runs the watch for the changes in angular files.  At first, finds all the angular applications using their congif files,
 * then watches the changes of related JS and HTML files. Every change invokes 'buildAngular' taks;
 */
gulp.task('watch', function () {
    if (!angularApps.length) {
        findAngularApps();
    }
    angularApps.forEach(function (angularApp) {
        var sourceFiles = [angularApp.appRoot + angularApp.angularBootstrap || '', angularApp.appRoot + '**/*.js', angularApp.appRoot + '**/*.html'];
        gulp.watch(sourceFiles, ['buildAngular']);
    });
});


/**
 * Builds the angular JS files together with the template cache built earlier. Makes sure angular bootstrap is concatenated as the first script.
 * Wraps the concatenated result with cmsdefine function to support the AMD resolution. The result is stored next to the application root in build.js file.
 * @param {object} angularApp Angular application object to be built obtained earlier with @see findAngularApps.
 */
function buildAngularJS(angularApp) {
    var sourceFiles = [angularApp.appRoot + angularApp.angularBootstrap || '', angularApp.appRoot + '**/*.js', angularApp.appRoot + '../build.templates.js'];
    return gulp.src(sourceFiles)
        .pipe(concat('build.js'))
        .pipe(ngAnnotate())
        .pipe(gulp.dest(function () {
            return angularApp.appRoot + '/../../../../../../Tests/AngularUI.Tests/scripts/' + getModuleRoot(angularApp.appRoot);
        }))
        .pipe(insert.wrap(getBuildFileHeader(angularApp), getBuildFileFooter(angularApp)))
        .pipe(gulp.dest(function () {
            return angularApp.appRoot + '/..';
        }));
}


/**
 * Builds the angular templates into single template cache. Adds data- prefixes to all used ng- attribute directives to gain the HTML validity.
 * The result is stored next to the application root in build.templates.js file.
 * @param {string} appRoot path to the application root
 */
function buildAngularTemplates(appRoot) {
    var moduleRoot = getModuleRoot(appRoot),
        sourceFiles = appRoot + '**/*.html';
    return gulp.src(sourceFiles)
        .pipe(htmlify())
        .pipe(singleQuotes())
        .pipe(resourceStrings('../build.localization.json'))
        .pipe(templateCache('build.templates.js', {
            module: moduleRoot + 'app.templates', standalone: true, transformUrl: function (url) {
                return moduleRoot + url;
            }
        }))
        .pipe(gulp.dest(function (file) {
            return file.base + '/..';
        }));
}


/**
 * Converts all double quotes withing the given stream to the single ones.
 */
function singleQuotes() {
    return through.obj(function (file, encoding, callback) {
        var data = file.contents.toString(encoding);
        data = toSingleQuotes(data);
        file.contents = new Buffer(data);

        callback(null, file);
    });
}


/**
 * Searches for the localization pattern within the file. All found resource string are serialized into file located at given filePath. Then replaces the occurrences
 * within the file with the resource filter.
 * Note that if some resource strings are used in the view, it is required to specify resource string filter as application dependency.
 * @param {string} filePath specifies location where the resource string file will be stored
 */
function resourceStrings(filePath) {
    var foundResourceStrings = [];
    var originalFile;
    return through.obj(function (file, encoding, callback) {
        if (file.isNull()) {
            cb(null, file);
            return;
        }
        originalFile = file;
        var data = file.contents.toString(encoding),
            pattern = /\{\$\s*([.\w]*)\s*\$\}/g;
        
        // Store all found resource strings for later proccessing
        foundResourceStrings.push(getMatches(data, pattern, 1));

        // And replace them with the call to resolve filter. This filter is responsible for replacing with resource string obtained from the server
        // and can be found in CMS/Filters/Resolve
     
        data = data.replace(pattern, '{{ "$1" | resolve }}');
        file.contents = new Buffer(data);

        callback(null, file);
    }, function (callback) {
        // Writes the found resource strings into the file. Server is then responsible for loading and passing them to the module init
        fs.writeFileSync(path.join(originalFile.base, filePath), new Buffer(JSON.stringify(_.sortedUniq(_.flatten(foundResourceStrings)))));
        callback();
    });
}


/**
 * Gets first group from the given strign matching the given pattern.
 * @param {string} string input string to be matched
 * @param {string} regex regular expression pattern. It has to have first group specified
 * @returns {Array} collection containing all the found matches
 */
function getMatches(string, regex) {
    var matches = [];
    var match;
    while (!!(match = regex.exec(string))) {
        matches.push(match[1]);
    }
    return matches;
}


/**
 * Gets root of the javascript module. Trims scripts root from the start of given input. If the path ends with 'app/', trims
 * it from the end since app folder is not part of the module path.
 * @param {string} appRoot original application root
 * @returns {string} altered appRoot matching the module path
 */
function getModuleRoot(appRoot) {
    var pattern = new RegExp(rootPath + '(.*)(app/)$', 'i'),
        modulePath = appRoot;
    if (modulePath.match(pattern)) {
        return modulePath.replace(pattern, '$1').toLowerCase();
    }

    return modulePath.replace(rootPath, '').toLowerCase();
}


/**
 * Build header for the build.js file. Adds cmsdefine function to support the AMD resolution. Adds dependencies to the modules specified in given @param angularApp
 * and returning function for collecting the data from server. Specifies moduleName variable containign the name of the module. All these variables are then available from the outer scope
 * of angular JS source files. 
 * @param {object} angularApp Angular application object to be built obtained earlier with @see findAngularApps.
 * @returns {string} header for the build.js file.
 */
function getBuildFileHeader(angularApp) {
    var dependencySources = [],
        dependencyNames = [],
        jsDependencies = angularApp.jsDependencies;
    for (var dependency in jsDependencies) {
        if (jsDependencies.hasOwnProperty(dependency)) {
            dependencySources.push('\'' + jsDependencies[dependency] + '\'');
            dependencyNames.push(dependency);
        }
    }

    return 'cmsdefine([' + dependencySources.join() + '], function(' + dependencyNames.join() + ') { \r\n' +
        'var moduleName = \'' + getModuleRoot(angularApp.appRoot) + '\';\r\n' +
        'return function(dataFromServer) { \r\n' +
        'if(angular && resolve && dataFromServer && dataFromServer.resources) { \r\n' +
        'resolve(angular, dataFromServer.resources); \r\n' +
        '} \r\n';
}

/**
 * Builds footer for the build.js file. Returns path to the application root which will be used for the angular module.
 * @param {object} angularApp Angular application object to be built obtained earlier with @see findAngularApps.
 * @returns {string} footer for the build.js file.
 */
function getBuildFileFooter(angularApp) {
    return 'return \'' + getModuleRoot(angularApp.appRoot) + '\';' +
        '}' +
        '})';
}


/**
 * Searches the application root for all app.config.json files specifiying the angular applications.
 */
function findAngularApps() {
    var files = glob.sync(rootPath + '**/app.config.json');
    files.forEach(function (file) {
        var configData = jsonfile.readFileSync(file);
        if (configData && configData.angularApp) {
            var appRoot = slash(path.join(path.dirname(file), configData.appPath || ''));
            angularApps.push({
                appRoot: appRoot,
                jsDependencies: configData.jsDependencies || {}
            });
        }
    });
}