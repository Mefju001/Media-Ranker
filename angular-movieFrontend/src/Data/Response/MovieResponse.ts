import { DirectorResponse } from "./DirectorResponse";
import { GenreResponse } from "./GenreResponse";
import { MediaStatsResponse } from "./MediaStatsResponse";
import { ReviewResponse } from "./ReviewResponse";

export interface MovieResponse{
id: number;
  title: string;
  description: string;
  duration: string; // HH:MM:SS format
  isCinemaRelease: boolean;
  releaseDate: string; // ISO 8601 date string  
  language: string;

  director: DirectorResponse;
  genre: GenreResponse;
  mediaStats: MediaStatsResponse;
  reviews: ReviewResponse[];
}