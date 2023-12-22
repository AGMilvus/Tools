using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using EnrichMindboxContacts;
using EnrichMindboxContacts.Models;


Stopwatch sw = new Stopwatch();
sw.Start();


var mindboxContacts = CsvFileService.Load<MindboxContact>("csv/MindBoxContacts.csv", ";");
Console.WriteLine("MindBox Contacts loaded. {0}", sw.ElapsedMilliseconds);

var bpmSoftContacts = CsvFileService.Load<BpmSoftContact>("csv/BPMSoftContacts.csv");
Console.WriteLine("bpmSoft Contacts loaded. {0}", sw.ElapsedMilliseconds);

var phoneDict = new Dictionary<string, BpmSoftContact>();
var emailDict = new Dictionary<string, BpmSoftContact>();
foreach (var m in bpmSoftContacts)
{
    if(m.MobilePhone != string.Empty) phoneDict.TryAdd(m.MobilePhone, m);
    if(m.Email != string.Empty) emailDict.TryAdd(m.Email, m);
}
Console.WriteLine("MindBox Contacts hashed. {0}", sw.ElapsedMilliseconds);

var diff = new List<FullNameDiff> ();
var stat = MergeContacts(mindboxContacts, phoneDict, emailDict);

Console.WriteLine("Contacts merged. {0}", sw.ElapsedMilliseconds);

Console.WriteLine($"Found by:\n\t" +
                  $"Total: {mindboxContacts.Count}\n\t" +
                  $"Phone: {stat.FoundByPhone}\n\t" +
                  $"Email With Phone: {stat.FoundByEmailWithPhone}\n\t" +
                  $"Email Without Phone: {stat.FoundByEmailWithoutPhone}\n\t" +
                  $"Full Names differs: {stat.FullNameNotEquals}\n\t");

var idDuplicates = FindGuidDuplicates(mindboxContacts);
Console.WriteLine("Id Duplicates: {0}", idDuplicates.Count);

CsvFileService.Write("csv/nameDiff.csv", diff);
Console.WriteLine("nameDiff writen: {0}", sw.ElapsedMilliseconds);

CsvFileService.Write("csv/idDuplicates.csv", idDuplicates);
Console.WriteLine("idDuplicates writen: {0}", sw.ElapsedMilliseconds);

CsvFileService.Write("csv/output.csv", mindboxContacts);
Console.WriteLine("output writen: {0}", sw.ElapsedMilliseconds);

Console.WriteLine("End. {0}", sw.ElapsedMilliseconds);

Statistics MergeContacts(IEnumerable<MindboxContact> mindboxContacts, Dictionary<string, BpmSoftContact> phoneDict, Dictionary<string, BpmSoftContact> emailDict)
{
    var stat = new Statistics();

    foreach (var m in mindboxContacts)
    {
        bool foundByPhone = false, foundByEmail = false;
        BpmSoftContact? bpmSoftContact = null;
        if (!string.IsNullOrEmpty(m.Phone))
        {
            foundByPhone = phoneDict.TryGetValue(m.Phone, out bpmSoftContact);
        }

        if (!string.IsNullOrEmpty(m.Email) && !foundByPhone)
        {
            foundByEmail = emailDict.TryGetValue(m.Email, out bpmSoftContact);
        }

        if(foundByPhone) stat.FoundByPhone++;
        if(foundByEmail)
        {
            if(string.IsNullOrEmpty(m.Phone))
                stat.FoundByEmailWithoutPhone++;
            else
                stat.FoundByEmailWithPhone++;
        }

        if (bpmSoftContact is not null)
        {
            m.BpmId = bpmSoftContact.Id.ToString();
            if (!FullNameEquals(m, bpmSoftContact))
            {
                diff.Add(new FullNameDiff
                {
                    Id = bpmSoftContact.Id,
                    BpmFirstName = bpmSoftContact.GivenName,
                    BpmMiddleName = bpmSoftContact.MiddleName,
                    BpmLastName = bpmSoftContact.Surname,
                    MBFirstName = m.FirstName,
                    MBMiddleName = m.MiddleName,
                    MBLastName = m.LastName
                });
                stat.FullNameNotEquals++;
            }
        }
    }
    return stat;
}

bool FullNameEquals(MindboxContact m, BpmSoftContact b)
{
    return m.FirstName == b.Surname || m.MiddleName == b.Surname || m.LastName == b.Surname
        && m.FirstName == b.MiddleName || m.MiddleName == b.MiddleName || m.LastName == b.MiddleName
        && m.FirstName == b.GivenName || m.MiddleName == b.GivenName || m.LastName == b.GivenName;
}

List<Entity> FindGuidDuplicates(List<MindboxContact> list)
{
    var result = new List<Entity>();
    var tmpDict = new Dictionary<string, MindboxContact>();
    foreach (var m in mindboxContacts)
    {
        if(string.IsNullOrEmpty(m.BpmId))
            continue;
        if (!tmpDict.TryAdd(m.BpmId, m))
            result.Add(new Entity { Id = m.BpmId });
    }
    return result;
}

public class Statistics
{
    public int FoundByPhone;
    public int FoundByEmailWithPhone;
    public int FoundByEmailWithoutPhone;
    public int FullNameNotEquals;
}
