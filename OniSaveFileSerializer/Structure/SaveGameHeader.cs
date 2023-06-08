using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace OniSaveFileSerializer.Structure
{
    public sealed class SaveGameHeader
    {
        public uint BuildVersion { get; set; }

        public uint InfoSize { get; set; }

        public uint HeaderVersion { get; set; }

        public bool Compressed { get; set; }

        public SaveGameInfo? Info { get; set; }

        public sealed class SaveGameInfo
        {
            [JsonPropertyName("numberOfCycles")]
            public uint NumberOfCycles { get; set; }

            [JsonPropertyName("numberOfDuplicants")]
            public uint NumberOfDuplicants { get; set; }

            [JsonPropertyName("baseName")]
            public string BaseName { get; set; } = string.Empty;

            [JsonPropertyName("isAutoSave")]
            public bool AutoSave { get; set; }

            [JsonPropertyName("originalSaveName")]
            public string OriginalSaveName { get; set; } = string.Empty;

            [JsonPropertyName("saveMajorVersion")]
            public uint SaveMajorVersion { get; set; }

            [JsonPropertyName("saveMinorVersion")]
            public uint SaveMinorVersion { get; set; }

            [JsonPropertyName("clusterId")]
            public string ClusterId { get; set; } = string.Empty;

            [JsonPropertyName("worldTraits")]
            public JsonObject? WorldTraits { get; set; }

            [JsonPropertyName("sandboxEnabled")]
            public bool SandBoxEnabled { get; set; }

            [JsonPropertyName("colonyGuid")]
            public string ColonyGuid { get; set; } = string.Empty;

            [JsonPropertyName("dlcId")]
            public string DlcId { get; set; } = string.Empty;
        }
    }
}
