using ReportsCollectors.WebApi.Models;
using System.Collections.Generic;

namespace ReportsCollectors.WebApi.Interfaces
{
    public interface IDataBase
    {
        /// <summary>
        /// Получить данные для формирования отчета
        /// </summary>
        /// <param name="informationAboutPreparedReports">Все необходимые данные для ХП</param>
        /// <returns></returns>
        public IEnumerable<DataFromPoZaim> GetDataForReport(InformationAboutPreparedReports informationAboutPreparedReports);

        /// <summary>
        /// Отправка кода времени в ХП ПО Заим, инициализации формирования данных для отчета
        /// </summary>
        /// <param name="codeTime">Код интервала времени</param>
        /// <param name="typeReport">Тип отчета (132 - договора, 133 - платежи)</param>
        /// <returns>возвращает список данных которые необходимо отправить на email</returns>
        public List<DataFromPoZaim> SendCodeReport(int codeTime, int typeReport);
    }
}
