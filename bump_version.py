# bump_version.py
import semver
import sys

version = sys.argv[1]
release_type = sys.argv[2]

version_info = semver.VersionInfo.parse(version)

if release_type == 'major':
    new_version = version_info.bump_major()
elif release_type == 'minor':
    new_version = version_info.bump_minor()
elif release_type == 'patch':
    new_version = version_info.bump_patch()
else:
    raise ValueError(f"Unknown release type: {release_type}")

print(new_version)