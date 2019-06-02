using System.Collections.Generic;


namespace Watch.Services.ColorsRepository
{
    public interface IColorsRepository
    {
        Dictionary<string, string> ColorAndHex { get; set; }
    }
}
