# JpPublicHolidays

内閣府ホームページで公開されている「国民の祝日」CSVファイルを扱うためのライブラリです。

![国民の祝日](https://raw.githubusercontent.com/kiyoaki/JpPublicHolidays/master/nuget/default.png)

[![CI](https://github.com/kiyoaki/JpPublicHolidays/actions/workflows/ci.yml/badge.svg)](https://github.com/kiyoaki/JpPublicHolidays/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/JpPublicHolidays.svg)](https://www.nuget.org/packages/JpPublicHolidays/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 対応フレームワーク

- .NET Standard 2.0
- .NET Standard 2.1
- .NET 8.0

## インストール

NuGet Package Manager:
```
PM> Install-Package JpPublicHolidays
```

.NET CLI:
```
dotnet add package JpPublicHolidays
```

## Quick Start

### トップレベルステートメント (C# 9.0以降)
```csharp
using JpPublicHolidays;

var holidays = await PublicHolidays.Get();
foreach (var holiday in holidays)
{
    Console.WriteLine($"{holiday.Name} {holiday.Date.ToShortDateString()}");
}
```

### 従来のスタイル
```csharp
using JpPublicHolidays;
using System;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var holidays = await PublicHolidays.Get();
            foreach (var holiday in holidays)
            {
                Console.WriteLine($"{holiday.Name} {holiday.Date.ToShortDateString()}");
            }
        }
    }
}
```

## HolidayDatabase (MasterMemory)

祝日データを[MasterMemory](https://github.com/Cysharp/MasterMemory)を使用したインメモリデータベースとして保存・検索できます。

### データベースの作成と保存
```csharp
using JpPublicHolidays;

// APIからダウンロードしてデータベースを作成
var db = await HolidayDatabase.CreateFromApiAsync();

// ファイルに保存
db.Save("holidays.db");

// 既存のHoliday[]から作成することも可能
var holidays = await PublicHolidays.Get();
var dbFromHolidays = HolidayDatabase.Create(holidays);
```

### データベースの読み込み
```csharp
// ファイルから読み込み
var db = HolidayDatabase.LoadFromFile("holidays.db");

// byte[]から読み込み
var dbFromBytes = HolidayDatabase.LoadFromBytes(data);
```

### 日付で検索
```csharp
// 特定の日付の祝日を取得
var holiday = db.FindByDate(new DateTime(2025, 1, 1));
// holiday.Name => "元日"

// 祝日かどうかを確認
bool isHolidayJan1 = db.IsHoliday(new DateTime(2025, 1, 1)); // true
bool isHolidayJan2 = db.IsHoliday(new DateTime(2025, 1, 2)); // false

// TryFindByDateで安全に取得
if (db.TryFindByDate(new DateTime(2025, 1, 1), out var result))
{
    Console.WriteLine(result.Name);
}
```

### 名前で検索
```csharp
// 名前で検索（複数の結果が返される可能性あり）
var holidays = db.FindByName("元日");
// 2024年と2025年の元日が両方返される
```

### 日付範囲で検索
```csharp
// 日付範囲で検索
var holidays = db.FindByDateRange(
    new DateTime(2025, 5, 1),
    new DateTime(2025, 5, 31));
// 5月のすべての祝日
```

### 最も近い祝日を検索
```csharp
// 指定日以前で最も近い祝日
var closestBefore = db.FindClosestByDate(new DateTime(2025, 2, 1), selectLower: true);

// 指定日以降で最も近い祝日
var closestAfter = db.FindClosestByDate(new DateTime(2025, 2, 1), selectLower: false);
```

### その他のメソッド
```csharp
// すべての祝日を取得
var all = db.GetAll();

// 祝日の数を取得
int count = db.Count;
```

## テスト

テストを実行するには、以下のコマンドを使用してください:

```
dotnet run --project tests/JpPublicHolidays.Test/JpPublicHolidays.Test.csproj
```

または、ビルド後に実行する場合:

```
dotnet build
dotnet run --project tests/JpPublicHolidays.Test/JpPublicHolidays.Test.csproj --no-build
```

**注意:** このプロジェクトはNextUnitテストフレームワークを使用しているため、`dotnet test`コマンドはサポートされていません。テストの実行には`dotnet run`を使用してください。

## ライセンス

[MIT License](LICENSE)
