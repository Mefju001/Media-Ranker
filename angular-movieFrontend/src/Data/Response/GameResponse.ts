import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface GameResponse {
  id: string;
  title: string;
  description: string;
  genreResponse: GenreResponse;
  releaseDate: string; 
  language: string;
  reviews: ReviewResponse[] | null;
  mediaStatsResponse: MediaStatsResponse;
  developer: string;
  engine: string | null;
  pegiRating: number;
  supportsCrossPlay: boolean;
  platforms: string[];
  $type?:'game';
}