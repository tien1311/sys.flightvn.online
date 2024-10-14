using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Manager.Model.Models.Other
{
    public class ConvertObjectToByte
    {
        public byte[] ObjectToByteArray(object obj)
        {
            try
            {
                byte[] bytes = null;
                BinaryFormatter bf = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    bytes = ms.ToArray();
                }
                return bytes;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        // Convert a byte array to an Object
        public object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;
        }
    }
}
