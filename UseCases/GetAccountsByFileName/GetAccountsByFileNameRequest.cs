using B1Task2.Models;
using MediatR;

namespace B1Task2.UseCases.GetAccountsByFileName
{
    public record GetAccountsByFileNameRequest(string FileName): 
        IRequest<GetAccountsByFileNameResponse>;
    public record GetAccountsByFileNameResponse(bool IsSuccess, string Message, List<AccountInfoDto> Accounts);

    public class AccountInfoDto
    { 
        public int AccountCode { get; set; }
        public int ClassCode { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public List<Element> Elements { get; set; } = new List<Element>();
    }

}
