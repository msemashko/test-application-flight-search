<script setup lang="ts">
defineProps<{
  modelValue: 'oneway' | 'return'
}>()

const emit = defineEmits<{
  'update:modelValue': [value: 'oneway' | 'return']
}>()
</script>

<template>
  <div class="btn-group mb-3" role="group">
    <input
      type="radio"
      class="btn-check"
      id="trip-oneway"
      value="oneway"
      :checked="modelValue === 'oneway'"
      @change="emit('update:modelValue', 'oneway')"
    />
    <label class="btn btn-outline-primary" for="trip-oneway">One-way</label>

    <input
      type="radio"
      class="btn-check"
      id="trip-return"
      value="return"
      :checked="modelValue === 'return'"
      @change="emit('update:modelValue', 'return')"
    />
    <label class="btn btn-outline-primary" for="trip-return">Return</label>
  </div>
</template>
