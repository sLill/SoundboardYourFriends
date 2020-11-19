using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SoundboardYourFriends.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace SoundboardYourFriends.Model.JsonConverters
{
    public class AudioCaptureDeviceJsonConverter : JsonConverter<IEnumerable<AudioCaptureDevice>>
    {
        #region Methods..
        #region WriteJson
        public override void WriteJson(JsonWriter writer, [AllowNull] IEnumerable<AudioCaptureDevice> value, JsonSerializer serializer)
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
        public override IEnumerable<AudioCaptureDevice> ReadJson(JsonReader reader, Type objectType, [AllowNull] IEnumerable<AudioCaptureDevice> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var deviceCollection = new List<AudioCaptureDevice>();

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
                            deviceCollection.Add(new AudioCaptureDevice(deviceId) { DeviceActive = true }); ;
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
