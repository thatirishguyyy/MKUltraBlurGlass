# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Eine persönliche Wartungs-Fork von DWMBlurGlass 2.3.1 für Effekte auf Windows-Titelleisten und Fensterrahmen. Gepflegt von **thatirishguyyy**.

**Das Original stammt von Maplespe.** Vielen Dank an Maplespe, ALTaleX531 und alle Mitwirkenden und Übersetzer. Siehe [Danksagungen](CREDITS.md) und [Originalprojekt](https://github.com/Maplespe/DWMBlurGlass).

## Version und Download

**0.1.1** behebt die Erkennung des dunklen Fenstermodus auf aktuellen Windows-11-Builds. Helle und dunkle Fenster verwenden unabhängig voneinander ihre jeweiligen Farben für aktive und inaktive Fenster. Die Fork enthält außerdem Geometrie- und Farbkontextkorrekturen, eine blaue Oberfläche, einen abschaltbaren Menüklick unter About und einen englischen Deinstaller.

Ein Download genügt: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) enthält Anwendung und `MKUltraBlurGlassUninstall.exe`. Das Quellcodearchiv ist für Entwickler optional. Alle Downloads: [Releases](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Installation und Aktualisierung

1. Beim Wechsel zuerst den Host der alten Kopie deinstallieren und deren Oberfläche schließen. Original und Fork dürfen nicht gleichzeitig aktiv sein.
2. Das vollständige ZIP außerhalb des Benutzerprofilordners entpacken, etwa nach `C:\ProgramData\MKUltraBlurGlass`. Programm, DLLs und `data` zusammenlassen.
3. `MKUltraBlurGlass.exe` als Administrator starten. Unter **Symbol** passende Windows-Symbole herunterladen, dann unter **General** auf **Install** klicken. „Installed“ allein bestätigt weder gültige Symbole noch sichtbare Unschärfe. Windows-Updates können neue Symbole erfordern.
4. **Light Mode colors** und **Dark Mode colors** gelten für den Modus des jeweiligen Fensters. Mit **Save** speichern.

## Deinstallation

`MKUltraBlurGlassUninstall.exe` öffnen, den erkannten Pfad prüfen, Administratorrechte anfordern und **Uninstall** wählen. Standardmäßig bleiben Dateien, Einstellungen und Symbole erhalten. Die optionale Bereinigung entfernt nur die vier Programmbinärdateien der gewählten Kopie; der Datenordner bleibt bestehen. Auch die ursprüngliche DWMBlurGlass-Aufgabe wird erkannt.

## Tests und bekannte Einschränkungen

Getestet auf Windows 11 25H2 **26200.9974**. Installation, Deinstallation, erneute Installation und Start nach Neustart wurden für 0.1.0 geprüft. Für 0.1.1 wurden Update und unabhängige helle/dunkle Fensterfarben bestätigt; der Neustarttest wurde für diese Korrektur nicht wiederholt. Andere Builds und Effektmethoden sind nicht vollständig geprüft. Startmenü und Taskleiste gehören nicht zum Wirkungsbereich.

Der Farbwähler übernimmt eine Auswahl möglicherweise erst nach Bewegung des Auswahlpunkts auf der Palette. Die genaue Reproduktion steht noch aus: [bekannte Probleme](KNOWN-ISSUES.md).

## Quellcode, Dokumentation und Lizenz

Gepflegter Branch: `mkultra/main`. Upstream-Änderungen vor dem Zusammenführen prüfen und testen; nicht direkt synchronisieren. Historische Projektdateinamen bleiben erhalten; die Ausgaben heißen `MKUltraBlurGlass.exe` und `MKUltraBlurGlassExt.dll`.

Weiterführende Dokumentation auf Englisch: [Build](docs/BUILDING.md), [Wartung](docs/MAINTAINING.md), [Fehlerbehebung](docs/TROUBLESHOOTING.md), [Deinstaller](tools/uninstaller/README.md) und [Versionshinweise](RELEASE-NOTES.md). Für Übersetzungen XML-Werte in `Languagefiles/` bearbeiten; Schlüssel, Platzhalter und Autorenangaben erhalten.

GNU LGPL Version 3 oder neuer: [COPYING.LESSER](COPYING.LESSER) und [LICENSE.txt](LICENSE.txt). Ursprüngliche Urheberrechts- und Drittanbieterhinweise bleiben erhalten. Abhängigkeiten und ihre Autoren stehen in [CREDITS.md](CREDITS.md).
