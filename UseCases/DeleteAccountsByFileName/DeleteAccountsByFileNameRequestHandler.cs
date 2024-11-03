using B1Task2.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace B1Task2.UseCases.DeleteAccountsByFileName
{
    public class DeleteAccountsByFileNameRequestHandler
        : IRequestHandler<DeleteAccountsByFileNameRequest, DeleteAccountsByFileNameResponse>
    {
        public BankDataContext _context;
        public DeleteAccountsByFileNameRequestHandler(BankDataContext context)
        {
            _context = context;
        }
        public async Task<DeleteAccountsByFileNameResponse> Handle(DeleteAccountsByFileNameRequest request, CancellationToken cancellationToken)
        {
            var file = await _context.Accountsources
                .Where(s => s.SourceType == request.FileName)
                .Include(s => s.AccountClasses)
                .ThenInclude(c => c.Accounts)
                .FirstOrDefaultAsync();

            if (file == null)
                return new DeleteAccountsByFileNameResponse(false, "File is not found");

            _context.Accountsources.Remove(file);
            await _context.SaveChangesAsync();

            return new DeleteAccountsByFileNameResponse(true, string.Empty);
        }
    }
}
