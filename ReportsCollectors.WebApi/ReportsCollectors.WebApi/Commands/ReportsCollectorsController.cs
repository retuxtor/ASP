using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportsCollectors.WebApi.Interfaces;
using ReportsCollectors.WebApi.Models;
using System.Threading.Tasks;


namespace ReportsCollectors.WebApi.Commands
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsCollectorsController : ControllerBase
    {
        private readonly IDataBase _dataBase;
        private readonly IFastReport _fastReport;
        private readonly ISmtp _smtp;
        private readonly ILogger _logger;
        public ReportsCollectorsController(IDataBase dataBase, IFastReport fastReport, ISmtp smtp, ILogger<ReportsCollectorsController> logger)
        {
            _dataBase = dataBase;
            _fastReport = fastReport;
            _smtp = smtp;
            _logger = logger;
        }

        /// <summary>
        /// Проверка работы
        /// </summary>
        /// <returns>200</returns>
        [HttpGet("checkstate")]
        public async Task<IActionResult> CheckState()
        {
            return Ok("Сервис ReportsCollectors.WebApi работает");
        }


        /// <summary>
        /// Получение данных для формирования для отчетов и отправки по email. TODO старый метод, в ходе перехода нужно убрать
        /// </summary>
        /// <param name="codeTime">Код времени, сейчас доступны 1/2</param>
        /// <returns></returns>
        /// <param name="codeReport">Код отчета, сейчас доступны 133/132</param>
        /// <returns></returns>
        [HttpGet("send/code/")]
        public async Task<IActionResult> SendReportCode(int codeTime, int codeReport)
        {
            var dataFromDb = _dataBase.SendCodeReport(codeTime, codeReport);
            int excelCounterToMail = 0;
            dataFromDb.ForEach(async currentData =>
            {
                var document = await _fastReport.GetDocument(currentData.ReportData, currentData.Template, currentData.KeyCA);
                var topik = currentData.Topic;
                _smtp.SendExelToMail(document, currentData);
                excelCounterToMail++;
            });

            return Ok($"Количество отправленных документов: {excelCounterToMail}");
        }


        /// <summary>
        /// Формирование отчета и отправка его по email
        /// </summary>
        /// <param name="dataReport">Данные о там для какого КА какой отчет сформировался</param>
        [HttpPost("generate/report")]
        public async Task<IActionResult> SendReport(InformationAboutPreparedReports dataReport)
        {
            var dataFromDb = _dataBase.GetDataForReport(dataReport);

            foreach(var currenData in dataFromDb)
            {
                var document = await _fastReport.GetDocument(currenData.ReportData, currenData.Template, currenData.KeyCA);

                _smtp.SendExelToMail(document, currenData);
            }
           
            return Ok($"Запрос на формирование и отправку отчетов был получен");
        }
    }  
}
