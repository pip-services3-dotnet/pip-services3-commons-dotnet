#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

$component = Get-Content -Path "component.json" | ConvertFrom-Json
$version = $component.version
$spec = "$component.nuspec"
$package = $component.$version.nupkg

# # Automatically login to server
# if ($env:NPM_USER -ne $null -and $env:NPM_PASS -ne $null) {
#     npm-cli-login
# }

# Build package
nuget pack $spec

# Push to nuget repo
nuget push $package -Source https://www.nuget.org/api/v2/package