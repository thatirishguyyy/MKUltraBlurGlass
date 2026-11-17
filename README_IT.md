# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Un fork personale di manutenzione di DWMBlurGlass 2.3.1 per gli effetti sulle barre del titolo e sui bordi delle finestre di Windows. Mantenuto da **thatirishguyyy**.

**L’originale è stato creato da Maplespe.** Grazie a Maplespe, ALTaleX531 e a tutti i collaboratori e traduttori. Vedi i [riconoscimenti](CREDITS.md) e il [progetto originale](https://github.com/Maplespe/DWMBlurGlass).

## Versione e download

**0.1.1** corregge il rilevamento della modalità scura delle finestre nelle build recenti di Windows 11. Le finestre chiare e scure seguono indipendentemente i rispettivi colori per lo stato attivo e inattivo. Questo fork include inoltre correzioni della geometria e del contesto colore, un’interfaccia blu, un suono del menu disattivabile in About e un programma di disinstallazione in inglese.

Basta un download: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) include l’applicazione e `MKUltraBlurGlassUninstall.exe`. L’archivio del codice sorgente è facoltativo per gli sviluppatori. Tutti i download: [release](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Installazione e aggiornamento

1. Per passare a un’altra copia, disinstalla prima l’host precedente e chiudi la sua interfaccia. Non attivare contemporaneamente l’originale e questo fork.
2. Estrai tutto lo ZIP fuori dalla cartella del profilo utente, ad esempio in `C:\ProgramData\MKUltraBlurGlass`. Mantieni insieme programma, DLL e `data`.
3. Esegui `MKUltraBlurGlass.exe` come amministratore. In **Symbol**, scarica i simboli corrispondenti a Windows, poi premi **Install** in **General**. «Installed» da solo non garantisce simboli validi o una sfocatura visibile. Gli aggiornamenti di Windows possono richiedere nuovi simboli.
4. **Light Mode colors** e **Dark Mode colors** seguono la modalità della singola finestra. Salva con **Save**.

## Disinstallazione

Apri `MKUltraBlurGlassUninstall.exe`, controlla il percorso rilevato, richiedi i privilegi di amministratore e premi **Uninstall**. Per impostazione predefinita, file, impostazioni e simboli vengono conservati. La pulizia facoltativa rimuove solo i quattro file binari della copia selezionata; la cartella dei dati viene mantenuta. Viene riconosciuta anche l’attività del DWMBlurGlass originale.

## Test e limiti noti

Testato su Windows 11 25H2 **26200.9974**. Per 0.1.0 sono stati verificati installazione, disinstallazione, reinstallazione e avvio dopo il riavvio. Per 0.1.1 sono stati confermati l’aggiornamento e i colori indipendenti delle finestre chiare e scure; il test di riavvio non è stato ripetuto per questa correzione. Altre build e modalità degli effetti non sono completamente convalidate. Il menu Start e la barra delle applicazioni non rientrano nell’ambito del programma.

Il selettore di colore potrebbe non applicare una scelta finché non si sposta il punto di selezione sulla tavolozza. I passaggi esatti per riprodurre il problema sono ancora da confermare: [problemi noti](KNOWN-ISSUES.md).

## Codice, documentazione e licenza

Ramo mantenuto: `mkultra/main`. Esamina e testa le modifiche del progetto originale prima di integrarle; non sincronizzare direttamente. I nomi storici dei progetti restano invariati; i file prodotti sono `MKUltraBlurGlass.exe` e `MKUltraBlurGlassExt.dll`.

Documentazione dettagliata in inglese: [compilazione](docs/BUILDING.md), [manutenzione](docs/MAINTAINING.md), [risoluzione dei problemi](docs/TROUBLESHOOTING.md), [disinstallazione](tools/uninstaller/README.md) e [note di rilascio](RELEASE-NOTES.md). Per tradurre, modifica i valori XML in `Languagefiles/`, conservando chiavi, segnaposto e crediti degli autori.

GNU LGPL versione 3 o successiva: [COPYING.LESSER](COPYING.LESSER) e [LICENSE.txt](LICENSE.txt). Restano validi gli avvisi originali di copyright e di terze parti. Le dipendenze e i loro autori sono elencati in [CREDITS.md](CREDITS.md).
