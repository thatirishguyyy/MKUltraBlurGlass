# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Windows pencere başlık çubukları ve kenarlıkları için efektler sunan, DWMBlurGlass 2.3.1 tabanlı kişisel bir bakım çatalıdır. Bakımını **thatirishguyyy** üstlenmektedir.

**Özgün proje Maplespe tarafından oluşturulmuştur.** Maplespe, ALTaleX531 ve tüm katkıda bulunanlara ve çevirmenlere teşekkür ederiz. [Katkılar](CREDITS.md) ve [özgün proje](https://github.com/Maplespe/DWMBlurGlass) sayfalarına bakın.

## Sürüm ve indirme

**0.1.1**, güncel Windows 11 derlemelerinde pencerenin koyu modunu algılama sorununu düzeltir. Açık ve koyu pencereler kendi etkin ve etkin olmayan pencere renklerini bağımsız olarak kullanır. Bu çatal ayrıca geometri ve renk bağlamı düzeltmeleri, mavi arayüz, About bölümünden kapatılabilen menü sesi ve İngilizce kaldırma aracı içerir.

Tek indirme yeterlidir: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip), uygulamayı ve `MKUltraBlurGlassUninstall.exe` dosyasını içerir. Kaynak kod arşivi geliştiriciler için isteğe bağlıdır. Tüm indirmeler: [sürümler](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Kurulum ve güncelleme

1. Başka bir kopyaya geçerken önce önceki kopyanın arka plan bileşenini kaldırın ve arayüzünü kapatın. Özgün sürümü ve bu çatalı aynı anda etkinleştirmeyin.
2. ZIP’in tamamını kullanıcı profili klasörünün dışına, örneğin `C:\ProgramData\MKUltraBlurGlass` konumuna çıkarın. Programı, DLL’leri ve `data` klasörünü bir arada tutun.
3. `MKUltraBlurGlass.exe` dosyasını yönetici olarak çalıştırın. **Symbol** bölümünden Windows sürümünüze uygun sembolleri indirin, ardından **General** bölümünde **Install** düğmesine basın. «Installed» durumu tek başına geçerli semboller veya görünür bulanıklık anlamına gelmez. Windows güncellemelerinden sonra yeni semboller gerekebilir.
4. **Light Mode colors** ve **Dark Mode colors**, her pencerenin kendi moduna göre uygulanır. **Save** ile kaydedin.

## Kaldırma

`MKUltraBlurGlassUninstall.exe` dosyasını açın, algılanan yolu kontrol edin, yönetici yetkisi isteyin ve **Uninstall** düğmesine basın. Varsayılan olarak dosyalar, ayarlar ve semboller korunur. İsteğe bağlı temizleme yalnızca seçili kopyanın dört program ikili dosyasını siler; veri klasörü korunur. Özgün DWMBlurGlass görevi de tanınır.

## Testler ve bilinen sınırlamalar

Windows 11 25H2 **26200.9974** üzerinde test edilmiştir. 0.1.0 için kurulum, kaldırma, yeniden kurulum ve yeniden başlatma sonrası açılış doğrulanmıştır. 0.1.1 için güncelleme ve açık/koyu pencere renklerinin bağımsız çalışması doğrulanmıştır; bu düzeltme için yeniden başlatma testi tekrarlanmamıştır. Diğer derlemeler ve efekt yöntemleri tam olarak doğrulanmamıştır. Başlat menüsü ve görev çubuğu kapsam dışındadır.

Renk seçici, palet üzerindeki seçim noktası hareket ettirilene kadar bir seçimi uygulamayabilir. Kesin yeniden üretme adımları henüz doğrulanmamıştır: [bilinen sorunlar](KNOWN-ISSUES.md).

## Kaynak kod, belgeler ve lisans

Bakımı yapılan dal: `mkultra/main`. Özgün projedeki değişiklikleri birleştirmeden önce inceleyin ve test edin; doğrudan eşitlemeyin. Projelerin tarihsel adları korunur; çıktı dosyaları `MKUltraBlurGlass.exe` ve `MKUltraBlurGlassExt.dll` olarak adlandırılır.

Ayrıntılı İngilizce belgeler: [derleme](docs/BUILDING.md), [bakım](docs/MAINTAINING.md), [sorun giderme](docs/TROUBLESHOOTING.md), [kaldırma aracı](tools/uninstaller/README.md) ve [sürüm notları](RELEASE-NOTES.md). Çeviri için `Languagefiles/` içindeki XML değerlerini düzenleyin; anahtarları, yer tutucuları ve yazar bilgilerini koruyun.

GNU LGPL sürüm 3 veya sonrası: [COPYING.LESSER](COPYING.LESSER) ve [LICENSE.txt](LICENSE.txt). Özgün telif hakkı ve üçüncü taraf bildirimleri korunur. Bağımlılıklar ve yazarları [CREDITS.md](CREDITS.md) içinde listelenir.
