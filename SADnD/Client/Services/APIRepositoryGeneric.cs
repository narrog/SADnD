﻿using SADnD.Shared.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SADnD.Client.Services
{
    public class APIRepositoryGeneric<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        string _controllerName;
        // string _primaryKeyName;
        HttpClient _httpClient;

        public APIRepositoryGeneric(HttpClient httpClient, string controllerName)
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
                var response = JsonSerializer.Deserialize<APIListOfEntityResponse<TEntity>>(responseBody);
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
                var response = JsonSerializer.Deserialize<APIEntityResponse<TEntity>>(responseBody);
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
                var response = JsonSerializer.Deserialize<APIEntityResponse<TEntity>>(responseBody);
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
                var response = JsonSerializer.Deserialize<APIEntityResponse<TEntity>>(responseBody);
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
            try
            {
                var url = _controllerName + "/" + WebUtility.HtmlEncode(id.ToString());
                var result = await _httpClient.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
