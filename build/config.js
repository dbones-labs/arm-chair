var args = require('yargs').argv;
var path = require('path');

var config = {

    outputDir : path.resolve('./output'),
    toolsDir : path.resolve('./tools'),
    buildVersion : args.buildNumber ? '0.6.' + args.buildNumber  : '0.0.0',
    company: 'dbones.co.uk'


};

//command config
config.command = {
    assembly: {
        copyright: 'Copyright '+ config.company
    },
    test:{
        dllName: '*.Tests.dll'
    },
    package: {
        //dependencyNameOverride: '//x:dependency[starts-with(@x:id, \'Boxes.\')]/@x:version'
        //configFile: null
        version: config.buildVersion
    }
}

module.exports = config;