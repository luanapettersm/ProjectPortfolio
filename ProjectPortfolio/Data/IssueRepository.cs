using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    internal class IssueRepository(IDbContextFactory<Repository> dbContextFactory, 
        ISystemUserRepository systemUserRepository, 
        IClientRepository clientRepository) : IIssueRepository
    { 
        public async Task<FilterResponseModel<IssueModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<IssueModel>().AsQueryable();

            //Pegar usuário logado
            //var userId = await cookie.GetSystemUserId();

            if (filter.IssueId != null)
            {
                query = query.Where(e => e.Id == filter.IssueId);
            }
            else
            {
                if (filter.IssueStatus == IssueStatusEnum.Opened)
                {
                    query = query.Where(e => e.AttendantId == null
                        && e.Status != IssueStatusEnum.Closed
                        && e.DateClosed == null);
                }
                else if (filter.IssueStatus == IssueStatusEnum.Pending)
                {
                    query = query.Where(e => e.DateClosed == null/* &&
                        (e.AttendantId != null && e.AttendantId == userId*/
                            && e.Status != IssueStatusEnum.Closed && e.Status != IssueStatusEnum.InProgress);
                    //);
                }
                else if (filter.IssueStatus == IssueStatusEnum.InProgress)
                {
                    query = query.Where(e => e.AttendantId != null /*&& e.AttendantId == userId*/ && e.DateClosed == null);
                }
                else if (filter.IssueStatus == IssueStatusEnum.Closed)
                {
                    //Fazer validação para sair da coluna de Closed em x dias
                    //var dateClose = DateTimeOffset.Now.AddDays(7);

                    //query = query.Where(e => e.DateClosed != null && e.DateClosed > dateClose);
                }
            }
            if (filter.Search != null && filter.Search.Length >= 3)
            {
                query = query.Where(e => e.Title.Contains(filter.Search) || e.Description.Contains(filter.Search) || e.SequentialId.ToString().Contains(filter.Search));
            }

            if (filter.ClientIds != null)
            {
                query = query.Where(e => filter.ClientIds.Contains(e.ClientId));
            }

            var total = query.Count();
            if (filter.IssueStatus == IssueStatusEnum.Opened)
            {
                query = query.OrderByDescending(e => e.SequentialId);
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).Select(e => e);
            }

            var cards = await query.AsNoTracking().Select(e => new IssueModel()
            {
                Id = e.Id,
                Title = e.Title,
                AttendantId = e.AttendantId,
                ClientId = e.ClientId,
                SequentialId = e.SequentialId,
                DateClosed = e.DateClosed,
                Priority = e.Priority,
                ClientName = e.ClientName
            }).ToListAsync();

            var userIds = cards.Where(e => e.AttendantId != null).Select(e => (Guid)e.AttendantId).ToList();
            userIds.AddRange(cards.Select(e => (Guid)e.AttendantId));
            var users = await systemUserRepository.GetListAsync(userIds.Distinct());
            var clientIds = cards.Select(e => e.ClientId).ToArray();
            var clients = await clientRepository.GetListAsync(clientIds);
            
            foreach (var card in cards)
            {
                card.ClientName = clients.Where(e => e.Id == card.ClientId).Select(e => e.Name).FirstOrDefault();
                if (card.AttendantId.HasValue)
                    card.AttendantName = users.Where(e => e.Id == card.AttendantId).Select(e => e.DisplayName).FirstOrDefault();
            }

            cards = cards.OrderByDescending(e => e.SequentialId).ToList();

            return new FilterResponseModel<IssueModel>()
            {
                Total = total,
                Result = cards
            };
        }

        public IQueryable<IssueModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<IssueModel>();
        }

        public async Task<IssueModel> InsertAsync(IssueModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IssueModel> GetAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<IssueModel>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IssueModel>> RetrieveInProgressIssuesPerAttendant(Guid attendantId)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var tickets = await ct.Set<IssueModel>()
                .Where(e => e.AttendantId == attendantId)
                .Where(e => e.Status != IssueStatusEnum.Closed)
                .Where(e => e.DateClosed == null)
                .ToListAsync();
            return tickets;
        }
    }
}
