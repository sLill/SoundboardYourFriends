﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace SoundboardYourFriends.Model.JsonConverters
{
    public class AudioCaptureDeviceJsonConverter : JsonConverter<IEnumerable<AudioCaptureDevice>>
    {
        public override void WriteJson(JsonWriter writer, [AllowNull] IEnumerable<AudioCaptureDevice> value, JsonSerializer serializer)
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

        public override IEnumerable<AudioCaptureDevice> ReadJson(JsonReader reader, Type objectType, [AllowNull] IEnumerable<AudioCaptureDevice> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var deviceCollection = new List<AudioCaptureDevice>();

                while (reader.TokenType != JsonToken.EndArray)
                {
                    if ((string)reader.Value == "DeviceId")
                    {
                        reader.Read();

                        // DeviceId
                        var deviceId = Guid.Parse((string)reader.Value);
                        deviceCollection.Add(new AudioCaptureDevice(deviceId));
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
