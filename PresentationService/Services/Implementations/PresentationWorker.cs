using PresentationService.Models;
using PresentationService.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationService.Services.Implementations
{
    public class PresentationWorker : IWorkerWithDB
    {
        public PresentationContext Context { get; set; }

        public List<Presentation> GetAllFields(DateTime dateFrom, DateTime dateTo)
        {
            return Context.Presentation.Where(c => c.Time > dateFrom && c.Time < dateTo).ToList();
        }


        public async Task<ActionResult<Presentation>> GetField(int id)
        {
            var presentation = await Context.Presentation.FindAsync(id);

            if(presentation == null)
            {
                //Че та предусмотреть
            }

            return presentation;
        }

        public async Task<int> ChangeField()
        {
            return await Context.SaveChangesAsync();
        }

        public async Task<int> AddingField(Presentation presentation)
        {
            Context.Presentation.Add(presentation);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> DeleteField(int id)
        {
            var presentation = await Context.Presentation.FindAsync(id);
            Context.Presentation.Remove(presentation);
            return await Context.SaveChangesAsync();
        }
    }
}
