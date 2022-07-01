using System.Threading.Tasks;

namespace ReportsCollectors.WebApi.Interfaces
{
    public interface IFastReport
    {
        /// <summary>
        /// Взаимодействие с сервисом фаст репорта, для получения файла необходимого формата
        /// </summary>
        /// <param name="xml">Данные для формирования шаблона</param>
        /// <param name="typeTemplate">Тип шаблона</param>
        /// <param name="keyCA">Номер коллектрского агенства</param>
        /// <returns></returns>
        public Task<byte[]> GetDocument(string xml, string typeTemplate, int keyCA);
    }
}
