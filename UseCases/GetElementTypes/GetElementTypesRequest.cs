using MediatR;

namespace B1Task2.UseCases.GetElementTypes
{
    public record GetElementTypesRequest : IRequest<GetElementTypesResponse>;
    public record GetElementTypesResponse(bool IsSuccess, string Message, Dictionary<int, string?> ElementTypes);
}
