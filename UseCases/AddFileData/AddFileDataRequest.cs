using MediatR;

namespace B1Task2.UseCases.AddFileData
{
    public record AddFileDataRequest(IFormFile File) : IRequest<AddFileDataResponse>; 

    public record AddFileDataResponse(bool IsSuccess, string Message, int FileId);
}
