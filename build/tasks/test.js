var nunit = require('gulp-nunit-runner');
var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var unzip = require('unzip');
var mkdirp = require('mkdirp');
var config = require('../config');

var nunitLocation= 'https://github.com/nunit/nunit/releases/download/3.4.1/NUnit-3.4.1.zip';
var toolsDir = config.toolsDir;
var outputDir = config.outputDir +'/test';
var nunitDir = toolsDir + '/nunit';
var zipFileName = toolsDir + '/nunit.zip';
var runnerFileName =  nunitDir + '/bin/nunit3-console.exe';

var isWin = /^win/.test(process.platform);

gulp.task('test', ['get-nunit'], function () {

	mkdirp.sync(outputDir);

	var setup = {

		executable: runnerFileName,

		// The options below map directly to the NUnit console runner. See here
		// for more info: http://www.nunit.org/index.php?p=consoleCommandLine&r=2.6.3
		options: {

			// Name of XML result file (Default: TestResult.xml)
			result: 'test-results.xml',

			// Suppress XML result output.
			noresult: false,

			workers: 1,

			// Work directory for output files.
			work: outputDir,

			// Label each test in stdOut.
			//labels: true//,

			//// Set internal trace level.
			trace: 'Info'

			// Framework version to be used for tests.
			//framework: 'net-4.0'
		}

	};


    return gulp
        .src(['**/bin/**/' + config.command.test.dllName], { read: false })
        .pipe(nunit(setup));
});


gulp.task('get-nunit', ['nunit-unzip'], function(done) {
	done();
});

gulp.task('nunit-unzip', ['nunit-download'], function(done) {

	var file = runnerFileName;
	var fileExists = fs.existsSync(file);

	if(fileExists) {
		done();
		return;
	}
	
	fs.createReadStream(zipFileName)
		.pipe(unzip.Extract({ path: nunitDir }))
		.on('close', function () {done();});
});


gulp.task('nunit-download', function(done) {

	mkdirp.sync(toolsDir);

	var file = zipFileName;
	var zipExists = fs.existsSync(nunitDir);

	if(zipExists) {
		done();
		return;
	}

	request(nunitLocation)
		.pipe(fs.createWriteStream(file))
		.on('finish', function () { done(); });

});