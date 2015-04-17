using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LoL_URF_HybridStats_vs12.Model
{
    public class Serializer
    {
        public Serializer()
        {
        }

        public void SerializeObject(string filename, ChampionsProcessor objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        public ChampionsProcessor DeSerializeObject(string filename)
        {
            ChampionsProcessor objectToSerialize;
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            objectToSerialize = (ChampionsProcessor)bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}
