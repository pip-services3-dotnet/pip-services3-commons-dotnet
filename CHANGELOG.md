# <img src="https://uploads-ssl.webflow.com/5ea5d3315186cf5ec60c3ee4/5edf1c94ce4c859f2b188094_logo.svg" alt="Pip.Services Logo" width="200"> <br/> Portable abstractions and patterns for .NET Changelog

## <a name="3.5.0"></a> 3.5.0 (2023-12-15)

### Breaking Changes
* Migrate to .NET 8.0

## <a name="3.4.1"></a> 3.4.1 (2023-02-02)

### Features
* Added Too Many Requests exception

## <a name="3.4.0"></a> 3.4.0 (2022-08-04)

### Breaking Changes
* Migrate to .NET 6.0

## <a name="3.3.1"></a> 3.3.1 (2022-02-21)

### Bug Fixes

* Fixed AnyValueMap.FromTuplesArray reqursion bug
* Fixed CommandSet.Events property visibility

## <a name="3.3.0"></a> 3.3.0 (2021-08-31)

### Breaking Changes
* Migrate to .NET 5.0

## <a name="3.2.0"></a> 3.2.0 (2021-07-23)

### Features
* To **data** added **IChangeable**, **MultiString**, **TokenizedDataPage**, **TokenizedPagingParams**
* **ArrayConverter** added **ListToArray**
* **AnyValue** added **GetTypeCode**
* **AnyValueArray** added **GetAsType**, **GetAsTypeWithDefault**
* **AnyValueMap** added **FromTuplesArray**
* **StringValueMap** added **FromTuplesArray**
* **TypeReflector** added **IsPrimitive**
* **Scheme** added **ValidateAndReturnException**
* **ValidationException** added **FromResults**


## <a name="3.1.3"></a> 3.1.3 (2020-11-24)

### Features
* Fixed dynamic data convert for double

## <a name="3.1.2"></a> 3.1.2 (2020-11-24)

### Features
* added support for arrays in the MapConverter.ToMap method

## <a name="3.1.1"></a> 3.1.1 (2020-06-26)

### Features
* Implemented support backward compatibility

## <a name="3.1.0"></a> 3.1.0 (2020-05-26)

### Breaking Changes
* Migrated to .NET Core 3.1

## <a name="3.0.17"></a> 3.0.17 (2020-02-17)

### Features
* Added special provisions for dynamic data

## <a name="3.0.16"></a> 3.0.16 (2020-01-22)

### Bug Fixes
* Fixed schema enum validation

## <a name="3.0.15"></a> 3.0.15 (2019-11-01)

### Features
* **data** Added ability parse list of objects to the tag processor

## <a name="3.0.14"></a> 3.0.14 (2019-10-29)

### Features
* **data** Added tag processor

## <a name="3.0.0-3.0.13"></a> 3.0.0-3.0.13 (2019-10-02)

### Bug Fixes
* Extended error description with details about inner exceptions
* Added supporting of fetching null results from OptionsResolver in case if options are not specified

### Features
* **commands** Extended ICommand interface with Schema

### Bug Fixes
* Added supporting of dictionary type transfer in the Object Mapper

### Breaking Changes
* Moved components to pip-services3-components

## <a name="2.4.0"></a> 2.4.0 (2018-08-15)

### Features
* **lock** Added ILock, Lock and MemoryLock

## <a name="2.3.0-2.3.18"></a> 2.3.0-2.3.18 (2018-07-11)

### Bug Fixes
* Fixed ObjectMapper for collections

### Features
* **random** Added RandomDataGenerator
* **logic** Added AbstractController with default instrumentation, cache and audit methods
* **data** Added ProjectionParams
* **info** Added ContextInfo and InfoFactory
* **count** Added reset_timeout to CachedCounters

## <a name="2.2.0"></a> 2.2.0 (2017-10-24)

### Features
* **config** Migrated to Handlebars template engine
* Converted to new project structure
* Dockerized build and test
* **log** Added CachedLogger

## <a name="2.1.0"></a> 2.1.0 (2017-10-22)

### Features
* **config** Added support for mustache templates

## <a name="2.0.0-2.0.5"></a> 2.0.0-2.0.5 (2017-06-12)

### Features
* **commands** Replace IExecutable by ExecutableDelegate in Command constructor to simplify further usage
* **validate** Added FilterParamsSchema and PagingParamsSchema classes
* **data** Added FilterParams.FromValue method

### Bug Fixes
* Updated TypeMatcher to correctly validate TypeCode 
* Implemented Object.Equals int FilterParams and PagingParams to fix unit-tests

## <a name="2.0.0"></a> 2.0.0 (2017-02-24)

Cleaned up and simplified dependency management and object factories.

### Features
* **refer** Added DependencyResolver
* **build** Added Factory

### Breaking Changes
* Refactored **refer** package. Removed IDescriptable and ILocateable interface. Made locator a mandatory requirement to place component into references.
* Moved **ManagedReferences** to **pip-services3-container**

## <a name="1.0.3-1.0.40"></a> 1.0.3-1.0.45 (2017-01-07)

### Features
* **auth** MemoryCredentialStore
* **config** IConfigReader interface and readers for AppSettings and ConnectionStrings
* **config** IConfigReader, CachedConfigReader
* **config** NameResolver. Improved support for named (non-singleton) components 
* **connect** MemoryDiscovery
* **log** DiagnoticsLogger
* **refer** Made get methods in IReferences generic
* **refer** Added PutAll method to IReferences
* **run** Changed FixedRateTimer interface
* **refer** Added Kind field to Descriptor
* **refer** ReferencesDecorators and ManagedReferences.
* **data** Added IVersioned interface
* **log** EventLogger
* **config** OptionsResolver

### Bug Fixes
* Added description to NuGet package
* Added development documentation
* Made convenience changes in StringValueMap and AnyValueMap
* Fixed NullPointerException in JsonConverter.ToNullableMap
* Fixed NullPointerException in AnyValueMap and StringValueMap
* Made key methods virtual
* Fixed NullPointerException in NameResolver
* Fixed wrong cast in Referencer
* Fixed GetAll in references
* Fixed NullPointerException in MemoryDiscovery
* Set default log levels
* Fixed endless loop in Loggers while logging errors
* Fixed printing wrong level in loggers
* Renamed ReferenceSet to References
* Added auto-creation to GetOptional methods in References
* Added CompositeCounters constructor with references 
* Renamed SetAsMap method to Append
* Added missing descriptor to EventLogger
* Fixed timeout for CachedConfigReader
* IOpenable interface now inherits IClosable and had IsOpened() method

## Breaking changes
* Removed IParamNotifiable and IParamExecutable interfaces. Now INotifiable and IExecutable support arguments

## <a name="1.0.0"></a> 1.0.0 (2016-11-21)

Initial public release

### Features
* **auth** Credentials for client authentication
* **build** Component factories
* **commands** Command and Eventing patterns
* **config** Configuration framework
* **connect** Connection parameters
* **convert** Portable soft data converters
* **count** Performance counters
* **data** Data value objects
* **errors** Portable application errors
* **log** Logging components
* **random** Random data generators
* **refer** Component referencing framework
* **reflect** Portable reflection helpers
* **run** Execution framework
* **validate** Data validators

### Bug Fixes
No fixes in this version

