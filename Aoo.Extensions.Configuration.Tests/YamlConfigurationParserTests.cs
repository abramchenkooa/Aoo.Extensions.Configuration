namespace Aoo.Extensions.Configuration.Tests;

[TestFixture]
public class YamlConfigurationParserTests
{
    private YamlConfigurationParser _parser;
    
    [SetUp]
    public void Setup()
    {
        _parser = new YamlConfigurationParser();
    }

    [Test]
    public void Parse_WhenValidYaml_ReturnsDictionary()
    {
        // Arrange
        var yaml = @"
            key1: value1
            key2: value2
            key3: value3
        ";

        // Act
        using var stream = WriteStringToStream(yaml);
        var result = _parser.Parse(stream);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result["key1"], Is.EqualTo("value1"));
            Assert.That(result["key2"], Is.EqualTo("value2"));
            Assert.That(result["key3"], Is.EqualTo("value3"));
        });
    }

    [Test]
    public void Parse_WhenValidYamlWithNestedMapping_ReturnsDictionary()
    {
        // Arrange
        var yaml = @"
            key1: value1
            key2: 
                key3: value3
                key4: value4
            key5: value5
        ";

        // Act
        using var stream = WriteStringToStream(yaml);
        var result = _parser.Parse(stream);
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result["key1"], Is.EqualTo("value1"));
            Assert.That(result["key2:key3"], Is.EqualTo("value3"));
            Assert.That(result["key2:key4"], Is.EqualTo("value4"));
            Assert.That(result["key5"], Is.EqualTo("value5"));
        });
    }
    
    [Test]
    public void Parse_WhenValidYamlWithNestedSequence_ReturnsDictionary()
    {
        // Arrange
        var yaml = @"
            key1: value1
            key2: 
                - value2
                - value3
                - value4
            key5: value5
        ";

        // Act
        using var stream = WriteStringToStream(yaml);
        var result = _parser.Parse(stream);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result["key1"], Is.EqualTo("value1"));
            Assert.That(result["key2:0"], Is.EqualTo("value2"));
            Assert.That(result["key2:1"], Is.EqualTo("value3"));
            Assert.That(result["key2:2"], Is.EqualTo("value4"));
            Assert.That(result["key5"], Is.EqualTo("value5"));
        });
    }
    
    [Test]
    public void Parse_WhenInvalidYaml_ThrowsException()
    {
        // Arrange
        var yaml = @"
            key1: value1
            key2: 
                key3 = value3
                key4: value4
            key5: value5
        ";

        // Act
        using var stream = WriteStringToStream(yaml);
        // Assert
        Assert.Throws<Exception>(() => _parser.Parse(stream));
    }
    

    private Stream WriteStringToStream(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}