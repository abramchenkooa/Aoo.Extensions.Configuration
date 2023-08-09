namespace Aoo.Extensions.Configuration;

public interface IYamlConfigurationParser
{
    IDictionary<string, string> Parse(Stream input);
}