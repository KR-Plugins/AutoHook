using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Dalamud;

namespace Parser;

public partial class FishingParser
{
    private readonly struct Regexes
    {
        public Regex Cast { get; private init; }
        public string Undiscovered { get; private init; }
        public Regex AreaDiscovered { get; private init; }
        public Regex Mooch { get; private init; }

        public static Regexes FromLanguage(ClientLanguage lang)
        {
            return lang switch
            {
                ClientLanguage.English => English.Value,
                ClientLanguage.German => German.Value,
                ClientLanguage.French => French.Value,
                ClientLanguage.Japanese => Japanese.Value,
                ClientLanguage.Korean => Korean.Value,
                _ => ChineseSimplified.Value,
            };
        }

        // @formatter:off


        private static readonly Lazy<Regexes> English = new(() => new Regexes
        {
            Cast = new Regex(@"(?:You cast your|.*? casts (?:her|his)) line (?:on|in|at) (?<FishingSpot>.+)\.", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@".*?(on|at) (?<FishingSpot>.+) is added to your fishing log\.", RegexOptions.Compiled),
            Mooch = new Regex(@"line with the fish still hooked.", RegexOptions.Compiled),
            Undiscovered = "undiscovered fishing hole",
        });

        private static readonly Lazy<Regexes> German = new(() => new Regexes
        {
            Cast = new Regex(@"Du hast mit dem Fischen (?<FishingSpotWithArticle>.+) begonnen\.(?<FishingSpot>invalid)?", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@"Die neue Angelstelle (?<FishingSpot>.*) wurde in deinem Fischer-Notizbuch vermerkt\.", RegexOptions.Compiled),
            Mooch = new Regex(@"Du hast die Leine mit", RegexOptions.Compiled),
            Undiscovered = "unerforschten Angelplatz",
        });

        private static readonly Lazy<Regexes> French = new(() => new Regexes
        {
            Cast = new Regex(@"Vous commencez à pêcher\.\s*Point de pêche: (?<FishingSpot>.+)\.", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@"Vous notez le banc de poissons “(?<FishingSpot>.+)” dans votre carnet\.", RegexOptions.Compiled),
            Mooch = new Regex(@"Vous essayez de pêcher au vif avec", RegexOptions.Compiled),
            Undiscovered = "Zone de pêche inconnue",
        });

        private static readonly Lazy<Regexes> Japanese = new(() => new Regexes
        {
            Cast = new Regex(@".+\u306f(?<FishingSpot>.+)で釣りを開始した。", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@"釣り手帳に新しい釣り場「(?<FishingSpot>.+)」の情報を記録した！", RegexOptions.Compiled),
            Mooch = new Regex(@"は釣り上げた.+を慎重に投げ込み、泳がせ釣りを試みた。", RegexOptions.Compiled),
            Undiscovered = "未知の釣り場",
        });

        private static readonly Lazy<Regexes> ChineseSimplified = new(() => new Regexes
        {
            Cast = new Regex(@"在(?<FishingSpot>.+)甩出了鱼线开始钓鱼", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@"将新钓场(?<FishingSpot>.+)记录到了钓鱼笔记中！", RegexOptions.Compiled),
            Mooch = new Regex(@"开始利用上钩的.+尝试以小钓大。", RegexOptions.Compiled),
            Undiscovered = "未知钓场",
        });
        private static readonly Lazy<Regexes> Korean = new(() => new Regexes
        {
            Cast = new Regex(@".+ 님이 (?<FishingSpot>.+)에서 낚시를 시작합니다\.", RegexOptions.Compiled),
            AreaDiscovered = new Regex(@"낚시 수첩에 새로운 낚시터 (?<FishingSpot>.+)의 정보를 기록했습니다!", RegexOptions.Compiled),
            Mooch = new Regex(@"생미끼 낚시를 시도합니다\.", RegexOptions.Compiled),
            Undiscovered = "미지의 낚시터",
        });
        // @formatter:on
    }
}
