# nyx-scriptorium

## Development container

The repository includes a VS Code Dev Container that provides .NET 10, FFmpeg, Python,
and `faster-whisper` without installing them on the host.

### Requirements

- Docker Desktop or Docker Engine
- VS Code with the Dev Containers extension

### Open the environment

1. Clone the repository.
2. Open it in VS Code.
3. Run **Dev Containers: Reopen in Container**.

The container restores the NuGet packages automatically after creation.

### Persistent caches

The container uses two named Docker volumes:

- `nyx-scriptorium-nuget` for NuGet packages
- `nyx-scriptorium-huggingface` for faster-whisper and Hugging Face model downloads

The repository files remain in the workspace bind mount. No database or application
state is stored in a Docker volume.

Temporary files use `/tmp` inside the container.

### Validation

Run these commands inside the container:

```bash
dotnet --info
ffmpeg -version
python -c "import faster_whisper"
dotnet test NyxScriptorium.slnx -c Release --filter "TestCategory!=Integration"
```