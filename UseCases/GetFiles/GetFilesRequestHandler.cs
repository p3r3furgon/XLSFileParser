using AutoMapper;
using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.GetFiles
{
    public class GetFilesRequestHandler: IRequestHandler<GetFilesRequest, GetFilesResponse>
    {
        private readonly BankDataContext _context;
        private readonly IMapper _mapper;
        public GetFilesRequestHandler(BankDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetFilesResponse> Handle(GetFilesRequest request, CancellationToken cancellationToken1)
        {
            try
            {
                var files = await _context.Accountsources
                    .OrderByDescending(s => s.UploadDate)
                    .ToListAsync(cancellationToken1);

                var filesDto = _mapper.Map<List<FileDto>>(files);

                return new GetFilesResponse(true, string.Empty, filesDto);
            }
            catch(Exception ex)
            {
                return new GetFilesResponse(false, ex.Message, null);
            }
        }
    }
}