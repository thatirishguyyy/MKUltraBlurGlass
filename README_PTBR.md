# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Um fork pessoal de manutenção do DWMBlurGlass 2.3.1 para efeitos nas barras de título e bordas das janelas do Windows. Mantido por **thatirishguyyy**.

**O original foi criado por Maplespe.** Agradecemos a Maplespe, ALTaleX531 e a todos os colaboradores e tradutores. Veja os [créditos](CREDITS.md) e o [projeto original](https://github.com/Maplespe/DWMBlurGlass).

## Versão e download

**0.1.1** corrige a detecção do modo escuro das janelas em builds recentes do Windows 11. As janelas claras e escuras seguem de forma independente suas respectivas cores para os estados ativo e inativo. Este fork também inclui correções de geometria e contexto de cor, interface azul, som de clique desativável em About e um desinstalador em inglês.

Basta um download: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) inclui o aplicativo e `MKUltraBlurGlassUninstall.exe`. O arquivo de código-fonte é opcional para desenvolvedores. Todos os downloads: [versões publicadas](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Instalação e atualização

1. Para trocar de cópia, primeiro desinstale o host anterior e feche sua interface. Não ative o original e este fork ao mesmo tempo.
2. Extraia o ZIP completo fora da pasta do perfil de usuário, por exemplo em `C:\ProgramData\MKUltraBlurGlass`. Mantenha juntos o programa, as DLLs e `data`.
3. Execute `MKUltraBlurGlass.exe` como administrador. Em **Symbol**, baixe os símbolos correspondentes ao Windows; depois clique em **Install**, em **General**. «Installed» sozinho não garante símbolos válidos nem desfoque visível. Atualizações do Windows podem exigir novos símbolos.
4. **Light Mode colors** e **Dark Mode colors** seguem o modo de cada janela. Salve com **Save**.

## Desinstalação

Abra `MKUltraBlurGlassUninstall.exe`, confira o caminho detectado, solicite privilégios de administrador e clique em **Uninstall**. Por padrão, arquivos, configurações e símbolos são preservados. A limpeza opcional remove apenas os quatro arquivos binários da cópia selecionada; a pasta de dados é mantida. A tarefa do DWMBlurGlass original também é reconhecida.

## Testes e limitações conhecidas

Testado no Windows 11 25H2 **26200.9974**. Na versão 0.1.0 foram verificados instalação, desinstalação, reinstalação e início após reiniciar. Na 0.1.1 foram confirmados a atualização e os controles independentes de cores de janelas claras e escuras; o teste de reinicialização não foi repetido para esta correção. Outras builds e métodos de efeito não foram totalmente validados. O menu Iniciar e a barra de tarefas ficam fora do escopo.

O seletor de cores pode não aplicar uma escolha até que o ponto de seleção seja movido na paleta. Os passos exatos de reprodução ainda precisam ser confirmados: [problemas conhecidos](KNOWN-ISSUES.md).

## Código, documentação e licença

Branch mantida: `mkultra/main`. Revise e teste as alterações do projeto original antes de integrá-las; não sincronize diretamente. Os nomes históricos dos projetos são preservados; os arquivos gerados são `MKUltraBlurGlass.exe` e `MKUltraBlurGlassExt.dll`.

Documentação detalhada em inglês: [compilação](docs/BUILDING.md), [manutenção](docs/MAINTAINING.md), [solução de problemas](docs/TROUBLESHOOTING.md), [desinstalador](tools/uninstaller/README.md) e [notas de versão](RELEASE-NOTES.md). Para traduzir, edite os valores XML em `Languagefiles/`, preservando chaves, marcadores e créditos de autoria.

GNU LGPL versão 3 ou posterior: [COPYING.LESSER](COPYING.LESSER) e [LICENSE.txt](LICENSE.txt). Os avisos originais de direitos autorais e de terceiros são preservados. As dependências e seus autores estão em [CREDITS.md](CREDITS.md).
