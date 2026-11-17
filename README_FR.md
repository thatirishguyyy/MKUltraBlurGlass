# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Un fork personnel de maintenance de DWMBlurGlass 2.3.1 pour les effets sur les barres de titre et bordures des fenêtres Windows. Maintenu par **thatirishguyyy**.

**L’original a été créé par Maplespe.** Merci à Maplespe, ALTaleX531 et à tous les contributeurs et traducteurs. Voir les [crédits](CREDITS.md) et le [projet original](https://github.com/Maplespe/DWMBlurGlass).

## Version et téléchargement

**0.1.1** corrige la détection du mode sombre des fenêtres sur les versions récentes de Windows 11. Les fenêtres claires et sombres suivent indépendamment leurs couleurs de fenêtre active et inactive. Ce fork apporte aussi des corrections de géométrie et de contexte de couleur, une interface bleue, un son de clic désactivable dans About et un outil de désinstallation en anglais.

Un seul téléchargement suffit : [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) contient l’application et `MKUltraBlurGlassUninstall.exe`. L’archive des sources est facultative et destinée aux développeurs. Tous les téléchargements : [versions publiées](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Installation et mise à jour

1. Pour changer de copie, désinstaller d’abord l’hôte de l’ancienne copie, puis fermer son interface. Ne pas activer simultanément l’original et ce fork.
2. Extraire le ZIP complet hors du dossier de profil utilisateur, par exemple dans `C:\ProgramData\MKUltraBlurGlass`. Conserver ensemble le programme, les DLL et `data`.
3. Exécuter `MKUltraBlurGlass.exe` en tant qu’administrateur. Dans **Symbol**, télécharger les symboles correspondant à Windows, puis cliquer sur **Install** dans **General**. « Installed » ne garantit pas à lui seul des symboles valides ni un flou visible. Une mise à jour de Windows peut nécessiter de nouveaux symboles.
4. **Light Mode colors** et **Dark Mode colors** suivent le mode propre à chaque fenêtre. Enregistrer avec **Save**.

## Désinstallation

Ouvrir `MKUltraBlurGlassUninstall.exe`, vérifier le chemin détecté, demander les droits administrateur, puis cliquer sur **Uninstall**. Par défaut, les fichiers, réglages et symboles sont conservés. Le nettoyage facultatif supprime uniquement les quatre fichiers binaires de la copie sélectionnée ; le dossier de données est conservé. La tâche du DWMBlurGlass original est également reconnue.

## Tests et limites connues

Testé sur Windows 11 25H2 **26200.9974**. Pour 0.1.0, l’installation, la désinstallation, la réinstallation et le démarrage après redémarrage ont été vérifiés. Pour 0.1.1, la mise à jour et les couleurs indépendantes des fenêtres claires et sombres ont été confirmées ; le test de redémarrage n’a pas été répété pour ce correctif. Les autres versions de Windows et méthodes d’effet ne sont pas entièrement validées. Le menu Démarrer et la barre des tâches ne sont pas concernés.

Le sélecteur de couleur peut ne pas valider un choix avant le déplacement du point de sélection sur la palette. La reproduction exacte reste à préciser : [problèmes connus](KNOWN-ISSUES.md).

## Sources, documentation et licence

Branche maintenue : `mkultra/main`. Examiner et tester les changements du projet original avant de les intégrer ; ne pas synchroniser directement. Les noms historiques des projets sont conservés ; les fichiers produits sont `MKUltraBlurGlass.exe` et `MKUltraBlurGlassExt.dll`.

Documentation détaillée en anglais : [compilation](docs/BUILDING.md), [maintenance](docs/MAINTAINING.md), [dépannage](docs/TROUBLESHOOTING.md), [désinstallation](tools/uninstaller/README.md) et [notes de version](RELEASE-NOTES.md). Pour traduire, modifier les valeurs XML de `Languagefiles/` en conservant les clés, paramètres de substitution et crédits d’auteur.

GNU LGPL version 3 ou ultérieure : [COPYING.LESSER](COPYING.LESSER) et [LICENSE.txt](LICENSE.txt). Les mentions originales de droits d’auteur et de tiers sont conservées. Les dépendances et leurs auteurs figurent dans [CREDITS.md](CREDITS.md).
