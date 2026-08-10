import { Component, inject, OnInit} from '@angular/core';
import { MovieQuery } from './MovieQuery';
import { MovieService } from '../../../Services/MovieService';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MovieResponse } from '../../../Data/Response/MovieResponse';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { ReviewService } from '../../../Services/ReviewService';
@Component({
  selector: 'app-movie-web',
  standalone: true,
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly movieService = inject(MovieService);
  private readonly reviewService = inject(ReviewService);
  filterForm = this.fb.group({
      TitleSearch: [null],
      MinRating: [null],
      ReleaseYear: [null],
      genreName: [null],
      DirectorName: [null],
      DirectorSurname: [null],
      SortByField: [null as { sortBy: string; isDescending: boolean } | null]
    });
  movies: MovieResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: string[] = [];
  sortFields = [
    { name: 'Tytuł (A-Z)', sortBy: 'Title' , isDescending: false }, 
    { name: 'Ocena (najniższa)', sortBy: 'Rating', isDescending: false }, 
    { name: 'Rok Wydania (najstarsze)', sortBy: 'Date', isDescending: false },
    { name: 'Tytuł (Z-A)', sortBy: 'Title', isDescending: true },
    { name: 'Ocena (najwyższa)', sortBy: 'Rating', isDescending: true },
    { name: 'Rok Wydania (najnowsze)', sortBy: 'Date', isDescending: true },
    ];
ngOnInit(): void {
    this.loadMovies();
    this.loadGenres();
    this.GetTheLastestReviews();
  }
onFilter(): void {
    const rawValue = this.filterForm.getRawValue();
    const selectedSortField = rawValue.SortByField;
    const movieQuery: MovieQuery = {
      TitleSearch: rawValue.TitleSearch,
      MinRating: rawValue.MinRating,
      ReleaseYear: rawValue.ReleaseYear,
      genreName: rawValue.genreName,
      DirectorName: rawValue.DirectorName,
      DirectorSurname: rawValue.DirectorSurname,

      SortByField: selectedSortField ? selectedSortField.sortBy : null,
      IsDescending: selectedSortField ? selectedSortField.isDescending : false
    };
    this.loadMoviesByFilter(movieQuery);
  }
onReset(): void {
    this.filterForm.reset();
    this.loadMovies();
  }
loadMovies(): void {
    this.movieService.getMovies().subscribe((data) => {
      this.movies = data;
    });
  }
loadMoviesByFilter(query: MovieQuery): void {
    this.movieService.getMoviesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano filmy z filtrami:', data);
            this.movies = data;
            console.log('Filmy po zastosowaniu filtrów:', this.movies);
        },
        error: (err) => {
            console.error('Błąd ładowania filmów:', err);
            this.movies = [];
        }
    });
  }
loadGenres(): void {
  this.movieService.GetGenres().subscribe((data) => {
    this.genres = data;
  });
}
GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
  }
}
