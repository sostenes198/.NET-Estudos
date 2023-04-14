// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");

var result = Regex.Match("AppGlobal/4.47.37 iPhone12,5 iOS/16.1.1 CFNetwork/1399 Darwin/22.1.0",
                           "AppGlobal/(?<AppVersion>[\\S]+)", RegexOptions.Singleline);

var result1 = Regex.Match("AppGlobal/4.47.37 iPhone12,5 iOS/16.1.1 CFNetwork/1399 Darwin/22.1.0",
                         "AppGlobal/(?<AppVersion>[\\S]+\\s)(?<Platform>\\S+\\s)\\w+/{1}(?<VersionDevice>\\S+\\s)", RegexOptions.Singleline);

var result2 = Regex.Matches("AppGlobal/4.47.37 i Phone12,5 iOS/16.1.1 CFNetwork/1399 Darwin/22.1.0",
                          "AppGlobal/(?<AppVersion>[\\S]+\\s)|(?<Platform>\\S+\\s)\\w+/{1}(?<VersionDevice>\\S+\\s)", RegexOptions.Singleline);

var x =10;