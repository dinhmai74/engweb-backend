using AutoMapper;
using HollypocketBackend.Models;
using HollypocketBackend.Models.Provider;
using HollypocketBackend.Utils;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HollypocketBackend.Services
{
    public interface IProviderService
    {
        Task<List<Provider>> GetAll();
        Task<Provider> GetById(string id);
        Task<Provider> Create(CreateProviderModel provider);
        Task<Provider> Delete(string id);
        Task<bool> IsExisting(string id);
        Task PatchUpdate(string id, JsonPatchDocument<UpdateProviderModel> patchDoc);
        Task<UpdateProviderModel> Update(string id, UpdateProviderModel p);
    }
    public class ProviderService : IProviderService
    {
        private readonly IMongoCollection<Provider> _providers;
        private IMapper _mapper;
        public ProviderService(AppSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var datatbase = client.GetDatabase(settings.DatabaseName);

            _providers = datatbase.GetCollection<Provider>(settings.ProviderCollectionName);
            _mapper = mapper;
        }

        public async Task<List<Provider>> GetAll()
        {
            return await _providers.Find(p => true).ToListAsync();
        }

        public async Task<Provider> GetById(string id)
        {
            return await _providers.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Provider> Create(CreateProviderModel provider)
        {
            var p = _mapper.Map<Provider>(provider);
            await _providers.InsertOneAsync(p);
            return p;
        }

        public async Task<Provider> Delete(string id)
        {
            var provider = await GetById(id);
            await _providers.DeleteOneAsync(p => p.Id == id);
            return provider;
        }

        public async Task<bool> IsExisting(string id)
        {
            var provider = await GetById(id);
            if (provider != null)
                return true;
            return false;
        }

        public async Task<UpdateProviderModel> Update(string id, UpdateProviderModel p)
        {
            var provider = await GetById(id);
            _mapper.Map(p, provider);
            await _providers.ReplaceOneAsync(p => p.Id == id, provider);
            return p;
        }

        public async Task PatchUpdate(string id, JsonPatchDocument<UpdateProviderModel> patchDoc)
        {
            var provider = await GetById(id);

            var model = _mapper.Map<UpdateProviderModel>(provider);
            patchDoc.ApplyTo(model);

            await Update(id, model);
        }
    }
}
