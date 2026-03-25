import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface GameResponse {
  developer: string;
  platform: string;
  id: number;
  title: string;
  description: string;
  genre: GenreResponse;
  releaseDate: string; 
  language: string;
  mediaStats: MediaStatsResponse;
  reviews: ReviewResponse[];
}