#!/usr/bin/env pwsh

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

# Get component metadata and set necessary variables
$component = Get-Content -Path "$PSScriptRoot/component.json" | ConvertFrom-Json
$testImage = "$($component.registry)/$($component.name):$($component.version)-$($component.build)-test"

# Set environment variables
$env:IMAGE = $testImage

try {
    # Workaround to remove dangling images
    docker-compose -f "$PSScriptRoot/docker/docker-compose.test.yml" down

    docker-compose -f "$PSScriptRoot/docker/docker-compose.test.yml" up --build --abort-on-container-exit --exit-code-from test

    # Save the result to avoid overwriting it with the "down" command below
    $exitCode = $LastExitCode 
} finally {
    # Workaround to remove dangling images
    docker-compose -f "$PSScriptRoot/docker/docker-compose.test.yml" down
}

# Return the exit code of the "docker-compose.test.yml up" command
exit $exitCode 
