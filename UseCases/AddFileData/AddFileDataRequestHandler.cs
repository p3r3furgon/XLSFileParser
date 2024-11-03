using B1Task2.DataAccess;
using B1Task2.Interfaces;
using B1Task2.Models;
using MediatR;

namespace B1Task2.UseCases.AddFileData
{
    public class AddFileDataRequestHandler : IRequestHandler<AddFileDataRequest, AddFileDataResponse>
    {
        private readonly BankDataContext _context;
        private readonly IExcelTablesParsingService _excelTablesParsingService;
        public AddFileDataRequestHandler(BankDataContext context, IExcelTablesParsingService excelTablesParsingService)
        {
            _context = context;
            _excelTablesParsingService = excelTablesParsingService;
        }
        public async Task<AddFileDataResponse> Handle(AddFileDataRequest request, CancellationToken cancellationToken)
        {
            var elementsNames = new[]
{
               "IN_BALANCE_A",
               "IN_BALANCE_P",
               "TURNOVER_D",
               "TURNOVER_K",
               "OUT_BALANCE_A",
               "OUT_BALANCE_P"
            };

            try
            {
                var splitData = _excelTablesParsingService.SplitTableData(request.File);

                var accountClasses = _excelTablesParsingService.ParseAccountClasses(splitData.Keys.ToList());
                var accounts = _excelTablesParsingService.ParseAccounts(splitData.Values.ToList());
                var elements = _excelTablesParsingService.ParseElements(splitData.Values.ToList());

                var accountSource = new AccountSource() { SourceType = request.File.FileName, UploadDate = DateTime.UtcNow };

                for (int i = 0; i < elements.Count; i++)
                {
                    for (int j = 0; j < elements[i].Count; j++)
                    {
                        for (int k = 0; k < elements[i][j].Count; k++)
                        {
                            elements[i][j][k].Elementtypeid = _context.Dets
                                .Where(e => e.Name == elementsNames[k])
                                .Select(e => e.Id)
                                .FirstOrDefault();
                            accounts[i][j].Elements.Add(elements[i][j][k]);
                        }
                    }
                }

                for (int i = 0; i < accounts.Count; i++)
                    foreach (var account in accounts[i])
                    {
                        account.Class = accountClasses[i];
                        account.Class.Source = accountSource;
                        _context.Accounts.Add(account);
                    }

                await _context.SaveChangesAsync();

                return new AddFileDataResponse(true, string.Empty);
            }
            catch(Exception ex)
            {
                return new AddFileDataResponse(false, ex.Message);
            }
        }
    }
}
