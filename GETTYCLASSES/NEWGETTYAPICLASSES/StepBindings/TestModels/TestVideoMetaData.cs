using Newtonsoft.Json;

namespace NewGettyAPIclasses.StepBindings.TestModels
{
    [JsonObject]
    public class TestVideoMetaData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}