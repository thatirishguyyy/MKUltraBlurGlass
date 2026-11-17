# MKUltraBlurGlass

[English](README.md) · [Deutsch](README_DE.md) · [Español](README_ES.md) · [Français](README_FR.md) · [Italiano](README_IT.md) · [Português (Brasil)](README_PTBR.md) · [Türkçe](README_TR.md) · [Tiếng Việt](README_VI.md) · [简体中文](README_ZH.md)

Bản fork bảo trì cá nhân dựa trên DWMBlurGlass 2.3.1, tạo hiệu ứng cho thanh tiêu đề và viền cửa sổ Windows. Được **thatirishguyyy** duy trì.

**Dự án gốc do Maplespe tạo ra.** Xin cảm ơn Maplespe, ALTaleX531 cùng tất cả cộng tác viên và người dịch. Xem [ghi nhận đóng góp](CREDITS.md) và [dự án gốc](https://github.com/Maplespe/DWMBlurGlass).

## Phiên bản và tải xuống

**0.1.1** sửa lỗi nhận diện chế độ tối của cửa sổ trên các bản dựng Windows 11 mới. Các cửa sổ sáng và tối sử dụng độc lập màu tương ứng khi đang hoạt động hoặc không hoạt động. Bản fork cũng có các bản sửa hình học và ngữ cảnh màu, giao diện xanh dương, âm thanh bấm menu có thể tắt trong About và trình gỡ cài đặt tiếng Anh.

Chỉ cần tải một tệp: [MKUltraBlurGlass-0.1.1.zip](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases/download/v0.1.1/MKUltraBlurGlass-0.1.1.zip) chứa cả ứng dụng và `MKUltraBlurGlassUninstall.exe`. Gói mã nguồn dành cho nhà phát triển, không bắt buộc để sử dụng. Tất cả bản tải xuống: [các bản phát hành](https://github.com/thatirishguyyy/MKUltraBlurGlass/releases).

## Cài đặt và cập nhật

1. Khi chuyển sang bản khác, trước tiên gỡ thành phần chạy nền của bản cũ và đóng giao diện của nó. Không bật đồng thời bản gốc và bản fork này.
2. Giải nén toàn bộ ZIP ra ngoài thư mục hồ sơ người dùng, chẳng hạn `C:\ProgramData\MKUltraBlurGlass`. Giữ chương trình, các DLL và thư mục `data` cùng nhau.
3. Chạy `MKUltraBlurGlass.exe` với quyền quản trị viên. Trong **Symbol**, tải các tệp ký hiệu phù hợp với Windows, sau đó bấm **Install** trong **General**. Trạng thái «Installed» tự nó không đảm bảo ký hiệu hợp lệ hoặc hiệu ứng mờ đang hoạt động. Sau khi cập nhật Windows, có thể cần tải lại ký hiệu.
4. **Light Mode colors** và **Dark Mode colors** được áp dụng theo chế độ của từng cửa sổ. Bấm **Save** để lưu.

## Gỡ cài đặt

Mở `MKUltraBlurGlassUninstall.exe`, kiểm tra đường dẫn được nhận diện, yêu cầu quyền quản trị viên và bấm **Uninstall**. Theo mặc định, các tệp, cài đặt và ký hiệu được giữ lại. Tùy chọn dọn dẹp chỉ xóa bốn tệp nhị phân của bản đã chọn; thư mục dữ liệu được giữ nguyên. Tác vụ của DWMBlurGlass gốc cũng được nhận diện.

## Kiểm thử và giới hạn đã biết

Đã kiểm thử trên Windows 11 25H2 **26200.9974**. Với 0.1.0, đã xác nhận cài đặt, gỡ cài đặt, cài lại và tự khởi động sau khi khởi động lại Windows. Với 0.1.1, đã xác nhận cập nhật và màu độc lập của cửa sổ sáng/tối; chưa lặp lại phép thử khởi động lại riêng cho bản sửa này. Các bản dựng và phương thức hiệu ứng khác chưa được xác thực đầy đủ. Menu Start và thanh tác vụ không thuộc phạm vi của chương trình.

Bộ chọn màu có thể chưa áp dụng lựa chọn cho đến khi di chuyển điểm chọn trên bảng màu. Chưa xác nhận được các bước tái hiện chính xác: [các vấn đề đã biết](KNOWN-ISSUES.md).

## Mã nguồn, tài liệu và giấy phép

Nhánh được duy trì: `mkultra/main`. Xem xét và kiểm thử thay đổi từ dự án gốc trước khi hợp nhất; không đồng bộ trực tiếp. Tên lịch sử của các dự án được giữ lại; tệp đầu ra là `MKUltraBlurGlass.exe` và `MKUltraBlurGlassExt.dll`.

Tài liệu chi tiết bằng tiếng Anh: [biên dịch](docs/BUILDING.md), [bảo trì](docs/MAINTAINING.md), [khắc phục sự cố](docs/TROUBLESHOOTING.md), [trình gỡ cài đặt](tools/uninstaller/README.md) và [ghi chú phát hành](RELEASE-NOTES.md). Khi dịch, chỉnh sửa giá trị XML trong `Languagefiles/`; giữ nguyên khóa, chuỗi giữ chỗ và thông tin tác giả.

GNU LGPL phiên bản 3 trở lên: [COPYING.LESSER](COPYING.LESSER) và [LICENSE.txt](LICENSE.txt). Giữ nguyên thông báo bản quyền gốc và của bên thứ ba. Các thư viện phụ thuộc và tác giả được liệt kê trong [CREDITS.md](CREDITS.md).
