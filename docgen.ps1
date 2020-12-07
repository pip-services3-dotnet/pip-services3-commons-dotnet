#!/usr/bin/env pwsh

##Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

# Generate image and container names using the data in the "component.json" file
$component = Get-Content -Path "component.json" | ConvertFrom-Json

$docImage="$($component.registry)/$($component.name):$($component.version)-$($component.build)-docs"
$container=$component.name

# Remove build files
if (Test-Path "./docs") {
    Remove-Item -Recurse -Force -Path "./docs/*"
} else {
    New-Item -ItemType Directory -Force -Path "./docs"
}

# Build docker image
docker build -f docker/Dockerfile.docgen -t $docImage .

# Create and copy compiled files, then destroy the container
docker create --name $container $docImage
docker cp "$($container):/dotnet/app/html/." ./docs
docker rm $container

if (!(Test-Path "./docs")) {
    Write-Host "docs folder doesn't exist in root dir. Build failed. Watch logs above."
    exit 1
}
