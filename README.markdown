#ArmChair.Core
=======

CouchDB + .NET + Unit-Of-Work = ArmChair.

ArmChair has been built from the ground up with the following goals:

* Unit-Of-Work pattern support - supporting the all or nothing commit.
* POCO's - no base class or interface required. (Just add your own Id and Rev).
* Conventions - where possible we implement small conventions to make the framework work for you.
* Customisable - Add support for your IDs, Tracking, Serialization etc.
* Use of an Indexing Service - Search by IDs, where the initial search can be executed on ElasticSearch / Solr / Lucene

##Documentation

its being worked on but you can find some here: http://docs.dbones.co.uk/ArmChair.Default.aspx

##Compatibility

* .NET 4.0 +
* CouchDB 1.2 +

Planned Mono (on Linux) Support



##Licence
Apache 2.0 licensed.

###Note
Please ensure that you test the usage of this library, before using this in your production system.