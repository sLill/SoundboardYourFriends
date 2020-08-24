using Newtonsoft.Json;
using System;
using System.IO;

namespace SoundboardYourFriends.Core.Config
{
    public class SettingsAgent : ISettingsReader
    {
        #region Member Variables..
        private readonly string _configurationFilePath;
        private readonly string _sectionNameSuffix;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new SettingsContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        #endregion Member Variables..

        #region Constructors..
        public SettingsAgent(string configurationFilePath, string sectionNameSuffix = "Settings")
        {
            _configurationFilePath = configurationFilePath;
            _sectionNameSuffix = sectionNameSuffix;
        }
        #endregion Constructors..

        #region Methods..
        #region Load
        public T Load<T>() where T : class, new() => Load(typeof(T)) as T;
        #endregion Load

        #region LoadSection
        public T LoadSection<T>() where T : class, new() => LoadSection(typeof(T)) as T;
        #endregion LoadSection

        #region Load
        public object Load(Type type)
        {
            if (!File.Exists(_configurationFilePath))
            {
                return Activator.CreateInstance(type);
            }

            var jsonFile = File.ReadAllText(_configurationFilePath);
            return JsonConvert.DeserializeObject(jsonFile, type, JsonSerializerSettings);
        }
        #endregion Load

        #region LoadSection
        public object LoadSection(Type type)
        {
            if (!File.Exists(_configurationFilePath))
            {
                return Activator.CreateInstance(type);
            }

            var jsonFile = File.ReadAllText(_configurationFilePath);
            var section = type.Name.Replace(_sectionNameSuffix, string.Empty).ToCamelCase();
            var settingsData = JsonConvert.DeserializeObject<dynamic>(jsonFile, JsonSerializerSettings);
            var settingsSection = settingsData[section];

            return settingsSection == null ? Activator.CreateInstance(type) : JsonConvert.DeserializeObject(JsonConvert.SerializeObject(settingsSection), type, JsonSerializerSettings);
        }
        #endregion LoadSection
        #endregion Methods..
    }
}
