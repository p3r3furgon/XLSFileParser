using B1Task2.Models;
using MediatR;

namespace B1Task2.UseCases.GetClassElementsValues
{
    public record GetClassElementsValuesRequest(string FileName)
        : IRequest<GetClassElementsValuesResponse>;
    public record GetClassElementsValuesResponse(bool IsSuccess, string Message, Dictionary<int, List<ElementDto>> ClassElements);

    public class ElementDto
    { 
        public int ElementTypeId { get; set; }
        public decimal Value { get; set; }
    }

}
