using Aoo.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

public class YamlConfigurationProvider 
    : FileConfigurationProvider
{
    private readonly IYamlConfigurationParser _parser;
    
    public YamlConfigurationProvider(YamlConfigurationSource source)
        : base(source)
    {
        _parser = source.Parser ?? throw new Exception();
    }

    public override void Load(Stream stream)
    {
        // parse yaml file
        Data = _parser.Parse(stream);
    }
}