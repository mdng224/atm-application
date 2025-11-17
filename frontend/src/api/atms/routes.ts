// src/api/atm/routes.ts

export const AtmRoutes = Object.freeze({
  // Accounts
  listAccounts: '/accounts',

  // Transactions
  getTransactions: (id: string) => `/accounts/${id}/transactions`,

  // Actions
  deposit: (id: string) => `/accounts/${id}/deposit`,
  withdraw: (id: string) => `/accounts/${id}/withdraw`,
  transfer: '/accounts/transfer',
} as const);
