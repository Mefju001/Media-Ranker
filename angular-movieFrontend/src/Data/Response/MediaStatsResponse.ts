export interface MediaStatsResponse {
  id: number;
  averageRating: number;
  reviewCount: number;
  lastCalculated: string; // ISO 8601 date string
}