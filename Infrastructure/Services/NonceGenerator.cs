using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal sealed class NonceGenerator : INonceGenerator
    {
        public static Dictionary<string, string> nonces = new Dictionary<string, string>();

        public void CreateNonce(string nonceName)
        {
            string nonceValue = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            nonces.Add(nonceName, nonceValue);
        }

        public string GetNonce(string nonceName)
        {
            if (!nonces.ContainsKey(nonceName))
            {
                CreateNonce(nonceName);
            }

            return nonces[nonceName];
        }
    }
}
