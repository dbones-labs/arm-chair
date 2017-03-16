#ArmChair.Core
=======

**CouchDB + .NET + Unit-Of-Work = ArmChair.**

ArmChair has been built from the ground up with the following goals:

* Unit-Of-Work pattern support - supporting the all or nothing commit.
* POCO's - no base class or interface required. (Just add your own Id and Rev).
* Conventions - where possible we implement small conventions to make the framework work for you.
* Customisable - Add support for your IDs, Tracking, Serialization etc.
* Use of an Indexing Service - Search by IDs, where the initial search can be executed on ElasticSearch / Solr / Lucene

##Other Features

* Mongo Query Support
* Linq Support (Partial)

##Documentation

its being worked on but you can find some here: http://docs.dbones.co.uk/ArmChair.Default.aspx

##Nuget

* **Current version:** 0.5.x
* **Released:** March 2017

```
PM> Install-Package ArmChair.Core
```

* **in development version:** 0.6.x

##Compatibility

* .NET 4.5 + (Mono latest)
* CouchDB 2.0 +

###Backwards Compatibility

* .NET 4.0 - last supported in release 0.5.x
* CouchDB 1.2 - Except the Mongo Query features

##Licence

Apache 2.0 licensed.

##Building from source

to build the project you will need to install *node* and *gulp*

```
npm install
gulp build-all [--buildNumber 123]
```

the buildNumber is optional, if not included it will set the patch number to 0.

####Note
*Please ensure that you test the usage of this library, before using this in your production system.*