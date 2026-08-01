#!/usr/bin/env bash

set -euo pipefail

project_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
library_dir="$project_root/Library"

if [[ ! -f "$project_root/ProjectSettings/ProjectVersion.txt" || ! -f "$project_root/Packages/manifest.json" ]]; then
  echo "error: $project_root does not look like a Unity project" >&2
  exit 1
fi

if [[ ! -d "$library_dir" ]]; then
  echo "Unity Library cache is already absent; nothing to remove."
  exit 0
fi

echo "Removing generated Unity cache at: $library_dir"
rm -rf -- "$library_dir"
echo "Done. Reopen the project in Unity so Package Manager can restore packages and reimport assets."
