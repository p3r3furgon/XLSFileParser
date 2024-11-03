using MediatR;

namespace B1Task2.UseCases.DeleteAccountsByFileId
{
    public record DeleteAccountsByFileIdRequest(int FileId) 
        : IRequest<DeleteAccountsByFileIdResponse>;

    public record DeleteAccountsByFileIdResponse(bool IsSuccess, string Message);

}
