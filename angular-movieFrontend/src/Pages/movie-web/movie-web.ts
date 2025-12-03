import { ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { MovieQuery } from '../../Data/Request/MovieQuery';
import { MovieService } from '../../Services/MovieService';
import { GenreService } from '../../Services/GenreService';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { GenreResponse } from '../../Data/Response/GenreResponse';
import { ReviewService } from '../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs';
@Component({
  selector: 'app-movie-web',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb implements OnInit {
  filterForm: FormGroup;
  movies: MovieResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: String[] = [];
  sortFields = [
          { name: 'Tytuł', value: 'Title' },
          { name: 'Ocena', value: 'average' },
          { name: 'Rodzaj', value: 'genre' },
          { name: 'Rok Wydania', value: 'releaseDate' }
      ];
  constructor(private fb: FormBuilder,private cdr: ChangeDetectorRef,private movieService: MovieService,private genreService: GenreService,private reviewService: ReviewService) {
  this.filterForm = this.fb.group({
      TitleSearch: [''],
      MinRating: [null],
      ReleaseYear: [null],
      genreName: [''],
      DirectorName: [''],
      DirectorSurname: [''],
      SortByField: [''],
      IsDescending: [false]
    });
  }
  ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(300), // Poczekaj 300ms po ostatniej zmianie
        distinctUntilChanged((prev, curr) => JSON.stringify(prev) === JSON.stringify(curr))
      )
      .subscribe((query: MovieQuery) => {
        this.loadMoviesByFilter(query);
      });
    this.loadMovies();
    this.loadGenres();
    this.GetTheLastestReviews();
  }

  loadMovies(): void {
    this.movieService.getMovies().subscribe((data) => {
      this.movies = data;
      this.cdr.detectChanges();
    });
  }
  loadMoviesByFilter(query: MovieQuery): void {
    this.movieService.getMoviesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano filmy z filtrami:', data);
            this.movies = data;
        },
        error: (err) => {
            console.error('Błąd ładowania filmów:', err);
            this.movies = [];
        }
    });
  }

loadGenres(): void {
  this.genreService.getGenres().subscribe((data) => {
    this.genres = data;
  });
}
  GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
  }
}
