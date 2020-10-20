using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace SoundboardYourFriends.Model.JsonConverters
{
    public class AudioOutputDeviceJsonConverter : JsonConverter<IEnumerable<AudioOutputDevice>>
    {
        public override void WriteJson(JsonWriter writer, [AllowNull] IEnumerable<AudioOutputDevice> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            value?.ToList().ForEach(device =>
            {
                writer.WriteStartObject();

                // Device Id
                writer.WritePropertyName("DeviceId");
                writer.WriteValue(device.DeviceId);

                // Playback Scope
                writer.WritePropertyName("PlaybackScope");
                writer.WriteValue(device.PlaybackScope.ToString());

                writer.WriteEndObject();
            });

            writer.WriteEndArray();
        }

        public override IEnumerable<AudioOutputDevice> ReadJson(JsonReader reader, Type objectType, [AllowNull] IEnumerable<AudioOutputDevice> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var deviceCollection = new List<AudioOutputDevice>();

                while (reader.TokenType != JsonToken.EndArray)
                {
                    if ((string)reader.Value == "DeviceId")
                    {
                        reader.Read();

                        // DeviceId
                        var deviceId = Guid.Parse((string)reader.Value);
                        reader.Read();

                        // PlaybackScope
                        reader.Read();
                        var playbackScope = (PlaybackScope)Enum.Parse(typeof(PlaybackScope), (string)reader.Value);

                        deviceCollection.Add(new AudioOutputDevice(deviceId) { PlaybackScope = playbackScope });
                    }
                    else
                    {
                        reader.Read();
                    }
                }

            existingValue = deviceCollection;
            return existingValue;
        }
    }
}
