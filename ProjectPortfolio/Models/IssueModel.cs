﻿using ProjectPortfolio.Enumerators;

namespace ProjectPortfolio.Models
{
    public class IssueModel
    {
        public Guid Id { get; set; }
        public long SequentialId { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public Guid? AttendantId { get; set; }
        public string Description { get; set; }
        public PriorityEnum Priority { get; set; }
        public string Title { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateClosed { get; set; }
        public IssueStatusEnum Status { get; set; }
        public string Solution { get; set; }
        public Guid ClientProjectId {get; set;}

        public List<string> CreateValidator()
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(Title) || Title.Length < 3 || Title.Length > 100)
                messages.Add("O título deve ter entre 3 e 100 caracteres.");
            if (string.IsNullOrEmpty(Description) || Description.Length < 3 ||Description.Length > 2000)
                messages.Add("O título deve ter entre 3 e 2000 caracteres.");
            if (ClientId == Guid.Empty)
                messages.Add("O cliente é obrigatório.");
            if (Priority.GetType() == null)
                messages.Add("A prioridade é obrigatória.");

            return messages;
        }
    }
}
