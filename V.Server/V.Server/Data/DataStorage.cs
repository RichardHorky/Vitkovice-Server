using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace V.Server.Data
{
    public class DataStorage
    {
        private readonly Errors _errors;

        public DataStorage(Errors errors)
        {
            _errors = errors;
            var fnFileName = GetFileName<TransferData.FnItems>();
            if (!File.Exists(fnFileName))
                SaveData(new TransferData.FnItems() { Date = DateTime.MinValue });
        }

        public void SaveData<T>(T data)
        {
            try
            {
                var fileName = GetFileName<T>();
                if (File.Exists(fileName))
                    File.Delete(fileName);
                var strData = JsonConvert.SerializeObject(data);
                File.WriteAllText(GetFileName<T>(), strData);
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
        }

        public T GetData<T>()
        {
            try
            {
                var fileName = GetFileName<T>();
                if (!File.Exists(fileName))
                    return default(T);
                var strData = File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<T>(strData);
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
            return default(T);
        }

        private string GetFileName<T>()
        {
            try
            {
                if (!Directory.Exists($"./dat"))
                    Directory.CreateDirectory($"./dat");
                return $"./dat/{typeof(T).Name}.dat";
            }
            catch (Exception ex)
            {
                _errors.ErrorList.Add(new ErrorModel(ex.ToString()));
            }
            return null;
        }
    }
}
