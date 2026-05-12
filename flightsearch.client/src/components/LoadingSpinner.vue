<script setup lang="ts">
defineProps<{
  message?: string
}>()
</script>

<template>
  <div class="d-flex flex-column align-items-center justify-content-center py-5">
    <div class="spinner-border text-primary mb-3" style="width: 3rem; height: 3rem" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
    <p class="text-muted">{{ message ?? 'Loading...' }}</p>
  </div>
</template>
