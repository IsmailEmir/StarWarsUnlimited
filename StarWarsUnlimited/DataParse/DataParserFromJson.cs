using HtmlAgilityPack;

namespace StarWarsUnlimited.DataParse;

public class DataParserFromJson
{
    public static string ParseFromJson(string json)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/141.0.0.0");
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(json);
        
        var doc = new HtmlWeb().Load("https://starwarsunlimited.com/cards");

        var node = doc.DocumentNode.SelectSingleNode(
            "/html/body/div[@id='__next']/div/main/div[contains(@class,'bg-neutral-900')]/div[@class='container pt-4 lg:pt-8 pb-16 lg:pb-24']/div[contains(@class,'mb-12')]/div[contains(@class,'flex md:grid')]/div[@class='md:col-span-2']/div[@class='flex justify-between items-center md:grid md:grid-cols-[minmax(0,1fr)_minmax(0,1fr)] gap-6']/div[@class='text-neutral-50 text-sm']");
        
        var cardAmount = node.InnerText;

        return cardAmount;
    }
}