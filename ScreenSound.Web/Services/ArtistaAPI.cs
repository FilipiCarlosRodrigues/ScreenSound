using ScreenSound.Web.Requests;
using ScreenSound.Web.Response;
using System.Net.Http.Json;

namespace ScreenSound.Web.Services
{
    public class ArtistaAPI
    {
        private readonly HttpClient _httpClient;

        public ArtistaAPI(IHttpClientFactory factory) //baixei um pacote nugget para poder usar 
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ICollection<ArtistaResponse>?> GetArtistasAsync()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<ArtistaResponse>>("artistas");
        }

        public async Task AddArtistaAsync(ArtistaRequest artista)
        {
            await _httpClient.PostAsJsonAsync("artistas", artista);
        }

        public async Task DeleteArtistaAsync(int Id)
        {
            await _httpClient.DeleteAsync($"artistas/{Id}");
        }

        public async Task<ArtistaResponse?> GetArtistaPorNomeAsync(string nome)
        {
            return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"Artistas/{nome}");
        }

        public async Task<ArtistaResponse?> GetArtistaPorIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ArtistaResponse>($"artistasid/{id}");
        }

        public async Task UpdateArtistaAsync(ArtistaRequestEdit artista)
        {
            await _httpClient.PutAsJsonAsync("artistas", artista);
        }
    }
}
