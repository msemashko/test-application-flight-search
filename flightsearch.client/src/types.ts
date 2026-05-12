export interface Airport {
  code: string
  name: string
  city: string
  country: string
}

export interface Flight {
  flightNumber: string
  origin: string
  destination: string
  departureTime: string
  arrivalTime: string
  price: number
  availableSeats: number
}
