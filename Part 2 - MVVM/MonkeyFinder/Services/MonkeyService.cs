using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpclient;
    public MonkeyService()
    {
        this.httpclient=new HttpClient ();
    }
    List<Monkey> monkeyList = new List<Monkey>();
    public async Task<List<Monkey>> GetMonkeys()
    {
        if(monkeyList?.Count > 0)
        {
            return monkeyList;
        }
        var response = await httpclient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
        }

        using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader=new StreamReader(stream);
        var contents=await reader.ReadToEndAsync();
        monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);
        return monkeyList;
    }
}
