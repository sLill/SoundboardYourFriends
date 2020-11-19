using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace SoundboardYourFriends.Model.JsonConverters
{
    public class AudioOutputDeviceJsonConverter : JsonConverter<IEnumerable<AudioOutputDevice>>
    {
        #region Methods..
        #region WriteJson
        public override void WriteJson(JsonWriter writer, [AllowNull] IEnumerable<AudioOutputDevice> value, JsonSerializer serializer)
        {
            try
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
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }
        }
        #endregion WriteJson

        #region ReadJson
        public override IEnumerable<AudioOutputDevice> ReadJson(JsonReader reader, Type objectType, [AllowNull] IEnumerable<AudioOutputDevice> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var deviceCollection = new List<AudioOutputDevice>();

            try
            {
                if (reader.TokenType != JsonToken.None)
                {
                    while (reader.TokenType != JsonToken.EndArray)
                    {
                        if (reader.Value?.ToString() == "DeviceId")
                        {
                            reader.Read();

                            // DeviceId
                            var deviceId = Guid.Parse((string)reader.Value);
                            reader.Read();

                            // PlaybackScope
                            reader.Read();
                            var playbackScope = (PlaybackScope)Enum.Parse(typeof(PlaybackScope), (string)reader.Value);

                            deviceCollection.Add(new AudioOutputDevice(deviceId) { PlaybackScope = playbackScope, DeviceActive = true });
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }

                existingValue = deviceCollection;
            }
            catch (Exception ex)
            {
                ApplicationLogger.Log(ex.Message, ex.StackTrace);
            }

            return existingValue;
        }
        #endregion ReadJson
        #endregion Methods..
    }
}
