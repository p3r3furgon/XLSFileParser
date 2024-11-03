using B1Task2.DataAccess;
using B1Task2.UseCases.AddFileData;
using B1Task2.UseCases.DeleteAccountsByFileName;
using B1Task2.UseCases.GetAccountsByFileName;
using B1Task2.UseCases.GetClassElementsValues;
using B1Task2.UseCases.GetElementTypes;
using B1Task2.UseCases.GetFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace B1Task2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankDataController : ControllerBase
    {
        private readonly BankDataContext _context;
        private readonly IMediator _mediator;

        public BankDataController(BankDataContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        /// <summary>
        /// Загрузка данных из .XLS файла определенной сигнатуры
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Возвращает успешность выполнения запроса(IsSuccess). В случае IsSuccess = false, 
        /// свойство Message содержит строку с описанием ошибки</returns>
        [HttpPost]
        public async Task<IActionResult> LoadDataFromFile([FromForm] AddFileDataRequest request)
        {
            var response = await _mediator.Send(request);

            if(!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status201Created, response);

        }

        /// <summary>
        /// Метод получения списка названий файлов 
        /// </summary>
        /// <returns>Возвращает список названий файлов - источников данных аккаунтов</returns>
        [HttpGet("files")]
        public async Task<IActionResult> GetFiles()
        {
            var response = await _mediator.Send(new GetFilesRequest());

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Метод получения типов элементов аккаунтов
        /// </summary>
        /// <returns>Возвращает список типов элементов аккаунтов</returns>
        [HttpGet("element_types")]
        public async Task<IActionResult> GetELementTypes()
        {
            var response = await _mediator.Send(new GetElementTypesRequest());

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Метод получения данных аккаунтов по названию файла
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <returns>Возвращает список аккаунтов</returns>
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetAccountsByFileName(string fileName)
        {
            var response = await _mediator.Send(new GetAccountsByFileNameRequest(fileName));

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Метод получения суммарных данных по классам по названию файла
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <returns>Возвращает словарь суммарных данных по классам</returns>
        [HttpGet("{fileName}/summary")]
        public async Task<IActionResult> Test(string fileName)
        {
            var response = await _mediator.Send(new GetClassElementsValuesRequest(fileName));

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }

        /// <summary>
        /// Удаляет аккаунты по названию их файла-источника
        /// </summary>
        /// <param name="fileName">Название файла</param>
        /// <returns>Возвращает успешность выполнения запроса(IsSuccess). В случае IsSuccess = false, 
        /// свойство Message содержит строку с описанием ошибки</returns>
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> DeleteAccountsByFileName(string fileName)
        {
            var response = await _mediator.Send(new DeleteAccountsByFileNameRequest(fileName));

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status400BadRequest, response);

            return StatusCode(StatusCodes.Status200OK, response);
        }
    }
}
