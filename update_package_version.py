# update_package_version.py
import json
import sys

version = sys.argv[1]

with open('chatbot.client/package.json', 'r') as f:
    data = json.load(f)

data['version'] = version

with open('chatbot.client/package.json', 'w') as f:
    json.dump(data, f, indent=2)

print(f"Updated package.json to version {version}")