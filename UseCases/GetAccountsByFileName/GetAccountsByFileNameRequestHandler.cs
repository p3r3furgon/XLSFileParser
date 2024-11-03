using AutoMapper;
using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.GetAccountsByFileName
{
    public class GetAccountsByFileNameRequestHandler :
        IRequestHandler<GetAccountsByFileNameRequest, GetAccountsByFileNameResponse>
    {
        private readonly BankDataContext _context;
        private readonly IMapper _mapper;

        public GetAccountsByFileNameRequestHandler(BankDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetAccountsByFileNameResponse> Handle(GetAccountsByFileNameRequest request, 
            CancellationToken cancellationToken)
        {
            try 
            {
                var accounts = await _context.Accounts
                    .Where(a => a.Class.Source.SourceType == request.FileName)
                    .Include(a => a.Class)
                    .Include(a => a.Elements)
                    .ToListAsync();

                var accountsInfoDtos = _mapper.Map<List<AccountInfoDto>>(accounts);

                return new GetAccountsByFileNameResponse(true, string.Empty, accountsInfoDtos);
            }
            catch(Exception ex)
            {
                return new GetAccountsByFileNameResponse(false, ex.Message, null);
            }
        }
    }
}
