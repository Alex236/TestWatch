using System.Collections.Generic;
namespace Watch.Services.ColorsRepository
{
    public class ColorsRepository : IColorsRepository
    {
        public Dictionary<string, string> ColorAndHex { get; set; } = new Dictionary<string, string>
        {
            {"Yellow", "#f1f442"},
            {"Blue", "#5d9afc"},
            {"White", "#ffffff"},
            {"Green", "#6dff6d"}
        };
    }
}
