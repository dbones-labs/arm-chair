# ArmChair.Core
=======

**CouchDB + .NET + Unit-Of-Work = ArmChair.**

ArmChair has been built from the ground up with the following goals:

* Unit-Of-Work pattern support - supporting the all or nothing commit.
* POCO's - no base class or interface required. (Just add your own Id and Rev).
* Conventions - where possible we implement small conventions to make the framework work for you.
* Customisable - Add support for your IDs, Tracking, Serialization etc.
* Use of an Indexing Service - Search by IDs, where the initial search can be executed on ElasticSearch / Solr / Lucene

## Other Features

* Mongo Query Support
* Linq Support (Partial)
* (configurable) Logs requests and responses, to assist development

## Documentation

its being worked on but you can find some here: https://bitbucket.org/dboneslabs/arm-chair/wiki/Home

## Nuget

* **Url** https://www.nuget.org/packages/ArmChair.Core/
* **Current version:** 0.12.x
* **Released:** May 2019

```
PM> Install-Package ArmChair.Core
```

## Compatibility

* .NET 4.5.2 + (Mono latest)
* .NET Standard 1.6 + (.NET core 1.1 +)
* CouchDB 2.0 +

## Licence

Apache 2.0 licensed.

## Building from source

To build this project you will need dotnet core

```
dotnet restore
dotnet msbuild /target:Build /p:Configuration=Release /p:BuildNumber=37
dotnet test
```

the buildNumber is optional, if not included it will set the patch number to 0.

#### Note

*Please ensure that you test the usage of this library, before using this in your production system.*