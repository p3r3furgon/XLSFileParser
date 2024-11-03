using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.GetFiles
{
    public class GetFilesRequestHandler: IRequestHandler<GetFilesRequest, GetFilesResponse>
    {
        private readonly BankDataContext _context;

        public GetFilesRequestHandler(BankDataContext context)
        {
            _context = context;
        }
        public async Task<GetFilesResponse> Handle(GetFilesRequest request, CancellationToken cancellationToken1)
        {
            try
            {
                var files = await _context.Accountsources
                    .OrderByDescending(s => s.UploadDate)
                    .Select(file => new FileDto()
                    {
                        fileName = file.SourceType,
                        dateAdded = file.UploadDate
                    }).ToListAsync(cancellationToken1);

                return new GetFilesResponse(true, string.Empty, files);
            }
            catch(Exception ex)
            {
                return new GetFilesResponse(false, ex.Message, null);
            }
        }
    }
}