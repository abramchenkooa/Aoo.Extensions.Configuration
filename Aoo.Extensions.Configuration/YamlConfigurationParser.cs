using YamlDotNet.RepresentationModel;

namespace Aoo.Extensions.Configuration;

public class YamlConfigurationParser 
    : IYamlConfigurationParser
{
    // Parses configuration from a yaml file stream into a key/value dictionary. 
    // The default implementation uses YamlDotNet.
    public IDictionary<string, string> Parse(Stream input)
    {         
        var yaml = new YamlStream();
        yaml.Load(new StreamReader(input));
        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        return ParseMapping(mapping);
    }

    private IDictionary<string, string> ParseMapping(YamlMappingNode mapping)
    {
        var result = new Dictionary<string, string>();
        foreach (var entry in mapping.Children)
        {
            var key = ((YamlScalarNode)entry.Key).Value;
            var value = ParseNode(entry.Value);
            result.Add(key, value);
        }
        return result;
    }

    private string ParseNode(YamlNode entryValue)
    {
        switch (entryValue)
        {
            case YamlScalarNode scalarNode:
                return scalarNode.Value;
            case YamlMappingNode mappingNode:
                return ParseMapping(mappingNode).ToString();
            case YamlSequenceNode sequenceNode:
                return ParseSequence(sequenceNode).ToString();
            default:
                throw new Exception();
        }
    }

    private object ParseSequence(YamlSequenceNode sequenceNode)
    {
        var result = new List<string>();
        foreach (var entry in sequenceNode.Children)
        {
            result.Add(ParseNode(entry));
        }
        return result;
    }
}