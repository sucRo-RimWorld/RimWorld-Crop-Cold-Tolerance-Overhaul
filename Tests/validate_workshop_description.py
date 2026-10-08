#!/usr/bin/env python3
"""Validate the single paste-ready English+Japanese CCTO Workshop description."""
from pathlib import Path
import re
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
title = "Crop Cold Tolerance Overhaul（作物耐寒性オーバーホール）"
body = (root / "Docs/SteamWorkshopDescription.txt").read_text(encoding="utf-8")
ja = (root / "Docs/SteamWorkshopDescription-ja.txt").read_text(encoding="utf-8").strip()
name = ET.parse(root / "About/About.xml").getroot().findtext("name")


def ensure(ok, message):
    if not ok:
        raise SystemExit("FAIL: " + message)


ensure(name == title, "About and Workshop display names differ")
ensure(body.startswith("[h1]" + title + "[/h1]"), "bilingual Workshop title missing")
ensure(body.count(title) == 1, "bilingual title appears more than once")
boundary = "\n\n[hr][/hr]\n\n[h2]日本語 / Japanese[/h2]"
ensure(body.count("[hr][/hr]") == 1 and body.count(boundary) == 1,
       "missing or repeated horizontal rule between English and Japanese")
english, japanese = body.split("\n\n[hr][/hr]\n\n")
ensure(japanese.strip() == ja, "Japanese section and Japanese source differ")
ensure("[h2]Key features[/h2]" in english and "[h2]主な特徴[/h2]" in japanese,
       "features missing in one language")
ensure("[h2]Save compatibility[/h2]" in english and "[h2]セーブ互換性[/h2]" in japanese,
       "save conditions missing in one language")
for required in ("Harmony", "Medieval Overhaul", "Vanilla Plants Expanded", "cold dormancy"):
    ensure(required in body, "required scope/compatibility information missing: " + required)
for tag in ("h1", "h2", "h3", "b", "list", "url", "img"):
    opened = re.findall(r"\[" + tag + r"(?:=[^\]]*)?\]", body)
    closed = re.findall(r"\[/" + tag + r"\]", body)
    ensure(len(opened) == len(closed), "unbalanced BBCode: " + tag)
images = re.findall(r"\[img\](.*?)\[/img\]", body)
ensure(len(images) == len(set(images)), "repeated Workshop image URL")
ensure(len(body.encode("utf-8")) <= 8000 and
       len(body.replace("\n", "\r\n").encode("utf-8")) <= 8000,
       "combined Workshop description exceeds 8000 UTF-8 bytes")
ensure(not any(term in body for term in
               ("MIT License", "AI assistance", "AIの支援", "Ko-fi", "img.shields.io")),
       "license, AI or support disclosure remains in Workshop")
print("PASS: CCTO bilingual Steam title, one [hr][/hr], both languages, size and BBCode")
