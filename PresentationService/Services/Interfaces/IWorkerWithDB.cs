using Microsoft.AspNetCore.Mvc;
using PresentationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationService.Services.Interfaces
{
    public interface IWorkerWithDB
    {
        PresentationContext Context { get; set; }

        /// <summary>
        /// Метод, который возвращает все значения полей из presentation
        /// </summary>
        /// <returns></returns>
        /// 
        List<Presentation> GetAllFields(DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Метод, который возвращает конкретное поле
        /// </summary>
        /// <param name="id">Интересующий номер поля presentation</param>
        /// <returns></returns>
        Task<ActionResult<Presentation>> GetField(int id);

        /// <summary>
        /// Метод, который изменяет существующую запись presentation
        /// </summary>
        /// <param name="id">Интересующий номер поля presentation</param>
        /// <returns></returns>
        Task<int> ChangeField();

        /// <summary>
        /// Метод, который добавляет новую запись в presentation
        /// </summary>
        /// <param name="id">Интересующий номер поля presentation</param>
        /// <returns></returns>
        Task<int> AddingField(Presentation presentation);

        /// <summary>
        /// Метод, который удаляет поле из presentation
        /// </summary>
        /// <param name="id">Интересующий номер поля presentation</param>
        /// <returns></returns>
        Task<int> DeleteField(int id);
    }
}
