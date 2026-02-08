#!/bin/bash


SCRIPT_DIR=$(dirname "$(readlink -f "${BASH_SOURCE[0]}")")

# Script requires jq

if ! command -v jq &> /dev/null; then
    echo "jq is required for this script"
    exit 1
fi


baseaddress="https://lewiss-dev-server-cjcpcgh4f8a4cpau.newzealandnorth-01.azurewebsites.net/api/v1/"
currentaddress="${baseaddress}/fabric/kineticsroller?width=1200&height=900&colour=Black&fabric=Adagio&opacity=LF"

response=$(curl \
    -H "Content-Type: application/json" \
    $currentaddress
)

echo "$response" | jq .