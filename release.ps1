#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
[xml]$xml = Get-Content -Path src/src.csproj
$version = $xml.Project.PropertyGroup[0].Version

if ($component.version -ne $version) {
    throw "Versions in component.json and src.csproj do not match"
}

# Build package
dotnet build src/src.csproj -c Release
dotnet pack src/src.csproj -c Release -o ../dist

$package = (Get-ChildItem -Path "dist/*.$version.nupkg").FullName

# Push to nuget repo
if ($env:NUGET_KEY -ne $null) {
    dotnet nuget push $package -s https://www.nuget.org/api/v2/package -k $env:NUGET_KEY
} else {
    nuget push $package -Source https://www.nuget.org/api/v2/package
}
