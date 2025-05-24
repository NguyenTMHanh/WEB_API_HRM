using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_API_HRM.Data;
using WEB_API_HRM.Models;

namespace WEB_API_HRM.Repositories
{
    public class RankRepository : IRankRepository
    {
        private readonly HRMContext _context;
        public RankRepository(HRMContext context)
        {
            _context = context;
        }
        public async Task<IdentityResult> CreateRankAsync(RankModel model)
        {

            var existingRank = await _context.Ranks.AnyAsync(r => r.Id == model.Id);
            if (existingRank)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Rank already exists in the system." });
            }

            var rank = new RankModel
            {
                Id = model.Id,
                RankName = model.RankName,
                PriorityLevel = model.PriorityLevel,
                Description = model.Description,
            };

            await _context.Ranks.AddAsync(rank);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteRankAsync(string rankId)
        {
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == rankId);
            if( rank == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Rank not found"
                });
            }

            _context.Ranks.Remove(rank);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<RankModel>> GetAllRanksAsync()
        {
            return await _context.Ranks.ToListAsync();
        }

        public async Task<string> GetCodeRank()
        {
            try
            {
                var rankFinal = await _context.Ranks
                    .OrderBy(r => r.Id)
                    .LastOrDefaultAsync();

                if (rankFinal == null || string.IsNullOrEmpty(rankFinal.Id))
                {
                    return "RANK001";
                }

                string numericPart = rankFinal.Id.Replace("RANK", "").Trim();
                if (int.TryParse(numericPart, out int currentNumber))
                {
                    var nextNumber = currentNumber + 1;
                    var rankCode = $"RANK{nextNumber:D3}";
                    return rankCode;
                }
                return "RANK001";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCodeRank: {ex.Message}");
                throw;
            }
        }
        public async Task<RankModel> GetRankAsync(string rankId)
        {
            return await _context.Ranks.FirstOrDefaultAsync(r => r.Id == rankId);
        }

        public async Task<IdentityResult> UpdateRankAsync(string rankId, RankModel model)
        {
            var rank = await _context.Ranks.FirstOrDefaultAsync(r => r.Id == rankId);
            if (rank == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Rank not found"
                });
            }
            rank.PriorityLevel = model.PriorityLevel;
            rank.RankName = model.RankName;
            rank.Description = model.Description;

            _context.Ranks.Update(rank);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
