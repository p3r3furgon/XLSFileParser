using System.Globalization;
using System.Text.RegularExpressions;
using B1Task2.Interfaces;
using B1Task2.Models;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;

namespace B1Task2.Services
{
    /// <summary>
    /// Сервис, используемый для парсинга XLS файла
    /// </summary>
    public class ExcelTablesParsingService : IExcelTablesParsingService
    {
        private readonly Regex _pattern = new Regex((@"(?i)класс\s+(\d+)\s+(.+)"));
        private int _blankRowsCounter = 0;
        private readonly int _maxBlankRows = 10;

        /// <summary>
        /// Парсит .XLS файл в список списков строк
        /// </summary>
        /// <param name="file">.XLS файл</param>
        /// <returns>Возвращает список списков строк - внутренний список отображает строку таблицы, элементы списка - ячейки строки</returns>
        private List<List<string>> GetTableData(IFormFile file)
        {
            try
            {
                var result = new List<List<string>>();
                using (var stream = file.OpenReadStream())
                {
                    using (StreamReader input = new StreamReader(stream))
                    {
                        IWorkbook workbook = new HSSFWorkbook(new POIFSFileSystem(input.BaseStream));
                        if (null == workbook)
                        {
                            Console.WriteLine(string.Format("Excel Workbook '{0}' could not be opened.", stream));
                            return null;
                        }

                        DataFormatter dataFormatter = new HSSFDataFormatter(new CultureInfo("ru"));

                        foreach (ISheet sheet in workbook)
                        {
                            foreach (IRow row in sheet)
                            {
                                if (row.Cells.Count == 0)
                                    if (++_blankRowsCounter == _maxBlankRows)
                                        break;

                                _blankRowsCounter = 0;
                                var rowDataList = new List<string>();
                                foreach (ICell cell in row)
                                {
                                    string value = GetValue(cell, dataFormatter);
                                    if (!string.IsNullOrWhiteSpace(value))
                                        rowDataList.Add(value);

                                }
                                result.Add(new List<string>(rowDataList));
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                if (null != ex.InnerException)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return null;
            }
        }

       /// <summary>
       /// Достает данные из ячейки таблицы
       /// </summary>
       /// <param name="cell">Ячейка таблицы</param>
       /// <returns>Строку данных текущей ячейки</returns>
        private string GetValue(ICell cell, DataFormatter dataFormatter)
        {
            string ret = string.Empty;
            if (null == cell) { return ret; }
            ret = dataFormatter.FormatCellValue(cell);
            return ret.Replace("\n", " ");
        }

        public Dictionary<string, List<List<string>>> SplitTableData(IFormFile file)
        {
            List<List<string>> data = GetTableData(file);
            data.RemoveAll(l => l.Count == 0);
            var splitData = new Dictionary<string, List<List<string>>>();
            var indices = data
                .Select((d, index) => new { List = d, Index = index })
                .Where(d => d.List.Count == 1 && _pattern.IsMatch(d.List[0]))
                .Select(d => d.Index)
                .ToList();

            for (int i = 0; i < indices.Count; i++)
            {
                int startIndex = indices[i];
                int endIndex = (i < indices.Count - 1) ? indices[i + 1] : data.Count;
                var className = data[startIndex][0];

                splitData[className] = data.GetRange(startIndex + 1, endIndex - startIndex - 1);
            }
            return splitData;
        }

        /// <summary>
        /// Парсит список строк с информацией про классы аккаунтов в список элементов типа AccountClass
        /// </summary>
        /// <param name="classesData">Строки с информацией про классы аккаунтов</param>
        /// <returns>Список элементов типа AccountClass</returns>
        public List<AccountClass> ParseAccountClasses(List<string> classesData)
        {
            var accountClasses = new List<AccountClass>();
            foreach (var data in classesData)
            {
                var match = _pattern.Match(data);

                if (match.Success)
                {
                    accountClasses.Add(
                        new AccountClass
                        {
                            ClassCode = int.Parse(match.Groups[1].Value),
                            ClassName = match.Groups[2].Value
                        });
                }
            }
            return accountClasses;
        }

        /// <summary>
        /// Парсит список строк с информацией про аккаунты в список списков элементов типа Account
        /// </summary>
        /// <param name="data">Строковые данные про аккаунтты</param>
        /// <returns>Список списков элементов типа Account</returns>
        public List<List<Account>> ParseAccounts(List<List<List<string>>> data)
        {
            var accounts = new List<List<Account>>();
            foreach (var classAccountData in data)
            {
                var currentClassAccounts = new List<Account>();
                foreach (var accountData in classAccountData)
                {

                    if (int.TryParse(accountData[0], out var result) && result >= 1000)
                    {
                        var account = new Account() { AccountCode = result };
                        currentClassAccounts.Add(account);
                    }
                }
                accounts.Add(currentClassAccounts);
            }
            return accounts;
        }

        /// <summary>
        /// Парсит список строк с информацией про элементы в 3ой вложенный список(для отдельных классов и аккаунтов) 
        /// элементов типа Element
        /// </summary>
        /// <returns>3ой вложенный список элементов типа Element</returns>
        public List<List<List<Element>>> ParseElements(List<List<List<string>>> data)
        {
            var elements = new List<List<List<Element>>>();
            foreach (var classAccountData in data)
            {
                var classElements = new List<List<Element>>();
                foreach (var elementsData in classAccountData)
                {
                    var accountElements = new List<Element>();
                    for (int i = 1; i < elementsData.Count; i++)
                    {
                        if (!int.TryParse(elementsData[0], out var res) || res < 1000)
                            break;

                        if (decimal.TryParse(elementsData[i].Replace(" ", ""), out var result))
                        {
                            var element = new Element() { Value = result };
                            accountElements.Add(element);
                        }
                    }
                    if (accountElements.Count != 0)
                        classElements.Add(accountElements);
                }
                elements.Add(classElements);
            }
            return elements;
        }
    }
}
