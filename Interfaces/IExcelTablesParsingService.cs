using B1Task2.Models;

namespace B1Task2.Interfaces
{
    public interface IExcelTablesParsingService
    {
        List<AccountClass> ParseAccountClasses(List<string> classesData);
        List<List<Account>> ParseAccounts(List<List<List<string>>> data);
        List<List<List<Element>>> ParseElements(List<List<List<string>>> data);
        Dictionary<string, List<List<string>>> SplitTableData(IFormFile file);
    }
}
