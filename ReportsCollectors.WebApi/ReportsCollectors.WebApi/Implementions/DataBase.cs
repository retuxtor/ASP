using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReportsCollectors.WebApi.Interfaces;
using Dapper;
using System.Data;
using ReportsCollectors.WebApi.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace ReportsCollectors.WebApi.Implementions
{
    public class DataBase : IDataBase
    {
        private readonly DbContext _context;
        private readonly AppSettings _configuration;
        private readonly ILogger _logger;

        public DataBase(DbContext context, IOptions<AppSettings> options, ILogger<DataBase> logger)
        {
            _context = context;
            _configuration = options.Value;
            _logger = logger;
        }

        public IEnumerable<DataFromPoZaim> GetDataForReport(InformationAboutPreparedReports informationAboutPreparedReports)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());

            var param = new
            {
                keyCA = informationAboutPreparedReports.KeyCa,
                keyTD = informationAboutPreparedReports.CodeReport,
                schedule = informationAboutPreparedReports.CodeSchedule
            };

            _logger.LogDebug("С даннами параметрами {@param} была вызвана ХП {storedProcedure}", param, "GetDataForAutosend");

            var dataForReport = connection.Query<DataFromPoZaim>(sql: "GetDataForAutosend", param: param, commandType: CommandType.StoredProcedure, commandTimeout: _configuration.TimeOutConnection);

            return dataForReport;
        }

        public List<DataFromPoZaim> SendCodeReport(int codeTime, int typeReport)
        {
            using var connection = new SqlConnection(_context.Database.GetConnectionString());
            var param = new
            {
                schedule = codeTime,
                keyTD = typeReport
            };
            
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var listDocument = connection.Query<DataFromPoZaim>(sql: "SendReportCA", param: param, commandType: CommandType.StoredProcedure, commandTimeout: _configuration.TimeOutConnection);

            stopWatch.Stop();
            
            _logger.LogTrace($"ХП SendReportCA выполнилась за {stopWatch.Elapsed.Minutes}:{stopWatch.Elapsed.Seconds} (mm:ss)");

            return listDocument.ToList();
        }

    }

}
