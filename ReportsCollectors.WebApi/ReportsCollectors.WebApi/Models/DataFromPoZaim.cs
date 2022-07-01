using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ReportsCollectors.WebApi.Models
{
    /// <summary>
    /// Класс реализующий модель данных которые будут полученные из ПО Заим.
    /// Тут содержатся данные о получателях, куда будут отправлены отчеты, и данные для формирования отчетов
    /// </summary>
    public record DataFromPoZaim
    {
        /// <summary>
        /// Код коллекторского агенства
        /// </summary>
        public int KeyCA { get; set; }

        /// <summary>
        /// Тема сообщения
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// Тип шаблона
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// Имя, которое необходимо будет задать сформированному документу
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Получатели, массив объектов json 
        /// </summary>
        public string MailCA { get; set; }

        /// <summary>
        /// Данные для формирования отчета, являются xml строкой.
        /// </summary>
        public string ReportData { get; set; }

        /// <summary>
        /// Список получателей 
        /// </summary>
        public List<Receiver> ReceiverForSMPT => JsonConvert.DeserializeObject<List<Receiver>>(MailCA).ToList();
    }

    /// <summary>
    /// Класс получателя
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// Почта получателя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Является ли копией
        /// </summary>
        public bool Copy { get; set; }
    }
}