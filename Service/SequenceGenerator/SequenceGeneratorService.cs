using Harendra_Ayurveda_HMS_WEB_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using static Harendra_Ayurveda_HMS_WEB_API.Helpers.GlobalAttributeHelper;

namespace Harendra_Ayurveda_HMS_WEB_API.Service.SequenceGenerator
{
    public class SequenceGeneratorService : ISequenceGeneratorService
    {
        private readonly ApplicationDBContext _dbContext;

        public SequenceGeneratorService(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<string> GenerateRegistrationNumberAsync()
        {
            var now = DateTime.UtcNow;
            var year = now.ToString("yy"); // 2-digit year
            var month = now.Month;
            var sequenceTitle = $"PATIENT-{year}-{month}"; // Unique key per month

            using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            try
            {
                var sequence = await _dbContext.Sequences
                    .FirstOrDefaultAsync(s => s.Title == sequenceTitle);

                if (sequence == null)
                {
                    sequence = new SequenceDataModel
                    {
                        Title = sequenceTitle,
                        Sequence = Sequences.REGISTRATION_SEQUENCE,
                        Start = 1,
                        Current = 1,
                        End = 9999, // Reset monthly with 4-digit sequence
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
                        throw new InvalidOperationException("Monthly patient registration limit reached");
                    }

                    sequence.Current++;
                    sequence.updated_at = now;
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return $"HA-{year}-{month}-{sequence.Current:D4}"; // Format: 23-1-0001
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<string> GenerateOPDNumberAsync()
        {
            var now = DateTime.UtcNow;
            var year = now.ToString("yy");         // 2-digit year (23)
            var monthLetter = (char)('A' + now.Month - 1);  // A-L for months
            var sequenceTitle = $"OPD-{year}-{monthLetter}"; // Unique monthly key

            using var transaction = await _dbContext.Database.BeginTransactionAsync(
                IsolationLevel.Serializable
            );

            try
            {
                var sequence = await _dbContext.Sequences
                    .FirstOrDefaultAsync(x => x.Title == sequenceTitle);

                if (sequence == null)
                {
                    sequence = new SequenceDataModel
                    {
                        Title = sequenceTitle,
                        Sequence = Sequences.OPD_SEQUENCE,
                        Start = 1,
                        Current = 0, // Will increment to 1 on first use
                        End = 9999,  // Reset monthly with 4-digit capacity
                        Increment = 1,
                        created_at = now,
                        updated_at = now
                    };
                    _dbContext.Sequences.Add(sequence);
                }

                if (sequence.Current >= sequence.End)
                {
                    throw new InvalidOperationException("Monthly OPD number capacity exhausted");
                }

                sequence.Current += sequence.Increment;
                sequence.updated_at = now;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return $"{year}{monthLetter}{sequence.Current:D4}"; // Format: 23L0001
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
