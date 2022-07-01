namespace ReportsCollectors.WebApi.Models
{
    /// <summary>
    /// Модель, которая отвечает за получение данных о готовых отчетах от ПО Заим
    /// </summary>
    public class InformationAboutPreparedReports
    {
        /// <summary>
        /// Номер коллекторского агенства для которого отчет подготовлен
        /// </summary>
        public int KeyCa { get; set; }

        /// <summary>
        /// Код отчета
        /// </summary>
        public int CodeReport { get; set; }

        /// <summary>
        /// Код расписания, недельный или месячный
        /// </summary>
        public int CodeSchedule { get; set; }
    }
}
