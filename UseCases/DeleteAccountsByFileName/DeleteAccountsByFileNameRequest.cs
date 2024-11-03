using MediatR;

namespace B1Task2.UseCases.DeleteAccountsByFileName
{
    public record DeleteAccountsByFileNameRequest(string FileName) 
        : IRequest<DeleteAccountsByFileNameResponse>;

    public record DeleteAccountsByFileNameResponse(bool IsSuccess, string Message);

}
