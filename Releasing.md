
Releasing
================

Versioning is handled by MinVer. The version is derived from git tags (e.g. `v2.0.151`).
No version numbers need to be edited in `.csproj` files.


Steps
-----

1. Ensure all commits are on `dev/v2.0.0` and the build is green.
2. Create and push a git tag:
   ```bash
   git tag v2.0.NNN
   git push origin v2.0.NNN
   git push origin HEAD:dev/v2.0.0
   ```
3. On GitHub, create a Release from that tag (Add release notes, publish).
4. The `publish.yml` workflow triggers automatically and:
   - Builds and packs all projects (with the CI signing key)
   - Validates the `.nupkg` files
   - Runs tests on Linux (net8.0) and Windows (net48)
   - Pushes all packages to NuGet.org


Preview releases
----------------

Tag with a pre-release suffix to publish a preview to NuGet:
```bash
git tag v2.0.NNN-preview1
```
Same steps apply.


Notes
-----

- The real strong-name key is not in source control. CI uses `SrkToolkit.CI-key.snk`.
