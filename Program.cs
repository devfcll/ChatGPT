using System.Text;
using System.Text.Json;

while (true)
{
    Console.WriteLine("Digite sua pergunta:");
    var prompt = Console.ReadLine();

    if (prompt.ToLower() == "sair")
        break;

    if (prompt.ToLower().StartsWith("imagem"))
        await imagem(prompt);
    else
        await pergunta(prompt);
}

async Task pergunta(string prompt)
{
    if (String.IsNullOrWhiteSpace(prompt))
        return;

    string apiKey = "YOUR API KEY";

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
        var response = await client.PostAsync("https://api.openai.com/v1/completions",
        new StringContent("{\"model\": \"text-davinci-003\", \"prompt\": \"" + prompt + "\", \"temperature\": 1, \"max_tokens\": 1024}", Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Resposta data = JsonSerializer.Deserialize<Resposta>(responseString);
            Console.ForegroundColor = ConsoleColor.Green;

            Array.ForEach(data.choices.ToArray(), (item) => Console.WriteLine(item.text.Replace("\n", "")));

            Console.ResetColor();

        }
        else
        {
            Console.WriteLine("Ocorreu um erro ao enviar a pergunta.");
        }
    }
}

async Task imagem(string prompt)
{
    if (String.IsNullOrWhiteSpace(prompt))
        return;

    string apiKey = "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";

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
        var response = await client.PostAsync("https://api.openai.com/v1/images/generations",
        new StringContent("{\"prompt\": \"" + prompt + "\", \"n\": 1, \"size\": \"1024x1024\"}", Encoding.UTF8, "application/json"));

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            Resposta resposta = JsonSerializer.Deserialize<Resposta>(responseString);
            Console.ForegroundColor = ConsoleColor.Red;

            Array.ForEach(resposta.data.ToArray(), (item) => Console.WriteLine(item.url.Replace("\n", "")));

            Console.ResetColor();

        }
        else
        {
            Console.WriteLine("Ocorreu um erro ao enviar a pergunta.");
        }
    }
}

class Resposta
{
    public List<Choice> choices { get; set; }
    public Data[] data { get; set; }
    public class Choice
    {
        public string text { get; set; }
    }

    public class Data
    {
        public string url { get; set; }

    }
}


