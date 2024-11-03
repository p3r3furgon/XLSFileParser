using AutoMapper;
using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.GetAccountsByFileId
{
    public class GetAccountsByFileIdRequestHandler :
        IRequestHandler<GetAccountsByFileIdRequest, GetAccountsByFileIdResponse>
    {
        private readonly BankDataContext _context;
        private readonly IMapper _mapper;

        public GetAccountsByFileIdRequestHandler(BankDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetAccountsByFileIdResponse> Handle(GetAccountsByFileIdRequest request, 
            CancellationToken cancellationToken)
        {
            try 
            {
                var accounts = await _context.Accounts
                    .Where(a => a.Class.Source.Id == request.FileId)
                    .Include(a => a.Class)
                    .Include(a => a.Elements)
                    .ToListAsync();

                var accountsInfoDtos = _mapper.Map<List<AccountInfoDto>>(accounts);

                return new GetAccountsByFileIdResponse(true, string.Empty, accountsInfoDtos);
            }
            catch(Exception ex)
            {
                return new GetAccountsByFileIdResponse(false, ex.Message, null);
            }
        }
    }
}
