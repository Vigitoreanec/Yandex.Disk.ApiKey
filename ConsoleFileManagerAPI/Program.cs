
using YandexDisk.Client;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

Console.WriteLine("Server is Run");

var clientApi = new DiskHttpApi("y0__xDrtJkFGLijNiCk6JPNEvRjUsH5le-oyOMziushmDklJQl0");

    //var rootFolder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
    //{
    //    Path = "/",

    //});

    //foreach (var item in rootFolder.Embedded.Items)
    //{

    //    if (string.Equals(item.Type.ToString(), "dir", StringComparison.OrdinalIgnoreCase) ||
    //    string.Equals(item.Type.ToString(), "folder", StringComparison.OrdinalIgnoreCase))
    //    {
    //        Console.WriteLine("==========");
    //        Console.WriteLine($"Папка: {item.Name}");
    //        item.Path = $"/{item.Name}";
    //        Console.WriteLine("==========");
    //    }
    //    else // файл
    //    {
    //        Console.WriteLine($"{item.Name,-25}\t\t{item.Type,-10}\t{item.MimeType,-10}\t\t{item.Created:yyyy-MM-dd}");
    //    }
    //}
    async Task ProcessFolderAsync(IDiskApi clientApi, string path, string indent = "")
{
    var folder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
    {
        Path = path
    });

    foreach (var item in folder.Embedded.Items)
    {
        if (string.Equals(item.Type.ToString(), "dir", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(item.Type.ToString(), "folder", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"{indent}==========");
            Console.WriteLine($"{indent}Папка: {item.Name}");
            Console.WriteLine($"{indent}==========");

            // Рекурсивный вызов для вложенной папки
            await ProcessFolderAsync(clientApi, $"{path}{item.Name}/", indent + "  ");
        }
        else
        {
            Console.WriteLine($"{item.Name,-25}\t\t{item.Type,-10}\t{item.MimeType,-10}\t\t{item.Created:yyyy-MM-dd}");
        }
    }
}
    await ProcessFolderAsync(clientApi, "/");  // Начинаем с корневой папки

    Console.ReadLine();
//<идентификатор приложения>
//https://oauth.yandex.ru/authorize?response_type=token&client_id=6c11118efd2848a6abe1918ad4733715

// http://localhost:8080/callback#access_token=y0__xDrtJkFGLijNiCk6JPNEvRjUsH5le-oyOMziushmDklJQl0&token_type=bearer&expires_in=31138121&cid=7n269fukt8jg2gq9rnddgun1cw