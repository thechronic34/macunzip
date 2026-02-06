#!/usr/bin/env python3
"""Extract archives into a sibling folder using 7-Zip."""

from __future__ import annotations

import argparse
import os
import shutil
import subprocess
import sys
from pathlib import Path

MULTI_EXTENSIONS = [
    ".tar.gz",
    ".tar.bz2",
    ".tar.xz",
    ".tar.zst",
    ".tar.lz",
    ".tar.lzma",
    ".tar.br",
    ".tgz",
    ".tbz2",
    ".txz",
]


def find_7z() -> str | None:
    """Return the 7z executable path if available."""
    for candidate in ("7z", "7zz"):
        path = shutil.which(candidate)
        if path:
            return path
    return None


def strip_extension(filename: str) -> str:
    lower = filename.lower()
    for ext in MULTI_EXTENSIONS:
        if lower.endswith(ext):
            return filename[: -len(ext)]
    return os.path.splitext(filename)[0]


def unique_output_dir(base_dir: Path) -> Path:
    if not base_dir.exists():
        return base_dir
    index = 1
    while True:
        candidate = Path(f"{base_dir} ({index})")
        if not candidate.exists():
            return candidate
        index += 1


def extract_archive(archive_path: Path, seven_zip: str) -> None:
    if not archive_path.exists():
        raise FileNotFoundError(f"Archive not found: {archive_path}")
    if not archive_path.is_file():
        raise ValueError(f"Not a file: {archive_path}")

    output_base = strip_extension(archive_path.name)
    output_dir = unique_output_dir(archive_path.with_name(output_base))

    output_dir.mkdir(parents=True, exist_ok=True)

    command = [
        seven_zip,
        "x",
        str(archive_path),
        f"-o{output_dir}",
        "-y",
    ]

    result = subprocess.run(command, capture_output=True, text=True)
    if result.returncode != 0:
        raise RuntimeError(
            "Extraction failed:\n"
            f"Command: {' '.join(command)}\n"
            f"stdout: {result.stdout}\n"
            f"stderr: {result.stderr}"
        )


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Extract archives into a sibling folder using 7-Zip.",
    )
    parser.add_argument(
        "archives",
        nargs="+",
        type=Path,
        help="One or more archive files to extract.",
    )
    return parser.parse_args()


def main() -> int:
    args = parse_args()
    seven_zip = find_7z()
    if not seven_zip:
        print(
            "7-Zip command not found. Install p7zip (brew install p7zip) and try again.",
            file=sys.stderr,
        )
        return 1

    failures: list[str] = []
    for archive in args.archives:
        try:
            extract_archive(archive, seven_zip)
            print(f"Extracted: {archive}")
        except Exception as exc:  # noqa: BLE001 - user-facing tool
            failures.append(f"{archive}: {exc}")

    if failures:
        print("\n".join(failures), file=sys.stderr)
        return 1
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
