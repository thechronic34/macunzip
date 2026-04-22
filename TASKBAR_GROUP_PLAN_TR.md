# Windows 10/11 Taskbar Group Uygulaması (MVP Planı)

Bu belge, görev çubuğundaki birden fazla uygulama kısayolunu tek bir grup kısayolu altında toplamak için geliştirilecek masaüstü uygulaması için teknik bir yol haritası sunar.

## 1) Hedef

- Kullanıcı görev çubuğuna **tek bir kısayol** sabitler.
- Bu kısayola tıklanınca küçük bir "launcher" pencere açılır.
- Pencerede kullanıcı tarafından seçilmiş uygulamalar (ikon + ad) listelenir.
- Uygulama seçildiğinde ilgili `.exe`, `shell:AppsFolder` UWP hedefi veya özel komut çalıştırılır.

## 2) Platform ve teknoloji önerisi

### Önerilen yığın (MVP için)
- **.NET 8 + WPF**
- **C#**
- Paketleme: isteğe bağlı **MSIX** (ilk sürümde şart değil)

### Neden WPF?
- Hızlı prototipleme
- Tek exe ile dağıtım kolaylığı
- Win32 API entegrasyonu basit

## 3) Kullanıcı deneyimi (UX)

1. Kullanıcı uygulamayı ilk kez açar.
2. "Grup Düzenle" ekranında uygulama ekler:
   - `.exe` dosyası seç
   - UWP uygulaması seç
   - Özel komut satırı ekle
3. Sıralama ve ikon özelleştirme yapar.
4. "Taskbar'a sabitle" talimatını izler (uygulamanın kendisini sabitler).
5. Taskbar ikonuna her tıklamada mini pencere açılır.

## 4) Mimari

- `AppHost`:
  - Tek instance kontrolü
  - Komut satırı argümanları (`--open-popup`, `--settings`)
- `PopupWindow`:
  - Grup içi uygulamaları grid/list halinde gösterir
- `SettingsWindow`:
  - Grup yönetimi
- `ConfigStore`:
  - JSON tabanlı yapılandırma
- `LauncherService`:
  - Uygulama/komut başlatma
- `IconService`:
  - EXE/UWP ikon çözümleme

## 5) Veri modeli (örnek)

```json
{
  "groups": [
    {
      "id": "default",
      "name": "Çalışma",
      "items": [
        {
          "id": "chrome",
          "type": "exe",
          "path": "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
          "args": "",
          "displayName": "Chrome"
        },
        {
          "id": "teams",
          "type": "uwp",
          "appUserModelId": "MSTeams_8wekyb3d8bbwe!MSTeams",
          "displayName": "Teams"
        }
      ]
    }
  ]
}
```

## 6) Kritik teknik detaylar

### A) Taskbar'da tek ikon davranışı
- Uygulama ana exe'si taskbar'a pinlenir.
- Tıklandığında popup açılır.
- Popup için:
  - `WindowStyle=None`
  - `ShowInTaskbar=False`
  - `Topmost=True` (gerekirse)
  - Dışarı tıklayınca kapanma

### B) Görev çubuğu konumuna yakın açılma
- Win32 API ile taskbar konumu tespit edilir:
  - `SHAppBarMessage(ABM_GETTASKBARPOS, ...)`
- Popup, taskbar üstüne/yanına hizalanır.

### C) Uygulama başlatma
- Win32 exe için `Process.Start`.
- UWP için `shell:AppsFolder` + AppUserModelID ile başlatma.

### D) Tek instance
- `Mutex` ile tek instance kontrolü.
- İkinci çağrıda mevcut instance'a "popup aç" sinyali gönder.

## 7) Güvenlik ve stabilite

- Başlatılacak path/komut doğrulaması
- Hatalı hedefte kullanıcıya net hata mesajı
- Konfigürasyon dosyası bozulursa yedekten geri dönme

## 8) MVP backlog

1. Boş popup penceresi
2. JSON config okuma/yazma
3. EXE ekleme ve çalıştırma
4. UWP ekleme ve çalıştırma
5. Sıralama + silme
6. Taskbar konumuna göre popup hizalama
7. Başlangıçta çalıştır (opsiyonel)
8. Import/Export grup ayarı

## 9) Sonraki sürüm fikirleri

- Birden fazla grup (farklı taskbar kısayolları simülasyonu)
- Arama kutusu
- Klavye kısa yolları
- Tema (Light/Dark/System)
- Kullanım istatistikleri (lokal)

## 10) Basit pseudo akış

```text
App started
 ├─ load config
 ├─ if --settings => open settings
 └─ else => open popup near taskbar
      ├─ render items
      ├─ click item => launch
      └─ click outside => close
```

## 11) İlk çalışan iskelet için öneri

- `TaskbarGroup.App` (WPF)
- İlk sürümde sadece `exe` hedeflerini destekle
- JSON dosyasını `%AppData%\TaskbarGroup\config.json` altında tut
- UWP desteğini ikinci iterasyona bırak

---

Eğer istersen bir sonraki adımda sana doğrudan **çalışan bir WPF proje iskeleti** (dosya yapısı + temel kod) da hazırlayabilirim.
