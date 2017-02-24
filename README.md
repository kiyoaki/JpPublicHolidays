# JpPublicHolidays.NET
内閣府ホームページで公開されている「国民の祝日」CSVファイルを扱うためのライブラリです

[![Build status](https://ci.appveyor.com/api/projects/status/nkdp4cwcnn8ofe0i?svg=true)](https://ci.appveyor.com/project/kiyoaki/jppublicholidays-net)

Install
---
* PM> Install-Package [JpPublicHolidays](https://www.nuget.org/packages/JpPublicHolidays/)

Quick Start
---
```csharp
class Program
{
    static void Main(string[] args)
    {
        var days = PublicHolidays.Get().Result;
        foreach (var holiday in days)
        {
            Console.WriteLine($"{holiday.Name} {holiday.Date.ToShortDateString()}");
        }
        Console.ReadKey();
    }
}
```
Console output
```
元日 2016/01/01
成人の日 2016/01/11
建国記念の日 2016/02/11
春分の日 2016/03/20
昭和の日 2016/04/29
憲法記念日 2016/05/03
みどりの日 2016/05/04
こどもの日 2016/05/05
海の日 2016/07/18
山の日 2016/08/11
敬老の日 2016/09/19
秋分の日 2016/09/22
体育の日 2016/10/10
文化の日 2016/11/03
勤労感謝の日 2016/11/23
天皇誕生日 2016/12/23
元日 2017/01/01
成人の日 2017/01/09
建国記念の日 2017/02/11
春分の日 2017/03/20
昭和の日 2017/04/29
憲法記念日 2017/05/03
みどりの日 2017/05/04
こどもの日 2017/05/05
海の日 2017/07/17
山の日 2017/08/11
敬老の日 2017/09/18
秋分の日 2017/09/23
体育の日 2017/10/09
文化の日 2017/11/03
勤労感謝の日 2017/11/23
天皇誕生日 2017/12/23
元日 2018/01/01
成人の日 2018/01/08
建国記念の日 2018/02/11
春分の日 2018/03/21
昭和の日 2018/04/29
憲法記念日 2018/05/03
みどりの日 2018/05/04
こどもの日 2018/05/05
海の日 2018/07/16
山の日 2018/08/11
敬老の日 2018/09/17
秋分の日 2018/09/23
体育の日 2018/10/08
文化の日 2018/11/03
勤労感謝の日 2018/11/23
天皇誕生日 2018/12/23
```
