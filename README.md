# Tagolog

Tagolog is a free .NET library for structured logging like a breeze.
Tagolog libraries are
- **Tagolog** [![NuGet Version](http://img.shields.io/nuget/v/tagolog.svg?style=flat)](https://www.nuget.org/packages/tagolog/) - general tags and tag context support.
- **Tagolog.NLog** [![NuGet Version](http://img.shields.io/nuget/v/tagolog.svg?style=flat)](https://www.nuget.org/packages/tagolog.nlog/) - adapter for .NET logging library [NLog](http://nlog-project.org/)

## Getting started

- Install [Tagolog](https://www.nuget.org/packages/tagolog) and [Tagolog.NLog](https://www.nuget.org/packages/tagolog.nlog) NuGet packages.
- Surround your code fragments with tag scopes, fill scopes with structured tags (meta information).
```csharp
using ( var tagScope = TagScopeManager.CreateScope() )
{
    scope.Tags[ "UserId" ] = "jdoe";
    scope.Tags[ "VirtualMachine" ] = "appserver4.jdoe.tld";

    // ...
    log.Info( "Stopping virtual machine..." );
    // ...
}
```
- Modify your application config file append "tagolog" section.
[app.config](Src/Examples/CloudHosting/NLog/NLogConsoleExample/App.config) example
- Modify your NLog configuration.
[NLog.config](Src/Examples/CloudHosting/NLog/NLogConsoleExample/NLog.config) example
- Use *tagologmdcitem* or *tagologmdc* layout renderers to access your tag values
```xml
<target fileName="${basedir}/${tagologmdcitem:item=Application}.log.txt"
    name="HumanReadableFile" xsi:type="File" 
    layout="${date:format=HH\:mm\:ss} ${level} ${message}${newline}${tagologmdc:tagKey=#tagKey#:tagValue=#tagValue#:orderBy=true:builtInTags=false:format=  [#tagKey#\:#tagValue#]\\n}" />
```
- Use [TagologJsonLayout](Src/NLog/Tagolog.NLog/TagologJsonLayout.cs) to get your logs with tags in json.
```xml
<target name="JsonFile" xsi:type="File" fileName="${basedir}/log.json.txt" >
    <layout xsi:type="TagologJsonLayout" SuppressSpaces="True" BuiltInTagsEnabled="True">
        <attribute name="TimestampLocal" layout="${longdate}" />
        ....
```
- Grab json logs with your business tags to your favourite centralized log storage like Elasticsearch or Graylog

## License

Tagolog is open source software, licensed under the terms of BSD license.
Tagolog.NLog is open source software, licensed under the terms of BSD license.
See [license.txt](license.txt) for details.

## How to build

Use Visual Studio 2017. Solution files are:
- [Tagolog.Logging.sln](Src/Tagolog.Logging.sln) - Tagolog and Tagolog.NLog projects.
