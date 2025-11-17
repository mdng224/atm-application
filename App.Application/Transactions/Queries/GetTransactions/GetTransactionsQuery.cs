using App.Application.Common.Pagination;

namespace App.Application.Transactions.Queries.GetTransactions;

public sealed record GetTransactionsQuery(Guid AccountId, PagedQuery PagedQuery);