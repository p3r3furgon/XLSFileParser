using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.DeleteAccountsByFileId
{
    public class DeleteAccountsByFileIdRequestHandler
        : IRequestHandler<DeleteAccountsByFileIdRequest, DeleteAccountsByFileIdResponse>
    {
        public BankDataContext _context;
        public DeleteAccountsByFileIdRequestHandler(BankDataContext context)
        {
            _context = context;
        }
        public async Task<DeleteAccountsByFileIdResponse> Handle(DeleteAccountsByFileIdRequest request, CancellationToken cancellationToken)
        {
            var file = await _context.Accountsources
                .Where(s => s.Id == request.FileId)
                .Include(s => s.AccountClasses)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefaultAsync();

            if (file == null)
                return new DeleteAccountsByFileIdResponse(false, "File is not found");

            _context.Accountsources.Remove(file);
            await _context.SaveChangesAsync();

            return new DeleteAccountsByFileIdResponse(true, string.Empty);
        }
    }
}
