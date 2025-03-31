using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using YandexDisk.Client;
using YandexDisk.Client.Http;
using YandexDisk.Client.Protocol;

namespace WForms_App_Filemanager
{
    public class YandexDiskClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public YandexDiskClient(string token)
        {
            _httpClient = new HttpClient();
            _token = token;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"OAuth {_token}");
        }

        // Метод для получения информации о диске
        public async Task<string> GetDiskInfoAsync()
        {

            try
            {
                // Отправляем GET-запрос к корневому пути Яндекс.Диска
                var response = await _httpClient.GetAsync("https://cloud-api.yandex.net/v1/disk/");

                // Проверяем, что запрос выполнен успешно (HTTP 200-299)
                response.EnsureSuccessStatusCode();

                // Читаем ответ в формате JSON
                string responseBody = await response.Content.ReadAsStringAsync();

                // Парсим JSON для удобного извлечения данных
                var diskInfo = JObject.Parse(responseBody);

                // Форматируем информацию в читаемый вид
                string result = $"📁 Общий объем: {diskInfo["total_space"]} байт\n" +
                                $"💾 Использовано: {diskInfo["used_space"]} байт\n" +
                                $"🆓 Свободно: {diskInfo["free_space"]} байт";

                return result;
            }
            catch (HttpRequestException ex)
            {
                return $"❌ Ошибка при запросе к Яндекс.Диску: {ex.Message}";
            }
        }


        // Метод для получения списка файлов и папок
        public async Task<JObject> GetFilesAsync(string path)
        {
            path = path.ToLower();
            try
            {

                var response = await _httpClient.GetAsync($"https://cloud-api.yandex.net/v1/disk/resources?path={Uri.EscapeDataString(path)}");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content);
            }
            catch (Exception ex)
            {
                return JObject.Parse(ex.Message);
            }
        }
    }
}
