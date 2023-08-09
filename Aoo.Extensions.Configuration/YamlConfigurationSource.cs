using Aoo.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

public class YamlConfigurationSource 
    : FileConfigurationSource
{
    public IYamlConfigurationParser? Parser { get; set; }
    
    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);
        
        Parser ??= new YamlConfigurationParser();
        
        return new YamlConfigurationProvider(this);
    }
}