using ReportsCollectors.WebApi.Models;
using System.Collections.Generic;


namespace ReportsCollectors.WebApi.Interfaces
{
    public interface ISmtp
    {
        /// <summary>
        /// Метод осуществляющий отправку сформированного excel отчета на полученные email адреса
        /// </summary>
        /// <param name="title">Тема сообщения</param>
        /// <param name="fileReport">Excel отчет в байтах</param>
        /// <param name="dataFromPoZaim">Объект содержащий необходимые данные о получателях, наименовании документа</param> 
        void SendExelToMail(byte[] fileReport, DataFromPoZaim dataFromPoZaim);
    }
}
