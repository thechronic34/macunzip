# MacUnzip

MacUnzip is a small helper that extracts archives (zip/rar/7z/tar, etc.) into a folder
next to the archive, similar to macOS “Archive Utility.” It uses the open‑source 7‑Zip
command line tool (`7z`) for the extraction backend.

## Requirements

- Python 3
- 7‑Zip CLI (`7z`)

### macOS

Install 7‑Zip via Homebrew:

```bash
brew install p7zip
```

### Windows

Install 7‑Zip from https://www.7-zip.org/. The script looks for `7z.exe` in standard
install locations (`C:\\Program Files\\7-Zip` and `C:\\Program Files (x86)\\7-Zip`) or
on your `PATH`.

## Usage

```bash
python3 macunzip.py /path/to/archive.zip
```

The script creates a folder next to the archive (same name without the extension) and
extracts the contents into it. If the folder already exists, it will create a unique
name like `Archive (1)`.

## Windows x86/x64 executable (optional)

You can build a standalone EXE for 32‑bit or 64‑bit Windows using PyInstaller. Build on
the same architecture you want to target:

```bash
pyinstaller --onefile --name macunzip macunzip.py
```

The output will be in `dist/macunzip.exe`.

## Double‑click / “Open” behavior on macOS

To make it behave like a double‑click extractor:

1. Open **Automator**.
2. Create a new **Application**.
3. Add **Run Shell Script**.
4. Set **Shell** to `/bin/zsh` and **Pass input** to **as arguments**.
5. Paste:

```bash
/usr/bin/env python3 /path/to/macunzip.py "$@"
```

6. Save the application as **MacUnzip.app**.
7. In Finder, **Open With → Other…** and select the app, then click **Always Open With**.

Now double‑clicking a supported archive will extract it into a folder next to the file.

## Notes

- The script supports multiple files at once.
- It works with most formats supported by 7‑Zip (zip, rar, 7z, tar, tar.gz, tar.xz, etc.).

## TaskbarGroup.App manual UI test guide

For the Windows taskbar-group launcher prototype, see:

- `TaskbarGroup.App/TESTING_TR.md`
