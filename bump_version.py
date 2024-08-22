# bump_version.py
import semver
import sys

version = sys.argv[1]
release_type = sys.argv[2]

new_version = semver.VersionInfo.parse(version).bump_release(release_type)
print(new_version)