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
  standalone: true,
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb implements OnInit {
  filterForm: FormGroup;
  movies: MovieResponse[];
  genres: GenreResponse[] = [];
  reviewsTitle: String[] = [];
  sortFields = [
    { name: 'Tytuł (A-Z)', value: 'Title|false' }, 
    { name: 'Ocena (najniższa)', value: 'average|false' }, 
    { name: 'Rok Wydania (najstarsze)', value: 'releaseDate|false' },
    { name: 'Tytuł (Z-A)', value: 'Title|true' },
    { name: 'Ocena (najwyższa)', value: 'average|true' },
    { name: 'Rok Wydania (najnowsze)', value: 'releaseDate|true' },
    ];
  constructor(private fb: FormBuilder,private cdr: ChangeDetectorRef,private movieService: MovieService,private genreService: GenreService,private reviewService: ReviewService) {
  this.filterForm = this.fb.group({
      TitleSearch: [null],
      MinRating: [null],
      ReleaseYear: [null],
      genreName: [null],
      DirectorName: [null],
      DirectorSurname: [null],
      SortByField: [null],
      IsDescending: [false]
    });
    this.movies = [];
  }
  ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(300)
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
    });
  }

  loadMoviesByFilter(query: MovieQuery): void {
    this.movieService.getMoviesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano filmy z filtrami:', data);
            this.movies = data;
            console.log('Filmy po zastosowaniu filtrów:', this.movies);
            this.cdr.detectChanges();
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
