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
        public void SaveData<T>(T data)
        {
            var fileName = GetFileName<T>();
            if (File.Exists(fileName))
                File.Delete(fileName);
            var strData = JsonConvert.SerializeObject(data);
            File.WriteAllText(GetFileName<T>(), strData);
        }

        public T GetData<T>()
        {
            var fileName = GetFileName<T>();
            if (!File.Exists(fileName))
                return default(T);
            var strData = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(strData);
        }

        private string GetFileName<T>()
        {
            if (!Directory.Exists($"./dat"))
                Directory.CreateDirectory($"./dat");
            return $"./dat/{typeof(T).Name}.dat";
        }
    }
}
