using Newtonsoft.Json;
using SADnD.Shared;
using System.Net;
using System.Net.Http.Json;

namespace SADnD.Client.Shared
{
    public class APIRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        string _controllerName;
        // string _primaryKeyName;
        HttpClient _httpClient;

        public APIRepository(HttpClient httpClient, string controllerName)
        {
            _httpClient = httpClient;
            _controllerName = controllerName;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                var result = await _httpClient.GetAsync(_controllerName);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIListOfEntityResponse<TEntity>>(responseBody);
                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    return new List<TEntity>();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TEntity> GetByID(object id)
        {
            try
            {
                var arg = WebUtility.HtmlEncode(id.ToString());
                var url = _controllerName + "/" + arg;
                var result = await _httpClient.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<TEntity>>(responseBody);
                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TEntity> Insert(TEntity entity)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync(_controllerName, entity);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<TEntity>>(responseBody);
                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TEntity> Update(TEntity entity)
        {
            try
            {
                var result = await _httpClient.PutAsJsonAsync(_controllerName, entity);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<TEntity>>(responseBody);
                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> Delete(TEntity entity)
        {
            try
            {
                var value = entity.GetType()
                    .GetProperty("Id")
                    .GetValue(entity, null)
                    .ToString();

                var arg = WebUtility.HtmlEncode(value);
                var url = _controllerName + "/" + arg;
                var result = await _httpClient.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> Delete(object id)
        {
            Console.WriteLine("Funktion Delete in APIRepository aufgerufen");
            try
            {
                var url = _controllerName + "/" + WebUtility.HtmlEncode(id.ToString());
                Console.WriteLine($"url: {url}");
                var result = await _httpClient.DeleteAsync(url);
                Console.WriteLine($"result: {result}");
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
