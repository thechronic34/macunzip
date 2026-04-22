# TaskbarGroup.App Arayüz Test Rehberi (Windows 10/11)

Bu rehber, uygulamanın arayüzünü manuel olarak test etmen için adım adım senaryo içerir.

## 1) Ön koşullar

- Windows 10 veya Windows 11
- .NET 8 SDK

Kontrol:

```powershell
dotnet --version
```

## 2) Uygulamayı çalıştırma

Repo kökünde:

```powershell
dotnet run --project .\TaskbarGroup.App\TaskbarGroup.App.csproj
```

Ayarlar ekranını direkt açmak için:

```powershell
dotnet run --project .\TaskbarGroup.App\TaskbarGroup.App.csproj -- --settings
```

## 3) Arayüz test senaryoları

### Senaryo A — Popup açılışı ve kapanışı
1. Uygulamayı normal şekilde başlat.
2. Popup pencerenin taskbar'a yakın açıldığını doğrula.
3. Pencere dışına tıkla.
4. Pencerenin kapandığını doğrula.

Beklenen:
- Pencere görev çubuğuna yakın konumda açılır.
- Dışarı tıklayınca kapanır.

### Senaryo B — EXE ekleme ve çalıştırma
1. `--settings` ile ayarlar ekranını aç.
2. **EXE Ekle** butonuyla bir uygulama seç (ör. notepad.exe).
3. **Kaydet** yap ve ayarlar penceresini kapat.
4. Uygulamayı normal aç, listeden eklediğin öğeyi seç ve **Aç** tıkla.

Beklenen:
- Uygulama listede görünür.
- Tıklandığında ilgili EXE açılır.

### Senaryo C — UWP ekleme (manuel)
1. Ayarlarda **UWP Ekle** de.
2. Eklenen satırın `AUMID` sütununu doldur.
3. **Kaydet**.
4. Normal popup üzerinden öğeyi seçip **Aç** de.

Beklenen:
- Geçerli AUMID ise UWP uygulaması açılır.

### Senaryo D — UWP tarama
1. Ayarlarda **UWP'leri Tara** butonuna bas.
2. Listeye uygulamalar eklendiğini doğrula.
3. **Kaydet**.

Beklenen:
- Daha önce olmayan UWP öğeleri listeye eklenir.
- Aynı AUMID ikinci kez eklenmez.

### Senaryo E — Sıralama
1. Ayarlarda bir öğe seç.
2. **Yukarı/Aşağı** ile yerini değiştir.
3. **Kaydet**.
4. Popup'ta sıralamanın korunduğunu doğrula.

Beklenen:
- Sıralama popup listesine yansır.

## 4) Config dosyası kontrolü

Konum:

```text
%AppData%\TaskbarGroup\config.json
```

Beklenen:
- Eklenen/silinen/sıralanan öğeler bu dosyada güncellenmiş olmalı.

## 5) Sık hatalar

- `dotnet` bulunamıyor:
  - .NET 8 SDK kurulu değil.
- UWP açılmıyor:
  - AUMID hatalı olabilir.
- Popup yanlış yerde:
  - Çoklu monitör/dikey taskbar gibi düzenlerde ince ayar gerekebilir.

## 6) Hızlı smoke checklist

- [ ] Popup açılıyor
- [ ] Dışarı tıklayınca kapanıyor
- [ ] EXE öğesi açılıyor
- [ ] UWP öğesi açılıyor
- [ ] UWP tarama çalışıyor
- [ ] Sıralama kaydoluyor
