<script setup lang="ts">
import type { Airport } from '../types'

defineProps<{
  label: string
  modelValue: string
  airports: Airport[]
  disabled?: boolean
  placeholder?: string
  loading?: boolean
}>()

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()
</script>

<template>
  <div>
    <label class="form-label">
      {{ label }}
      <span v-if="loading" class="spinner-border spinner-border-sm ms-1"></span>
    </label>
    <select
      class="form-select"
      :value="modelValue"
      :disabled="disabled"
      @change="emit('update:modelValue', ($event.target as HTMLSelectElement).value)"
    >
      <option value="" disabled>{{ placeholder ?? 'Select...' }}</option>
      <option v-for="a in airports" :key="a.code" :value="a.code">
        {{ a.code }} — {{ a.city }} ({{ a.name }})
      </option>
    </select>
  </div>
</template>
