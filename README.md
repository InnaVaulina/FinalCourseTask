# FinalCourseTask

Краткое описание
-----------------
Учебный проект FinalCourseTask. Содержит несколько взаимосвязанных проектов:

- WebApplication1 — серверный ASP.NET Core (net6.0) с EF Core, Identity и JWT (API и аутентификация).
- TTB (net8.0) — хост‑сервер для Blazor WASM (ASP.NET Core hosted) и API для клиентской части.
- TTB.Client (net8.0) — Blazor WebAssembly клиент (WASM).
- TTClassLibrary (net8.0) — общая библиотека типов и логики для клиент/сервер/десктопа.
- WpfBLazorHybridClient (net8.0-windows) — WPF приложение с встраиваемым Blazor (BlazorWebView / WebView2).

Технологии
----------
- .NET 8 (основная часть проектов) и .NET 6 (WebApplication1)
- Blazor WebAssembly (клиентская часть) и Blazor встраиваемый WebView для WPF
- ASP.NET Core (хостинг, Web API)
- Entity Framework Core (SQL Server)
- ASP.NET Core Identity и JWT для аутентификации
- Swashbuckle/Swagger для документации API
- Microsoft.Web.WebView2 и Microsoft.AspNetCore.Components.WebView.Wpf для WPF‑гибридного клиента
- HtmlAgilityPack и другие вспомогательные библиотеки

Как запустить
-------------
1. Откройте решение в Visual Studio 2026.
2. Выберите стартовый проект (обычно Blazor или серверный проект) и запустите (F5).
3. Для командной строки:
   - dotnet restore
   - dotnet build
   - dotnet run --project Путь/К/Проекту

Контрибьюция
------------
1. Создайте ветку: `git checkout -b feature/your-feature`
2. Сделайте коммиты и отправьте ветку: `git push origin feature/your-feature`

Лицензия
--------
Лицензия: MIT — см. файл LICENSE.
