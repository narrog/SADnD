using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SADnD.Client.Shared;
using SADnD.Shared;
using SADnD.Shared.Models;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SADnD.Client.Services
{
    public class NoteApiManager : APIRepository<Note>
    {
        string _controllerName;
        HttpClient _httpClient;
        public NoteApiManager(HttpClient httpClient)
            : base(httpClient,"note")
        {
            _httpClient = httpClient;
            _controllerName = typeof(Note).Name;
        }

        private Dictionary<string, Type> typeMapping = new Dictionary<string, Type>()
        {
            {"NoteStory",typeof(NoteStory)},
            {"NotePerson",typeof(NotePerson)},
            {"NoteLocation",typeof(NoteLocation)},
            {"NoteQuest",typeof(NoteQuest)},
            {"NoteHint",typeof(NoteHint)}
        };
        public override async Task<IEnumerable<Note>> GetAll()
        {
            try
            {
                var result = await _httpClient.GetAsync(_controllerName);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIListOfEntityResponse<JObject>>(responseBody);
                if (response.Success)
                {
                    var notes = new List<Note>();
                    foreach (var item in response.Data)
                    {
                        notes.Add((Note)item.ToObject(typeMapping[item["type"]?.ToString()]));
                    }
                    return notes;
                }
                else
                {
                    return new List<Note>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        public override async Task<Note> GetByID(object id)
        {
            try
            {
                var arg = WebUtility.HtmlEncode(id.ToString());
                var url = _controllerName + "/" + arg;
                var result = await _httpClient.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<JObject>>(responseBody);
                if (response.Success)
                {
                    return (Note)response.Data.ToObject(typeMapping[response.Data["type"]?.ToString()]);
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
        public override async Task<Note> Insert(Note entity)
        {
            try
            {
                Console.WriteLine($"Insert {JsonConvert.SerializeObject(entity)}");
                var result = await _httpClient.PostAsync(_controllerName, new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json"));
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<JObject>>(responseBody);
                if (response.Success)
                {
                    return (Note)response.Data.ToObject(typeMapping[response.Data["type"]?.ToString()]);
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
        public override async Task<Note> Update(Note entity)
        {
            try
            {
                var result = await _httpClient.PutAsync(_controllerName, new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json"));
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<APIEntityResponse<JObject>>(responseBody);
                if (response.Success)
                {
                    return (Note)response.Data.ToObject(typeMapping[response.Data["type"]?.ToString()]);
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
        public override async Task<bool> Delete(Note entity)
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
        public override async Task<bool> Delete(object id)
        {
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
