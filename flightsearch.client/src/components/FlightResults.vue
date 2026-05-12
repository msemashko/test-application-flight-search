<script setup lang="ts">
import type { Flight } from '../types'

const props = defineProps<{
  flights: Flight[]
  origin: string
  destination: string
  passengers: number
  label?: string
}>()

function formatDateTime(iso: string): string {
  return new Date(iso).toLocaleString(undefined, {
    dateStyle: 'medium',
    timeStyle: 'short',
  })
}
</script>

<template>
  <div class="mt-4">
    <h5>
      {{ props.label ? props.label + ': ' : '' }}{{ props.origin }} → {{ props.destination }}
      <span class="text-muted fs-6">
        ({{ props.passengers }} passenger{{ props.passengers > 1 ? 's' : '' }})
      </span>
    </h5>

    <div v-if="!flights.length" class="alert alert-info">
      No flights found for the selected criteria.
    </div>

    <div v-else class="table-responsive">
      <table class="table table-striped align-middle">
        <thead>
          <tr>
            <th>Flight</th>
            <th>Departure</th>
            <th>Arrival</th>
            <th>Price</th>
            <th>Seats</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="f in flights" :key="f.flightNumber">
            <td><strong>{{ f.flightNumber }}</strong></td>
            <td>{{ formatDateTime(f.departureTime) }}</td>
            <td>{{ formatDateTime(f.arrivalTime) }}</td>
            <td>${{ f.price.toFixed(2) }}</td>
            <td>{{ f.availableSeats }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
