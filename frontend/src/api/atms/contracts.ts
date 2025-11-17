// src/api/atm/contracts.ts

export type PagedResult<T> = {
  items: T[];
  totalCount: number;
};

/* ------------------------------ Account DTOs ------------------------------ */

export type AccountDto = {
  id: string;
  name: string;
  balance: number;
};

/* ---------------------------- Transaction DTOs ---------------------------- */

export type GetTransactionsRequest = {
  page: number;
  pageSize: number;
}

export type TransactionDto = {
  id: string;
  accountId: string;
  type: TransactionType;
  amount: number;
  occurredAtUtc: string;
  description: string | null;
};

export type TransactionType =
  | 'Deposit'
  | 'Withdrawal'
  | 'TransferIn'
  | 'TransferOut';

/* ------------------------------- Requests -------------------------------- */

export type DepositRequest = {
  amount: number;
};

export type WithdrawRequest = {
  amount: number;
};

export type TransferRequest = {
  fromAccountId: string;
  toAccountId: string;
  amount: number;
};
