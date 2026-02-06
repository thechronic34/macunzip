# MacUnzip

MacUnzip is a small helper that extracts archives (zip/rar/7z/tar, etc.) into a folder
next to the archive, similar to macOS “Archive Utility.” It uses the open‑source 7‑Zip
command line tool (`7z`) for the extraction backend.

## Requirements

- macOS (or any system with Python 3 and 7‑Zip installed)
- 7‑Zip CLI (`7z`)

Install 7‑Zip via Homebrew:

```bash
brew install p7zip
```

## Usage

```bash
python3 macunzip.py /path/to/archive.zip
```

The script creates a folder next to the archive (same name without the extension) and
extracts the contents into it. If the folder already exists, it will create a unique
name like `Archive (1)`.

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
