<template>
  <div class="space-y-10 max-w-3xl">
    <!-- TRANSFER / DEPOSIT / WITHDRAW FORMS -->
    <div class="space-y-10">
      <!-- TRANSFER FORM -->
      <div class="space-y-6 max-w-md">
        <h2 class="text-xl font-semibold mb-2">Transfer Funds</h2>

        <!-- Alerts -->
        <app-alert
          v-if="errorMessage"
          :message="errorMessage"
          variant="error"
          :icon="AlertTriangle"
        />
        <app-alert
          v-if="successMessage"
          :message="successMessage"
          variant="success"
          :icon="CheckCircle2"
        />

        <!-- FROM ACCOUNT -->
        <div>
          <label class="block text-sm font-medium mb-1">From Account</label>
          <select
            v-model="fromAccountId"
            class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
          >
            <option disabled value="">Select account</option>
            <option
              v-for="acc in accounts"
              :key="acc.id"
              :value="acc.id"
            >
              {{ acc.name }} — ${{ acc.balance }}
            </option>
          </select>
        </div>

        <!-- TO ACCOUNT -->
        <div>
          <label class="block text-sm font-medium mb-1">To Account</label>
          <select
            v-model="toAccountId"
            class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
          >
            <option disabled value="">Select account</option>
            <option
              v-for="acc in accounts"
              :key="acc.id + '-to'"
              :value="acc.id"
            >
              {{ acc.name }} — ${{ acc.balance }}
            </option>
          </select>
        </div>

        <!-- AMOUNT -->
        <div>
          <label class="block text-sm font-medium mb-1">Amount</label>
          <input
            v-model.number="amount"
            type="number"
            min="0"
            step="0.01"
            class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
            placeholder="Enter amount"
          />
        </div>

        <!-- Submit -->
        <button
          class="rounded-md bg-indigo-600 px-4 py-2 text-white font-medium hover:bg-indigo-500 disabled:opacity-60 disabled:cursor-not-allowed"
          :disabled="isSubmitting"
          @click="submitTransfer"
        >
          <span v-if="isSubmitting && currentAction === 'transfer'">Transferring...</span>
          <span v-else>Submit Transfer</span>
        </button>
      </div>

      <!-- DEPOSIT / WITHDRAW GRID -->
      <div class="grid gap-10 md:grid-cols-2 max-w-3xl">
        <!-- DEPOSIT -->
        <div class="space-y-4">
          <h2 class="text-lg font-semibold">Deposit</h2>

          <div>
            <label class="block text-sm font-medium mb-1">Account</label>
            <select
              v-model="depositAccountId"
              class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
            >
              <option disabled value="">Select account</option>
              <option
                v-for="acc in accounts"
                :key="acc.id + '-dep'"
                :value="acc.id"
              >
                {{ acc.name }} — ${{ acc.balance }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">Amount</label>
            <input
              v-model.number="depositAmount"
              type="number"
              min="0"
              step="0.01"
              class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
              placeholder="Enter amount"
            />
          </div>

          <button
            class="rounded-md bg-emerald-600 px-4 py-2 text-white font-medium hover:bg-emerald-500 disabled:opacity-60 disabled:cursor-not-allowed"
            :disabled="isSubmitting"
            @click="submitDeposit"
          >
            <span v-if="isSubmitting && currentAction === 'deposit'">Depositing...</span>
            <span v-else>Submit Deposit</span>
          </button>
        </div>

        <!-- WITHDRAW -->
        <div class="space-y-4">
          <h2 class="text-lg font-semibold">Withdraw</h2>

          <div>
            <label class="block text-sm font-medium mb-1">Account</label>
            <select
              v-model="withdrawAccountId"
              class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
            >
              <option disabled value="">Select account</option>
              <option
                v-for="acc in accounts"
                :key="acc.id + '-wd'"
                :value="acc.id"
              >
                {{ acc.name }} — ${{ acc.balance }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">Amount</label>
            <input
              v-model.number="withdrawAmount"
              type="number"
              min="0"
              step="0.01"
              class="w-full rounded-md bg-slate-800 border border-slate-700 p-2 text-white"
              placeholder="Enter amount"
            />
          </div>

          <button
            class="rounded-md bg-rose-600 px-4 py-2 text-white font-medium hover:bg-rose-500 disabled:opacity-60 disabled:cursor-not-allowed"
            :disabled="isSubmitting"
            @click="submitWithdraw"
          >
            <span v-if="isSubmitting && currentAction === 'withdraw'">Withdrawing...</span>
            <span v-else>Submit Withdraw</span>
          </button>
        </div>
      </div>
    </div>

    <!-- TRANSACTIONS TABLE -->
    <div class="space-y-3">
      <div class="flex items-center justify-between gap-3">
        <h3 class="text-lg font-semibold text-slate-100">
          Transactions for {{ activeAccountName }}
        </h3>

        <div class="flex items-center gap-2 text-sm text-slate-300">
          <span>View for:</span>
          <select
            v-model="transactionsAccountId"
            class="rounded-md bg-slate-900 border border-slate-700 px-2 py-1 text-sm"
          >
            <option
              v-for="acc in accounts"
              :key="acc.id + '-tx'"
              :value="acc.id"
            >
              {{ acc.name }}
            </option>
          </select>
        </div>
      </div>

      <data-table
        :table
        :loading="isLoadingTransactions"
        :total-count="totalTransactions"
        empty-text="No transactions found."
      />

      <table-footer
        :table
        :total-count="totalTransactions"
        :total-pages
        :pagination
        :set-page-size
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue';
import { AlertTriangle, CheckCircle2 } from 'lucide-vue-next';
import AppAlert from '@/components/AppAlert.vue';
import DataTable from '@/components/table/DataTable.vue';
import TableFooter from '@/components/table/TableFooter.vue';

import type {
  AccountDto,
  TransactionDto,
  GetTransactionsRequest,
  PagedResult,
} from '@/api/atms';
import { atmService } from '@/api/atms';
import { extractApiError } from '@/api/error';

import {
  useVueTable,
  getCoreRowModel,
  getPaginationRowModel,
  type ColumnDef,
} from '@tanstack/vue-table';

// --------------------
// Transfer / deposit / withdraw state
// --------------------

const accounts = ref<AccountDto[]>([]);
const fromAccountId = ref('');
const toAccountId = ref('');
const amount = ref<number | null>(null);

const depositAccountId = ref('');
const depositAmount = ref<number | null>(null);

const withdrawAccountId = ref('');
const withdrawAmount = ref<number | null>(null);

const errorMessage = ref<string | null>(null);
const successMessage = ref<string | null>(null);
const isSubmitting = ref(false);
const currentAction = ref<'transfer' | 'deposit' | 'withdraw' | null>(null);

const loadAccounts = async () => {
  try {
    accounts.value = await atmService.getAccounts();
  } catch (err) {
    errorMessage.value = extractApiError(err);
  }
};

// --------------------
// Submit transfer
// --------------------

const submitTransfer = async () => {
  errorMessage.value = null;
  successMessage.value = null;

  if (!fromAccountId.value || !toAccountId.value) {
    errorMessage.value = 'Please select both accounts.';
    return;
  }

  if (fromAccountId.value === toAccountId.value) {
    errorMessage.value = 'Cannot transfer to the same account.';
    return;
  }

  if (!amount.value || amount.value <= 0) {
    errorMessage.value = 'Enter an amount greater than zero.';
    return;
  }

  isSubmitting.value = true;
  currentAction.value = 'transfer';

  try {
    await atmService.transfer({
      fromAccountId: fromAccountId.value,
      toAccountId: toAccountId.value,
      amount: amount.value,
    });

    successMessage.value = 'Transfer complete.';
    await loadAccounts();
    await loadTransactions();
  } catch (err) {
    errorMessage.value = extractApiError(err, 'amount');
  } finally {
    isSubmitting.value = false;
    currentAction.value = null;
    amount.value = null;
  }
};

// --------------------
// Submit deposit
// --------------------

const submitDeposit = async () => {
  errorMessage.value = null;
  successMessage.value = null;

  if (!depositAccountId.value) {
    errorMessage.value = 'Please select an account for deposit.';
    return;
  }

  if (!depositAmount.value || depositAmount.value <= 0) {
    errorMessage.value = 'Enter a deposit amount greater than zero.';
    return;
  }

  isSubmitting.value = true;
  currentAction.value = 'deposit';

  try {
    await atmService.deposit(depositAccountId.value, {
      amount: depositAmount.value,
    });

    successMessage.value = 'Deposit complete.';
    await loadAccounts();
    await loadTransactions();
  } catch (err) {
    errorMessage.value = extractApiError(err, 'amount');
  } finally {
    isSubmitting.value = false;
    currentAction.value = null;
    depositAmount.value = null;
  }
};

// --------------------
// Submit withdraw
// --------------------

const submitWithdraw = async () => {
  errorMessage.value = null;
  successMessage.value = null;

  if (!withdrawAccountId.value) {
    errorMessage.value = 'Please select an account to withdraw from.';
    return;
  }

  if (!withdrawAmount.value || withdrawAmount.value <= 0) {
    errorMessage.value = 'Enter a withdrawal amount greater than zero.';
    return;
  }

  isSubmitting.value = true;
  currentAction.value = 'withdraw';

  try {
    await atmService.withdraw(withdrawAccountId.value, {
      amount: withdrawAmount.value,
    });

    successMessage.value = 'Withdrawal complete.';
    await loadAccounts();
    await loadTransactions();
  } catch (err) {
    errorMessage.value = extractApiError(err, 'amount');
  } finally {
    isSubmitting.value = false;
    currentAction.value = null;
    withdrawAmount.value = null;
  }
};

// --------------------
// Transactions table
// --------------------

const transactions = ref<TransactionDto[]>([]);
const totalTransactions = ref(0);
const isLoadingTransactions = ref(false);

// Explicit account selection for transactions
const transactionsAccountId = ref<string>('');

// Active account id/name for the table
const activeAccountId = computed(() => transactionsAccountId.value || '');
const activeAccountName = computed(() => {
  const acc = accounts.value.find(a => a.id === activeAccountId.value);
  return acc ? acc.name : '—';
});

// Pagination
const pagination = ref({
  pageIndex: 0,
  pageSize: 10,
});

// Table columns
const columns = ref<ColumnDef<TransactionDto>[]>([
  { accessorKey: 'occurredAtUtc', header: 'Date', meta: { kind: 'datetime' } },
  {
    accessorKey: 'type',
    header: 'Type',
    meta: { kind: 'badge' },
  },
  { accessorKey: 'amount', header: 'Amount', meta: { kind: 'text' } },
  { accessorKey: 'description', header: 'Description', meta: { kind: 'text' } },
]);

// Table instance
const table = useVueTable({
  get data() {
    return transactions.value;
  },
  get columns() {
    return columns.value;
  },
  state: {
    get pagination() {
      return pagination.value;
    },
  },
  onPaginationChange: updater => {
    pagination.value =
      typeof updater === 'function' ? updater(pagination.value) : updater;
    loadTransactions();
  },
  getCoreRowModel: getCoreRowModel(),
  getPaginationRowModel: getPaginationRowModel(),
  manualPagination: true,
});

const totalPages = computed(() =>
  totalTransactions.value === 0
    ? 0
    : Math.ceil(totalTransactions.value / pagination.value.pageSize),
);

const setPageSize = (size: number): void => {
  pagination.value = { pageIndex: 0, pageSize: size };
  loadTransactions();
};

// Load transactions for selected account
const loadTransactions = async (): Promise<void> => {
  const accountId = activeAccountId.value;
  if (!accountId) {
    transactions.value = [];
    totalTransactions.value = 0;
    return;
  }

  isLoadingTransactions.value = true;

  try {
    const req: GetTransactionsRequest = {
      page: pagination.value.pageIndex + 1,
      pageSize: pagination.value.pageSize,
    };

    const res: PagedResult<TransactionDto> = await atmService.getTransactions(
      accountId,
      req,
    );

    transactions.value = res.items;
    totalTransactions.value = res.totalCount;
  } catch (err) {
    errorMessage.value = extractApiError(err);
  } finally {
    isLoadingTransactions.value = false;
  }
};

// 🔁 When transactions account changes, reset to first page + reload
watch(transactionsAccountId, (): void => {
  pagination.value.pageIndex = 0;
  void loadTransactions();
});

// Initial load
onMounted(async (): Promise<void> => {
  await loadAccounts();

  // Default some sensible IDs
  if (accounts.value.length > 0) {
    const firstId = accounts.value[0].id;
    if (!fromAccountId.value) fromAccountId.value = firstId;
    if (!depositAccountId.value) depositAccountId.value = firstId;
    if (!withdrawAccountId.value) withdrawAccountId.value = firstId;
    if (!transactionsAccountId.value) transactionsAccountId.value = firstId;
  }

  pagination.value.pageIndex = 0;
  await loadTransactions();
});
</script>
