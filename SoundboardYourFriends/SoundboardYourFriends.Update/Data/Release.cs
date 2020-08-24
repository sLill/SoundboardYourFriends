using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SoundboardYourFriends.Update.Data
{
    public class Release : IComparable
    {
        #region Properties..
        [JsonProperty("assets")]
        public List<Asset> Assets { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; }

        public Version Version => new Version(TagName);
        #endregion Properties..

        #region Methods..
        #region CompareTo
        public int CompareTo(object obj)
        {
            return this.Version > ((Release)obj).Version ? 1 : 0;
        }
        #endregion CompareTo
        #endregion Methods..
    }
}
