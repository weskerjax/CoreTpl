
EF 指令使用
=================================================

在 DbContext 的專案 (.csproj 的目錄) 開啟 cmd 指令視窗。

從 3.0 起，EF Core 命令列工具 (dotnet ef) 不再包含於 .NET Core SDK 裡，必須額外安裝。
安裝指令為 dotnet tool install --global dotnet-ef

Ref: https://docs.microsoft.com/zh-tw/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli


建立移轉
------------------------------------------

dotnet ef migrations add [MigrateName]


更新資料庫
------------------------------------------

dotnet ef database update



注意事項
=================================================


EF 指令工具需要依附在 Main 的進入點上，請在 DAO 專案加入 Program.cs 並保含一下內容。

    class Program
    {
        public static void Main(string[] args) 
        {
            TplDbContext dc = new DesignTimeDbContextFactory().CreateDbContext(args);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TplDbContext>
    {
        public TplDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TplDbContext>();
            builder.UseNpgsql("Host=localhost;Database=CoreTpl;Username=postgres;Password=p@ssw0rd");
            return new TplDbContext(builder.Options);
        }
    }


EF 指令工具如果發生找不到 DbContext 或奇怪的錯誤，很有可能是 DbContext 在執行階段發生錯誤。
可以將 DAO 的專案屬性 OutputType 從 Library 改成 Exe，並設置為啟始專案進行測試。 

    <Project Sdk="Microsoft.NET.Sdk">

      <PropertyGroup>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <OutputType>Exe</OutputType>
      </PropertyGroup>    

