using YandexDisk.Client;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

Console.WriteLine("Server is Run");

//async Task AddFiles(IDiskApi api) // добавление и создание папки
//{
//    const string fileFolder = "NewFolder_JPG";
//    var newfolder = await api.MetaInfo.GetInfoAsync(new ResourceRequest
//    {
//        Path = "/",
//    });

//    if (!newfolder.Embedded.Items.Any(i => i.Type == ResourceType.Dir && i.Name.Equals(fileFolder)))
//    {
//        await api.Commands.CreateDictionaryAsync("/" + fileFolder);
//    }
//    if (Directory.Exists(fileFolder))
//    {
//        var files = Directory.GetFiles(Environment.CurrentDirectory, "*.jpg"); // добавление
//        foreach (var file in files)
//        {
//            var link = await api.Files.GetUploadLinkAsync("/" + fileFolder + "/" + Path.GetFileName(file), overwrite: false); // без перезаписи
//            Console.WriteLine(link);
//            using (var fileStream = File.OpenRead(file))
//            {
//                await api.Files.UploadAsync(link, fileStream);
//            }
//        }
//    }
//}


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


var clientApi = new DiskHttpApi("y0__xDrtJkFGLijNiCk6JPNEvRjUsH5le-oyOMziushmDklJQl0");

async Task ProcessFolderAsync(IDiskApi clientApi, string path, string indent = "")
{
    try
    {

        var folder = await clientApi.MetaInfo.GetInfoAsync(new ResourceRequest
        {
            Path = path
        });

        foreach (var item in folder.Embedded.Items)
        {
            if (item.Type == ResourceType.File)
            {
                Console.WriteLine($"{item.Name,-25}\t\t{item.Type,-10}\t{item.MimeType,-10}\t\t{item.Created:yyyy-MM-dd}");
            }

            // if (string.Equals(item.Type.ToString(), "dir", StringComparison.OrdinalIgnoreCase) ||
            //     string.Equals(item.Type.ToString(), "folder", StringComparison.OrdinalIgnoreCase))
            else if (item.Type == ResourceType.Dir)
            {
                Console.WriteLine($"{indent}==========");
                Console.WriteLine($"{indent}Папка: {item.Name}");
                Console.WriteLine($"{indent}==========");

                // Рекурсивный вызов для вложенной папки
                await ProcessFolderAsync(clientApi, $"{path}{item.Name}/", indent + "  ");
            }
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

}

await ProcessFolderAsync(clientApi, "/");  // Начинаем с корневой папки

//await AddFiles(clientApi); только чтение
Console.ReadLine();

//<идентификатор приложения>
//https://oauth.yandex.ru/authorize?response_type=token&client_id=6c11118efd2848a6abe1918ad4733715

// http://localhost:8080/callback#access_token=y0__xDrtJkFGLijNiCk6JPNEvRjUsH5le-oyOMziushmDklJQl0&token_type=bearer&expires_in=31138121&cid=7n269fukt8jg2gq9rnddgun1cw