using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using IoT.DevaceListener.Interfaces;
using IoT.DevaceListener.Iterfaces.Context;
using Newtonsoft.Json;
using MongoDB.Bson;
using IoT.Common.Models.Device;

namespace IoT.DevaceListener.Repositories
{
    public class DeviceRepository :IRepository<Device>
    {
        private readonly IDeviceContext<Device> _context;
        private readonly IMongoCollection<BsonDocument> _deviceCollection;

        public DeviceRepository(IDeviceContext<Device> context)
        {
            _context = context;
            _deviceCollection = _context.Devices.Database.GetCollection<BsonDocument>("Device");
        }

        public async Task<bool> CreateAsync(Device model)
        {
            try
            {
                await _context.Devices.InsertOneAsync(model)
             .ConfigureAwait(false);
            }
            catch
            {
                return false;
            }
         
            return true;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        private object obj = new object();
        public async Task<IList<string>> GetListDevaces(IList<Guid> devaceSerialNumbers)
        {
       
            // var result = await _context.Devices.FindAsync(_ => _.SerialNumber );
            //var filter = Builders<BsonDocument>.Filter;
            //BsonElement bsonElement = new BsonElement("_id", devaceSerialNumbers[0]);
            //var filterDefinition = filter.In(_ => _.GetValue("_id")., bsonElement);
            //var cursor = await _deviceCollection.FindAsync(filterDefinition);
           
            var devices = new List<string>();
            BsonDocument filter = new BsonDocument();

            var cursor = await _deviceCollection.FindAsync(filter);
            await cursor.ForEachAsync(bsonDocument =>
            {
                Parallel.ForEach(devaceSerialNumbers, item =>
                {
                    var bsonDocumentId = bsonDocument.GetElement("_id").Value.AsGuid;
                    if (bsonDocumentId == item)
                    {
                        string json = JsonConvert.SerializeObject(BsonTypeMapper.MapToDotNetValue(bsonDocument))
                            .Replace("_", "");
                        devices.Add(json);
                    }
                });
            });

            return devices;
        }

        public async Task<Device> GetAsync(Guid Id)
        {
            return await _context.Devices.FindAsync(_ => _.SerialNumber == Id)
                .Result
                .FirstOrDefaultAsync();
        }
 
        public async Task<bool> IsExist(Guid id)
        {
            var result = await _context.Devices.FindAsync(_ => _.SerialNumber == id)
                 .Result
                 .FirstOrDefaultAsync()
                 .ConfigureAwait(false);

            if (result == null)
                return false;

            return true;
        }

        public async Task<bool> UpdateAsync(Device model)
        {

            var updateOption = new UpdateOptions
            {
                IsUpsert = true
            };
            var result = await _context.Devices.ReplaceOneAsync(_ => _.SerialNumber == model.SerialNumber, model, updateOption);

            return result.IsAcknowledged;
        }

        public async Task UpdateStatus(string mqttClient, string status)
        {
            try
            {
                var updateOption = new UpdateOptions
                {
                    IsUpsert = false
                };
                
                BsonElement bsonElement = new BsonElement("Status", status);
                var newDocument = new BsonDocument { { "$set", new BsonDocument (bsonElement)} };

                await _context.Devices.UpdateManyAsync(_ => _.MqttClient == mqttClient, newDocument, updateOption);
            }
            catch {

                throw;

            }

            await Task.CompletedTask;
        }
    }
}
