#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
$version = (Get-Content -Path src/src.csproj | ConvertFrom-Xml).Project.PropertyGroup[0].Version

if ($component.version -ne $version) {
    throw "Versions in component.json and src.csproj do not match"
}

$package = $component.$version.nupkg

# # Automatically login to server
# if ($env:NPM_USER -ne $null -and $env:NPM_PASS -ne $null) {
#     npm-cli-login
# }

# Build package
nuget pack src/src.csproj

# Push to nuget repo
nuget push $package -Source https://www.nuget.org/api/v2/package