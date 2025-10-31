﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface ILanguageRepository
    {
        Task Update(Language newLanguage);
        Task<Language?> GetByNameAsync(string name, CancellationToken ct);
    }
}
