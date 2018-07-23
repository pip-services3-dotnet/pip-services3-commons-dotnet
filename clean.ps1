#!/usr/bin/env pwsh

$component = Get-Content -Path "component.json" | ConvertFrom-Json
$buildImage="$($component.registry)/$($component.name):$($component.version)-build"
$testImage="$($component.registry)/$($component.name):$($component.version)-test"

# Clean up build directories
if (Test-Path "obj") {
    Remove-Item -Recurse -Force -Path "obj"
}
if (Test-Path "src/bin") {
    Remove-Item -Recurse -Force -Path "src/bin"
}
if (Test-Path "src/obj") {
    Remove-Item -Recurse -Force -Path "src/obj"
}
if (Test-Path "test/bin") {
    Remove-Item -Recurse -Force -Path "test/bin"
}
if (Test-Path "test/obj") {
    Remove-Item -Recurse -Force -Path "test/obj"
}
if (Test-Path "*.nupkg") {
    Remove-Item -Force -Path "*.nupkg"
}

# Remove docker images
docker rmi $buildImage --force
docker rmi $testImage --force
docker image prune --force

# Remove existed containers
docker ps -a | Select-String -Pattern "Exit" | foreach($_) { docker rm $_.ToString().Split(" ")[0] }
