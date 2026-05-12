<script setup lang="ts">
defineProps<{
  message: string
}>()
</script>

<template>
  <div class="alert alert-danger mt-3">{{ message }}</div>
</template>
