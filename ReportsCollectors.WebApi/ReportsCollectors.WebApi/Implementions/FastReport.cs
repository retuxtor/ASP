using FastReport.ApiClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReportsCollectors.WebApi.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ReportsCollectors.WebApi.Implementions
{
    public class FastReportRepos : IFastReport
    {
        private readonly IFastReport_ApiClient _fastReportClient;
        private readonly ILogger _logger;

        public FastReportRepos(IFastReport_ApiClient fastReportClient, ILogger<FastReportRepos> logger)
        {
            _fastReportClient = fastReportClient;
            _logger = logger;
        }

        public async Task<byte[]> GetDocument(string xml, string typeTemplate, int keyCA)
        {
            try
            {
                byte[] excelDoccument = await GetExcelDocument(xml, typeTemplate);

                _logger.LogDebug("Для коллекторского агенства {keyCA} был сформирован EXCEL документ из сервиса FastReport", keyCA);

                return excelDoccument;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "Произошла ошибка при попытке сформировать Excel документ для данного коллекторского агенства: {keyCA}", keyCA);
                
                return default;
            } 
        }

        public async Task<byte[]> GetExcelDocument(string xml, string typeTemplate)
        {
            byte[] document;
            var request = new CreateDocumentCommand
            {
                Data = xml,
                CodeTemplate = typeTemplate,
                Format = Format.EXCEL,
                Date = DateTime.Now
            };

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var response = await _fastReportClient.DocumentsAsync(request);
            document = await GetByte(response.Stream);
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            stopWatch.Stop();

            _logger.LogTrace($"Файл сформировался на FastReport'e за {stopWatch.Elapsed.Minutes}:{stopWatch.Elapsed.Seconds} (mm:ss)");

            return document;
        }

        /// <summary>
        /// Получение байтов из потока
        /// </summary>
        private async Task<byte[]> GetByte(System.IO.Stream body)
        {
            byte[] buffer;

            if(body == System.IO.Stream.Null || body == null)
                throw new Exception("Поток был пуст, преобразовывать в байты нечего");

            try
            {
                await using var streamReader = new MemoryStream();
                await body.CopyToAsync(streamReader);
                buffer = streamReader.ToArray();
                return buffer;
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при попытке преобразовать Stream в byte[]", ex);
            }
        }
    }
}