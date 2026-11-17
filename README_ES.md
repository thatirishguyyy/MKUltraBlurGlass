# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Una bifurcación personal de mantenimiento de DWMBlurGlass 2.3.1 para efectos en las barras de título y bordes de las ventanas de Windows. Mantenida por **thatirishguyyy**.

**El original fue creado por Maplespe.** Gracias a Maplespe, ALTaleX531 y todos los colaboradores y traductores. Consulta los [créditos](CREDITS.md) y el [proyecto original](https://github.com/Maplespe/DWMBlurGlass).

## Versión y descarga

**0.1.1** corrige la detección del modo oscuro de las ventanas en compilaciones recientes de Windows 11. Las ventanas claras y oscuras utilizan de forma independiente sus colores de ventana activa e inactiva. Esta bifurcación también incluye correcciones de geometría y contexto de color, una interfaz azul, un clic de menú que se puede silenciar en About y un desinstalador en inglés.

Solo necesitas una descarga: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) incluye la aplicación y `MKUltraBlurGlassUninstall.exe`. El archivo de código fuente es opcional para desarrolladores. Todas las descargas: [versiones publicadas](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Instalación y actualización

1. Para cambiar de copia, desinstala primero el host anterior y cierra su interfaz. No actives el original y esta bifurcación a la vez.
2. Extrae el ZIP completo fuera de tu carpeta de perfil, por ejemplo en `C:\ProgramData\MKUltraBlurGlass`. Mantén juntos el programa, las DLL y `data`.
3. Ejecuta `MKUltraBlurGlass.exe` como administrador. En **Symbol**, descarga los símbolos correspondientes a Windows; después pulsa **Install** en **General**. «Installed» por sí solo no garantiza símbolos válidos ni desenfoque visible. Las actualizaciones de Windows pueden requerir nuevos símbolos.
4. **Light Mode colors** y **Dark Mode colors** corresponden al modo de cada ventana. Guarda con **Save**.

## Desinstalación

Abre `MKUltraBlurGlassUninstall.exe`, comprueba la ruta detectada, solicita permisos de administrador y pulsa **Uninstall**. Por defecto se conservan archivos, ajustes y símbolos. La limpieza opcional elimina únicamente los cuatro binarios de la copia seleccionada; conserva la carpeta de datos. También reconoce la tarea del DWMBlurGlass original.

## Pruebas y limitaciones conocidas

Probado en Windows 11 25H2 **26200.9974**. En 0.1.0 se verificaron instalación, desinstalación, reinstalación e inicio tras reiniciar. En 0.1.1 se confirmaron la actualización y los colores independientes de ventanas claras y oscuras; no se repitió la prueba de reinicio para esta corrección. Otras compilaciones y métodos de efecto no están completamente validados. El menú Inicio y la barra de tareas quedan fuera del alcance.

El selector de color puede no aplicar una selección hasta mover el punto de selección en la paleta. Aún faltan pasos exactos de reproducción: [problemas conocidos](KNOWN-ISSUES.md).

## Código, documentación y licencia

Rama mantenida: `mkultra/main`. Revisa y prueba los cambios del proyecto original antes de integrarlos; no sincronices directamente. Se conservan los nombres históricos de los proyectos; los archivos generados son `MKUltraBlurGlass.exe` y `MKUltraBlurGlassExt.dll`.

Documentación detallada en inglés: [compilación](docs/BUILDING.md), [mantenimiento](docs/MAINTAINING.md), [solución de problemas](docs/TROUBLESHOOTING.md), [desinstalador](tools/uninstaller/README.md) y [notas de versión](RELEASE-NOTES.md). Para traducir, modifica los valores XML de `Languagefiles/` y conserva claves, marcadores y créditos de autor.

GNU LGPL versión 3 o posterior: [COPYING.LESSER](COPYING.LESSER) y [LICENSE.txt](LICENSE.txt). Se conservan los avisos originales de derechos de autor y de terceros. Las dependencias y sus autores figuran en [CREDITS.md](CREDITS.md).
