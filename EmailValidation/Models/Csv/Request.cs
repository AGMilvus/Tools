namespace EmailValidation.Models.Csv;

public class Request
{
    private readonly string _basePath;
    
    public List<Entity> Records { get; set;}
    
    public Request(string  basePath)
    {
        _basePath = basePath;
    }
    
    public Request Load()
    {
        Records = CsvFileService.Load<Entity>(_basePath);
        return this;
    }
}