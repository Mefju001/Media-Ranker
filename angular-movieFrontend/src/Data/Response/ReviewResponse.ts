export interface Review {
  id: number;
  username: string;
  rating: number; // Assuming 0-10 or 1-10 scale
  comment: string;
}