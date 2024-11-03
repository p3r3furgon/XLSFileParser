using MediatR;

namespace B1Task2.UseCases.GetFiles
{
    public record GetFilesRequest(): IRequest<GetFilesResponse>;
    
    public record GetFilesResponse(bool IsSuccess, string Message, List<FileDto> Files);

    public class FileDto
    {
        public int fileId { get; set; }
        public string fileName { get; set; } = string.Empty;
        public DateTime? dateAdded { get; set; }
    }

}
