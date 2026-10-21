using StarWarsUnlimited.DataParse;

Console.WriteLine("Hello, World!");

var webSite = "https://starwarsunlimited.com/cards";

var result = DataParserFromJson.ParseFromJson(webSite);

Console.WriteLine("Loading card list...");

