using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Queries.QueryResults.Language;

namespace MeetCode.Application.Queries.QueryEntities.Language
{
    public sealed record LanguageReadQuery(Guid LangId) : IRequest<Result<LanguageReadQueryResult>>;
}
