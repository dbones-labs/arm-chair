var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var mkdirp = require('mkdirp');
var path = require('path');
var config = require('../config');
var xmlpoke = require('xmlpoke');


var glob = require("glob");
var Promise = require('es6-promise').Promise;
var xml2js = require('xml2js');
var eyes = require('eyes');

var downloadLocation= 'http://nuget.org/nuget.exe';
var toolsDir = config.toolsDir;
var nugetDir = toolsDir + '/nuget';
var runnerFileName =  nugetDir + '/nuget.exe';

var nuspecNamespace = 'http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd';

gulp.task('nuegt-set-dependency-version', function(done){
    //xmlpoke

    if(config.buildVersion == null
        || config.command.package == null
        || config.command.package.dependencyNameOverride == null
        || config.command.package.dependencyNameOverride == '') {
        done();
        return;
    }

    xmlpoke('./**/*.nuspec', function(xml) {
        xml
            .addNamespace('x', nuspecNamespace)
            .set(config.command.package.dependencyNameOverride, config.buildVersion);
    });

    done();

});

gulp.task('nuspec-sync-dependency-version', function(done){

    //ugly code
    //find nuspec and package files
    var packageFile = function(projectName){
        projectName = path.basename(projectName, '.nuspec');
        return  new Promise(function(resolve, reject){
            var toFind = './**/' + projectName + '/packages.config';

            glob(toFind, function(er, files){
                if(files.length === 0){
                    resolve();
                } else {
                    resolve(files[0]);
                }
            });

        });
    };

    var projectFile = function(projectName){
        projectName = path.basename(projectName, '.nuspec');
        return new Promise(function(resolve, reject){
            var toFind = './**/'+ projectName +'.*proj';
            console.log(toFind);
            glob(toFind, function(er, files){
                if(files.length === 0){
                    resolve();
                } else {
                    resolve(files[0]);
                }
            });

        });
    };

    var specFiles = function(){
        console.log("finding specFiles");
        return new Promise(function(resolve, reject) {
            glob('./**/*.nuspec', function (er, files) {
                resolve(files);
            });
        });
    };

    var getAll= function(files, delegte){
        var promises = [];

        files.forEach(function(file){
            promises.push(delegte(file));
        });

        return Promise.all(promises);
    };

    var packageDependencies = function(ctx){
        console.log("getting package dependecies for " + ctx.package);
        return new Promise(function(resolve, reject) {

            if(ctx.package == null){
                resolve(ctx);
                return;
            }

            var parser = new xml2js.Parser();

            var handle = function(cfg) {
                cfg.packages.package.forEach(function(p){

                    eyes.inspect(p);


                    var target = p.$.targetFramework;
                    var key;

                    if (target == null || target === '') {
                        key = 'default'
                    }
                    else {
                        
                        //if(target.length === 4) {
                        //    target = target + "0";
                        //}
                        
                        key = target;
                    }
                    if (ctx.dependencies[key] == null) {
                        ctx.dependencies[key] = [];
                    }

                    var dependency = {
                        id: p.$.id,
                        version: p.$.version
                    };

                    ctx.dependencies[key].push(dependency);

                });
                resolve(ctx);
            };

            fs.readFile(ctx.package, function(err, data) {
                parser.parseString(data, function(err, d) { handle(d); });
            });

        });
    };

    var projectDependencies = function(ctx){
        return new Promise(function(resolve, reject) {

            if(ctx.project == null){
                console.log('should this be a package? ' + ctx.nuspec + ' (nuspec name has no matching project)');
                resolve(ctx);
                return;
            }

            //console.log(ctx.project);
            var parser = new xml2js.Parser();

            var handle = function(proj) {
                //eyes.inspect(proj);

                if(proj.Project.ItemGroup == null){
                    resolve(ctx);
                    return;
                }

                proj.Project.ItemGroup.forEach(function(grp){
                    if(grp.ProjectReference == null) return;
                    grp.ProjectReference.forEach(function(ref){

                        //console.log(ref.Name[0]);

                        var key = 'default';

                        if (ctx.dependencies[key] == null) {
                            ctx.dependencies[key] = [];
                        }

                        var dependency = {
                            id: ref.Name[0],
                            version: config.buildVersion
                        };

                        ctx.dependencies[key].push(dependency);

                    });
                });

                resolve(ctx);
            };

            fs.readFile(ctx.project, function(err, data) {
                parser.parseString(data, function(err, d) { handle(d); });
            });

        });
    };

    var specs = specFiles();
    var packages = specs.then(function(files) {return getAll(files, packageFile)}).then((function(f){console.log('done');return f;}));
    var projects = specs.then(function(files) {return getAll(files, projectFile)}).then((function(f){console.log('done');return f;}));


    Promise.all([specs, packages, projects]).then(function(results){
        console.log('listed files');
        var items=[];
        for(var i = 0; i < results[0].length; i++){
            items.push({
                nuspec : results[0][i],
                package : results[1][i],
                project : results[2][i],
                dependencies: {}
            });
        }
        eyes.inspect(items);
        return items;
    }).then(function(results){
        console.log('created contexts');
        //clear dependencies
        results.forEach(function(result){
            xmlpoke(result.nuspec, function(xml) {
                xml
                    .addNamespace('x', nuspecNamespace)
                    .clear('//x:dependencies');
            });
        });
        return results;
    }).then(function(results){

        //get dependecies
        console.log('cleared dependecies');
        var getPackageDependencies = getAll(results, packageDependencies);
        var getProjectDependencies = getAll(results, projectDependencies);

        return Promise.all([getPackageDependencies, getProjectDependencies]).then(function(){return results;}).catch(function(e) {console.log(e)});

    }).then(function(results) {
        console.log('extracted new dependecies');
        //set dependencies
        results.forEach(function(ctx){

            xmlpoke(ctx.nuspec, function(xml) {
                xml.addNamespace('x', nuspecNamespace);
                for(var propertyName in ctx.dependencies) {
                    var group = propertyName === 'default' ? 'x:group[not(@*)]' : 'x:group[@x:targetFramework=\''+ propertyName +'\']';
                    xml.ensure('x:package/x:metadata/x:dependencies/' + group);

                    ctx.dependencies[propertyName].forEach(function(dependency){
                        xml.add('//x:dependencies/' + group + '/dependency', {
                            '@id': dependency.id,
                            '@version': dependency.version
                        });
                    });
                }
            });
        });

        done();
    });
});

gulp.task('get-nuget',  function(done) {

    mkdirp.sync(nugetDir);
    var nugetExists = fs.existsSync(runnerFileName);

    if(nugetExists){
        done();
        return;
    }

    console.log('getting nuget');

    request(downloadLocation)
        .pipe(fs.createWriteStream(runnerFileName))
        .on('finish', function () { done(); });
});