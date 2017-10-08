namespace Sharparam.StreamChatSync.Mixer.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ChatClient
    {
        private const int BufferSize = 1024;

        private readonly ILogger _log;

        private readonly string _url;

        private readonly string _authKey;

        private readonly User _user;

        private readonly ClientWebSocket _webSocket;

        private ArraySegment<byte> _buffer = new ArraySegment<byte>(new byte[BufferSize]);

        private long _messageRequestId = 1000;

        public ChatClient(ILogger<ChatClient> logger, string url, string authKey, User user)
        {
            _log = logger;
            _url = url;
            _authKey = authKey;
            _user = user;
            _webSocket = new ClientWebSocket();
        }

        public event EventHandler<MixerMessage> MessageReceived; 

        public async Task ConnectAsync()
        {
            await _webSocket.ConnectAsync(new Uri(_url), CancellationToken.None).ContinueWith(Connected);
        }

        public async Task DisconnectAsync()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            _webSocket.Dispose();
        }

        public async Task SendAsync(string message)
        {
            var msgRequest = new Method(_messageRequestId++, "msg", message);
            var msg = JsonConvert.SerializeObject(msgRequest);
            var bytes = Encoding.UTF8.GetBytes(msg);
            var segment = new ArraySegment<byte>(bytes);

            _log.LogDebug("Sending {Message} to web socket", msg);
            await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task Connected(Task task)
        {
            _log.LogInformation("Connected to Mixer chat services");

            _log.LogDebug("Calling receive asynchronously");
            await _webSocket.ReceiveAsync(_buffer, CancellationToken.None)
                .ContinueWith(ReceiveHandler);

            _log.LogDebug("Calling Auth");
            await Auth();
        }

        private async Task ReceiveHandler(Task<WebSocketReceiveResult> task)
        {
            var result = task.Result;

            _log.LogDebug("Received WebSocket result");

            var message = Encoding.UTF8.GetString(_buffer.Array);

            _log.LogDebug("Message: {Message}", message);

            var parsed = JObject.Parse(message);

            var type = parsed.GetValue("type").ToString();

            switch (type)
            {
                case "reply":
                    var reply = JsonConvert.DeserializeObject<Reply>(message, new MixerJsonConverter());
                    HandleReply(reply);
                    break;

                case "event":
                    var @event = JsonConvert.DeserializeObject<Event>(message, new MixerJsonConverter());
                    HandleEvent(@event);
                    break;
            }

            _buffer = new ArraySegment<byte>(new byte[BufferSize]);
            await _webSocket.ReceiveAsync(_buffer, CancellationToken.None).ContinueWith(ReceiveHandler);
        }

        private async Task Auth()
        {
            _log.LogInformation("Authenticating with chat API");
            var authRequest = new Method(0, "auth", _user.Channel.Id, _user.Id, _authKey);
            var message = JsonConvert.SerializeObject(authRequest);
            var bytes = Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(bytes);

            _log.LogDebug("Sending {Message} to web socket", message);
            await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private void HandleEvent(Event @event)
        {
            _log.LogDebug("Event: {@Event}", @event);

            if (@event.Name != "ChatMessage")
                return;

            var user = @event.Data["user_name"] as string;
            var channelId = @event.Data["channel"] as long?;

            var messageObject = @event.Data["message"] as Dictionary<string, object>;

            var messageList = messageObject["message"] as object[];

            var parts = new List<string>(messageList.Length);

            foreach (var part in messageList)
            {
                var pairs = part as Dictionary<string, object>;

                var text = pairs?["text"];

                if (text == null)
                    continue;

                parts.Add(text.ToString());
            }

            var message = string.Join(" ", parts);

            OnMessageReceived(user, channelId.Value, message);
        }

        private void HandleReply(Reply reply)
        {
            _log.LogDebug("Reply: {@Reply}", reply);
        }

        private void OnMessageReceived(string user, long channelId, string message)
        {
            MessageReceived?.Invoke(this, new MixerMessage(user, channelId.ToString(), message));
        }
    }
}
