// src/api/atm/services.ts

import type {
  AccountDto,
  TransactionDto,
  DepositRequest,
  WithdrawRequest,
  TransferRequest,
  GetTransactionsRequest,
  PagedResult,
} from './contracts';
import { AtmRoutes } from './routes';
import api from '..';

/* --------------------------- GET: all accounts --------------------------- */
const getAccounts = async (): Promise<AccountDto[]> => {
  const { data } = await api.get<AccountDto[]>(AtmRoutes.listAccounts);
  return data;
};


/* ------------------------ GET: account transactions ---------------------- */
const getTransactions = async (
  accountId: string,
  req: GetTransactionsRequest,
): Promise<PagedResult<TransactionDto>> => {
  console.log('getting transactions...')
  const { data } = await api.get<PagedResult<TransactionDto>>(
    AtmRoutes.getTransactions(accountId),
    {
      params: req, // { page, pageSize }
    },
  );
  return data;
};

/* ------------------------------- POST: deposit --------------------------- */
const deposit = async (id: string, payload: DepositRequest): Promise<AccountDto> => {
  const { data } = await api.post<AccountDto>(AtmRoutes.deposit(id), payload);
  return data;
};

/* ------------------------------- POST: withdraw -------------------------- */
const withdraw = async (id: string, payload: WithdrawRequest): Promise<AccountDto> => {
  const { data } = await api.post<AccountDto>(AtmRoutes.withdraw(id), payload);
  return data;
};

/* ------------------------------- POST: transfer -------------------------- */
const transfer = async (payload: TransferRequest): Promise<void> => {
  await api.post(AtmRoutes.transfer, payload);
};

/* ----------------------------- Export Service ---------------------------- */
export const atmService = {
  getAccounts,
  getTransactions,
  deposit,
  withdraw,
  transfer,
};
