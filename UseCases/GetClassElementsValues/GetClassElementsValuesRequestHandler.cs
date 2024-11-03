using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace B1Task2.UseCases.GetClassElementsValues
{
    public class GetClassElementsValuesRequestHandler
        : IRequestHandler<GetClassElementsValuesRequest, GetClassElementsValuesResponse>
    {
        private readonly BankDataContext _context;
        public GetClassElementsValuesRequestHandler(BankDataContext context)
        {
            _context = context;
        }
        public async Task<GetClassElementsValuesResponse> Handle(GetClassElementsValuesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _context.Accounts
                    .Where(a => a.Class.Source.Id == request.FileId)
                    .Include(a => a.Elements)
                    .Include(a => a.Class)
                    .GroupBy(a => a.Class.ClassCode)
                    .ToDictionaryAsync(
                        g => g.Key,
                        g => g.SelectMany(a => a.Elements)
                              .GroupBy(e => e.Elementtypeid)
                              .Select(g => new ElementDto
                              {
                                  ElementTypeId = g.Key,
                                  Value = g.Sum(e => e.Value)
                              })
                              .ToList()
                    );

                Console.WriteLine(data);
                return new GetClassElementsValuesResponse(true, string.Empty, data);
            }
            catch(Exception ex)
            {
                return new GetClassElementsValuesResponse(false, ex.Message, null);
            }
        }
    }
}
