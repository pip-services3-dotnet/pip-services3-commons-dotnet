#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

# Get component metadata and set necessary variables
$component = Get-Content -Path "$PSScriptRoot/component.json" | ConvertFrom-Json
$buildImage = "$($component.registry)/$($component.name):$($component.version)-$($component.build)-build"
$container=$component.name

# Remove build files
if (Test-Path -Path "$PSScriptRoot/obj") {
    Remove-Item -Recurse -Force -Path "$PSScriptRoot/obj"
}

# Build docker image
docker build -f "$PSScriptRoot/docker/Dockerfile.build" -t $buildImage .

# Create and copy compiled files, then destroy
docker create --name $container $buildImage
docker cp "$($container):/obj" "$PSScriptRoot/obj"
docker cp "$($container):/dist" "$PSScriptRoot/dist"
docker rm $container

# Verify that obj folder was indeed created after build
if (-not (Test-Path "$PSScriptRoot/obj")) {
    Write-Error "obj folder doesn't exist in root dir. Build failed. See logs above for more information."
}
