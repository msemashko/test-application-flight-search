import type { Airport, Flight } from './types'

const BASE_URL = import.meta.env.DEV ? 'http://localhost:5003/api' : '/api'

async function handleResponse<T>(response: Response): Promise<T> {
  if (!response.ok) {
    const body = await response.json().catch(() => null)
    throw new Error(body?.error ?? `Server error (${response.status})`)
  }
  return response.json()
}

export async function fetchAirports(): Promise<Airport[]> {
  const response = await fetch(`${BASE_URL}/airports`)
  return handleResponse<Airport[]>(response)
}

export async function fetchDestinations(origin: string): Promise<Airport[]> {
  const response = await fetch(`${BASE_URL}/airports/destinations/${origin}`)
  if (response.status === 404) return []
  return handleResponse<Airport[]>(response)
}

export async function searchFlights(
  origin: string,
  destination: string,
  passengers: number,
  departureDate: string,
  returnDate: string,
  tripType: string,
): Promise<Flight[]> {
  const params = new URLSearchParams({
    origin,
    destination,
    passengers: passengers.toString(),
    tripType,
  })
  if (departureDate) params.set('departureDate', departureDate)
  if (returnDate && tripType === 'return') params.set('returnDate', returnDate)
  const response = await fetch(`${BASE_URL}/flights/search?${params}`)
  return handleResponse<Flight[]>(response)
}
