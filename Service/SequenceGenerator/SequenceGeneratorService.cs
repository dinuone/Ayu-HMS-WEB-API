using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator
{
    public class SequenceGeneratorService : ISequenceGeneratorService
    {
        private readonly ApplicationDBContext _dbContext;

        public SequenceGeneratorService(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<string> GenerateRegistrationNumberAsync(string title)
        {
            var now = DateTime.UtcNow;
            var year = now.Year;
            var monthLetter = (char)('A' + now.Month - 1); // January = A, ..., December = L

            var sequence = await _dbContext.Sequences
                .FirstOrDefaultAsync(x => x.Title == title);

            if (sequence == null)
            {
                sequence = new SequenceDataModel
                {
                    Title = title,
                    Start = 1,
                    Current = 1,
                    End = 9999999999999,
                    Increment = 1,
                    created_at = now,
                    updated_at = now
                };

                _dbContext.Sequences.Add(sequence);
            }
            else
            {
                if (sequence.Current >= sequence.End)
                {
                    throw new Exception("Sequence range exceeded.");
                }

                sequence.Current += sequence.Increment;
                sequence.updated_at = now;
                _dbContext.Sequences.Update(sequence);
            }

            await _dbContext.SaveChangesAsync();

            string paddedSequence = sequence.Current.ToString("D4"); 
            return $"{year}{monthLetter}{paddedSequence}";
        }
    }
}
