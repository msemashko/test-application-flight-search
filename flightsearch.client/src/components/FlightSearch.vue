<script setup lang="ts">
  import { ref, watch, onMounted } from 'vue'
  import type { Airport, Flight } from '../types'
  import { fetchAirports, fetchDestinations, searchFlights } from '../api'
  import TripTypeToggle from './TripTypeToggle.vue'
  import AirportSelect from './AirportSelect.vue'
  import FlightResults from './FlightResults.vue'
  import ErrorAlert from './ErrorAlert.vue'
  import LoadingSpinner from './LoadingSpinner.vue'

  const tripType = ref<'oneway' | 'return'>('oneway')
  const origins = ref<Airport[]>([])
  const destinations = ref<Airport[]>([])
  const selectedOrigin = ref('')
  const selectedDestination = ref('')
  const passengers = ref(1)
  const departureDate = ref('')
  const returnDate = ref('')

  const flights = ref<Flight[]>([])
  const returnFlights = ref<Flight[]>([])
  const searched = ref(false)
  const searchedOrigin = ref('')
  const searchedDestination = ref('')
  const searchedPassengers = ref(1)
  const searchedTripType = ref<'oneway' | 'return'>('oneway')
  const initializing = ref(true)
  const searchLoading = ref(false)
  const loadingDestinations = ref(false)
  const error = ref('')

  async function loadAirports(retries = 5, delayMs = 1000) {
    error.value = ''
    for (let attempt = 1; attempt <= retries; attempt++) {
      try {
        origins.value = await fetchAirports()
        initializing.value = false
        return
      } catch (e: any) {
        if (attempt === retries) {
          error.value = e.message
          initializing.value = false
        } else {
          await new Promise((r) => setTimeout(r, delayMs))
        }
      }
    }
  }

  onMounted(() => {
    loadAirports()
  })

  watch(selectedOrigin, async (code) => {
    selectedDestination.value = ''
    destinations.value = []
    flights.value = []
    searched.value = false
    if (!code) return

    loadingDestinations.value = true
    error.value = ''
    try {
      destinations.value = await fetchDestinations(code)
    } catch (e: any) {
      error.value = e.message
    } finally {
      loadingDestinations.value = false
    }
  })

  async function onSearch() {
    if (!selectedOrigin.value || !selectedDestination.value) return

    searchLoading.value = true
    error.value = ''
    searched.value = false
    flights.value = []
    returnFlights.value = []
    searchedOrigin.value = selectedOrigin.value
    searchedDestination.value = selectedDestination.value
    searchedPassengers.value = passengers.value
    searchedTripType.value = tripType.value

    try {
      const outbound = searchFlights(
        selectedOrigin.value,
        selectedDestination.value,
        passengers.value,
        departureDate.value,
        returnDate.value,
        tripType.value,
      )

      if (tripType.value === 'return' && returnDate.value) {
        const [outResults, retResults] = await Promise.all([
          outbound,
          searchFlights(
            selectedDestination.value,
            selectedOrigin.value,
            passengers.value,
            returnDate.value,
            '',
            'oneway',
          ),
        ])
        flights.value = outResults
        returnFlights.value = retResults
      } else {
        flights.value = await outbound
      }
      searched.value = true
    } catch (e: any) {
      error.value = e.message
    } finally {
      searchLoading.value = false
    }
  }
</script>

<template>
  <div class="container py-4" style="max-width: 900px">
    <h1 class="mb-4">✈️ Flight Search</h1>

    <LoadingSpinner v-if="initializing" message="Connecting to server..." />

    <template v-else>
      <ErrorAlert v-if="error && !origins.length" :message="error" />

      <template v-else>
        <TripTypeToggle v-model="tripType" />

        <div class="row g-3 mb-3">
          <div class="col-md-6">
            <AirportSelect label="Origin"
                           v-model="selectedOrigin"
                           :airports="origins"
                           placeholder="Select origin" />
          </div>

          <div class="col-md-6">
            <AirportSelect label="Destination"
                           v-model="selectedDestination"
                           :airports="destinations"
                           :disabled="loadingDestinations || !selectedOrigin"
                           :loading="loadingDestinations"
                           :placeholder="
                selectedOrigin
                  ? loadingDestinations
                    ? 'Loading...'
                    : destinations.length
                      ? 'Select destination'
                      : 'No destinations'
                  : 'Pick origin first'
              " />
          </div>

          <div class="col-md-4">
            <label class="form-label">Departure date</label>
            <input type="date" class="form-control" v-model="departureDate" />
          </div>

          <div class="col-md-4" v-if="tripType === 'return'">
            <label class="form-label">Return date</label>
            <input type="date" class="form-control" v-model="returnDate" />
          </div>

          <div class="col-md-4">
            <label class="form-label">Passengers</label>
            <input type="number" class="form-control" v-model.number="passengers" min="1" max="9" />
          </div>
        </div>

        <button class="btn btn-primary"
                :disabled="!selectedOrigin || !selectedDestination || searchLoading"
                @click="onSearch">
          <span v-if="searchLoading" class="spinner-border spinner-border-sm me-1"></span>
          Search Flights
        </button>

        <ErrorAlert v-if="error" :message="error" />

        <FlightResults v-if="searched && !error"
                       :flights="flights"
                       :origin="searchedOrigin"
                       :destination="searchedDestination"
                       :passengers="searchedPassengers"
                       :label="searchedTripType === 'return' ? 'Outbound' : undefined" />

        <FlightResults v-if="searched && !error && searchedTripType === 'return'"
                       :flights="returnFlights"
                       :origin="searchedDestination"
                       :destination="searchedOrigin"
                       :passengers="searchedPassengers"
                       label="Return" />
      </template>
    </template>
  </div>
</template>
