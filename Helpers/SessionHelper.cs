using Microsoft.AspNetCore.Http;
using ReversiMvcApp.Models.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Helpers
{
    public static class SessionHelper
    {
        /// <summary>
        /// Session extension for serializing StateModel objects
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObjectAsBytes(this ISession session, string key, StateModel value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value.Guid);
                    writer.Write(value.State);
                }
                session.Set(key, stream.ToArray());
            }
        }

        /// <summary>
        /// Session extension for retrieving/deserializing StateModel objects
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static StateModel GetObjectFromBytes(this ISession session, string key)
        {
            StateModel model = new StateModel();

            byte[] data = session.Get(key);

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    model.Guid = reader.ReadString();
                    model.State = reader.ReadInt32();
                }
            }

            return model;
        }


    }
}
