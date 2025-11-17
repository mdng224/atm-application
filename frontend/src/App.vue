<template>
  <!-- HEADER -->
  <header class="bg-gray-900 shadow-sm">
    <nav
      aria-label="Global"
      class="mx-auto flex max-w-7xl items-center justify-between p-6 lg:px-8"
    >
      <!-- Logo / Brand -->
      <div class="flex items-center gap-3 lg:flex-1">
        <RouterLink to="/" class="flex items-center gap-2">
          <img
            src="https://tailwindcss.com/plus-assets/img/logos/mark.svg?color=indigo&shade=500"
            alt="Logo"
            class="h-8 w-auto"
          />
          <span class="text-sm font-semibold text-white">ATM Application</span>
        </RouterLink>
      </div>

      <!-- Right side empty (no auth, single-user app) -->
      <div class="hidden lg:flex lg:flex-1 lg:justify-end">
        <!-- Intentionally left blank -->
      </div>
    </nav>
  </header>

  <!-- Page wrapper -->
  <div class="min-h-screen bg-slate-950 text-slate-100">
    <main class="flex min-h-screen flex-col overflow-x-hidden transition-all duration-200">
      <section class="flex-1 p-6">
        <RouterView />
      </section>
    </main>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router';
</script>
