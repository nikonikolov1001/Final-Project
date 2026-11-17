# SPORTLINE

Начална версия на дипломен онлайн магазин за спортни стоки с ASP.NET Core MVC на .NET 10, Razor Views, Bootstrap, EF Core Code First и SQL Server.

## Етап 1

Налични са начална страница, прост каталог с търсене по име и подробности за размерите, цветовете и наличностите. Базата има 5 таблици и примерни данни за 6 продукта с 12 варианта.

Профили, роли, количка, поръчки, промоции и служебно управление са планирани за следващите етапи.

[Документация](docs/Etap-1.md) · [Word файл](docs/Etap-1-Documentation.docx)

## Подредба на решението

```text
SportStore.sln
SportStore.Core/
  Data/          Чисти примерни данни за каталога
  Dtos/          Данни, връщани от услугите
  Models/        Петте обекта на каталога
  Services/      IProductCatalogService
SportStore.Infrastructure/
  Data/          ApplicationDbContext, инициализатор и миграции
  Services/      ProductCatalogService
SportStore.Web/
  Controllers/   MVC действия
  Models/        Модели за уеб интерфейса
  Views/         Razor изгледи
  wwwroot/       Стилове и изображения
  Program.cs     Настройки и свързване на услугите
SportStore.Tests/
  Проверки на основното поведение на каталога
```

`Core` няма зависимости към другите проекти или към EF Core. `Infrastructure` използва `Core`, а `Web` използва `Core` и `Infrastructure`. Контролерите получават `IProductCatalogService`; EF реализацията е в `Infrastructure`. Тестовият проект проверява тази реализация и поведението на контролерите.

Подредбата следва разделянето на проектите във [FootballLeagueManagement](https://github.com/nikonikolov1001/FootballLeagueManagement). Уеб частта на SPORTLINE остава MVC с Razor Views.

## Стартиране

Необходими са Visual Studio 2026 с ASP.NET and web development, .NET 10 SDK и SQL Server LocalDB.

Само при първа подготовка на компютъра създайте локалната инстанция:

```powershell
SqlLocalDB create SportlineStage1Basic -s
```

Изпълнете от папката на решението:

```powershell
dotnet restore SportStore.sln
dotnet run --project SportStore.Web -- --initialize-database
dotnet run --project SportStore.Web
```

Отворете `http://localhost:5158`. Във Visual Studio отворете `SportStore.sln` и задайте `SportStore.Web` като начален проект.

Инициализаторът прилага съществуващата миграция и зарежда примерни данни само ако каталогът е празен. Повторното изпълнение не ги дублира. Обикновеното стартиране не променя схемата на базата.

## База и проверки

Контекстът и миграциите са в `SportStore.Infrastructure/Data`. Моделите са в `SportStore.Core/Models`. Преместването между проектите запазва таблиците и идентификатора на миграцията `20261106190602_InitialCatalogue`.

```powershell
dotnet tool restore
dotnet ef migrations has-pending-model-changes --project SportStore.Infrastructure --startup-project SportStore.Web
dotnet test SportStore.sln
```

Началната SQL схема е и в `database/initial-schema.sql`. За примерните записи използвайте инициализатора. Малкият тестов набор проверява съществуващите функции; пълните тестове на поръчки, наличности и отстъпки се развиват в следващите етапи.

Кодът и документацията са публично достъпни за четене от дипломния консултант.
