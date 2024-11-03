using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.GetElementTypes
{
    public class GetElementTypesRequestHandler
        : IRequestHandler<GetElementTypesRequest, GetElementTypesResponse>
    {
        public BankDataContext _context;
        public GetElementTypesRequestHandler(BankDataContext context)
        {
            _context = context;
        }

        public async Task<GetElementTypesResponse> Handle(GetElementTypesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var elementTypes = await _context.Dets.ToDictionaryAsync(item => item.Id, item => item.Description);
                return new GetElementTypesResponse(true, string.Empty, elementTypes);
            }
            catch(Exception ex)
            {
                return new GetElementTypesResponse(true, ex.Message, null);
            }
            
        }
    }
}
