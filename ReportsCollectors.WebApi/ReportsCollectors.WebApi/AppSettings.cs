namespace ReportsCollectors.WebApi
{
    /// <summary>
    /// Конфигурация сервиса 
    /// <remarks>\include DownloadTerroristFileService/appsettings.json</remarks>
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Урл для подключения к сервису фаст репорта
        /// </summary>
        public string UrlFastReport { get; set; }

        /// <summary>
        /// Сервер исходящей почты
        /// </summary>
        public string MailServerURL { get; set; }

        /// <summary>
        /// Порт сервера исходящей почты
        /// </summary>
        public int MailServerPort { get; set; }

        /// <summary>
        /// email с которого осуществляется отправка сообщений
        /// </summary>
        public string SendersMail { get; set; }

        /// <summary>
        /// Пароль почтового клиента
        /// </summary>
        public string MailClientPassword { get; set; }

        /// <summary>
        /// Название организации(имя отправителя в письме)
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Таймаут для ХП SendReportCA
        /// </summary>
        public int TimeOutConnection { get; set; }

        /// <summary>
        /// Таймаут для HTTP clienta
        /// </summary>
        public int TimeOutHTTPConnection { get; set; }
    }

}
