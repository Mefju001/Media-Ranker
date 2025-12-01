import { ChangeDetectorRef, Component } from '@angular/core';
import { MovieService } from '../../Services/MovieService';
import { GenreService } from '../../Services/GenreService';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { GenreResponse } from '../../Data/Response/GenreResponse';
import { ReviewService } from '../../Services/ReviewService';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-movie-web',
  imports: [RouterLink],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb {
  movies: MovieResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: String[] = [];

  constructor(private cdr: ChangeDetectorRef,private movieService: MovieService,private genreService: GenreService,private reviewService: ReviewService) {
    this.loadMovies();
    this.loadGenres();
    this.GetTheLastestReviews();
  }
  loadMovies(): void {
    this.movieService.getMovies().subscribe((data) => {
      this.movies = data;
    });
    this.cdr.detectChanges();
  }
  loadGenres(): void {
    this.genreService.getGenres().subscribe((data) => {
      this.genres = data;
    });
    this.cdr.detectChanges();
  }
  GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
    this.cdr.detectChanges();
  }
}
