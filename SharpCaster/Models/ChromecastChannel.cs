﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SharpCaster.Channels;

namespace SharpCaster.Models
{
    public abstract class ChromecastChannel : IChromecastChannel
    {
        private ChromeCastClient Client { get; set; }
        public string Namespace { get; set; }

        public event EventHandler<ChromecastSSLClientDataReceivedArgs> MessageReceived;

        protected ChromecastChannel(ChromeCastClient client, string @ns)
        {
            Namespace = ns;
            Client = client;
        }

        public async Task Write(CastMessage message)
        {
            Debug.WriteLine("Sending: " + message.GetJsonType());
            message.Namespace = Namespace;

            var bytes = message.ToProto();
            await Client.Write(bytes);
        }

        public void OnMessageReceived(ChromecastSSLClientDataReceivedArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
