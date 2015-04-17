using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoL_URF_HybridStats_vs12.Model;

namespace LoL_URF_HybridStats_vs12.Helper
{
    public static class Serializator
    {
        public static void SaveData(ChampionsProcessor processor)
        {
            Serializer serializer = new Serializer();
            serializer.SerializeObject("savedData.dat", processor);
        }

        public static ChampionsProcessor LoadData()
        {
            Serializer serializer = new Serializer();
            ChampionsProcessor processor = serializer.DeSerializeObject("savedData.dat");
            return processor;
        }

        public static void SaveData(ChampionsProcessor processor, string newName)
        {
            Serializer serializer = new Serializer();
            serializer.SerializeObject(newName, processor);
        }

        public static ChampionsProcessor LoadData(string fileName)
        {
            Serializer serializer = new Serializer();
            ChampionsProcessor processor = serializer.DeSerializeObject(fileName);
            return processor;
        }
    }
}
