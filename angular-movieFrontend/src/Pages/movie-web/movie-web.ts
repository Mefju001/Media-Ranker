import { ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { MovieQuery } from '../../Data/Request/MovieQuery';
import { MovieService } from '../../Services/MovieService';
import { GenreService } from '../../Services/GenreService';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { GenreResponse } from '../../Data/Response/GenreResponse';
import { ReviewService } from '../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
@Component({
  selector: 'app-movie-web',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './movie-web.html',
  styleUrl: './movie-web.css'
})
export class MovieWeb implements OnInit {
  filterForm!: FormGroup;
  movies: MovieResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: String[] = [];
  sortFields = [
          { name: 'Tytuł', value: 'Title' },
          { name: 'Ocena', value: 'AverageRating' },
          { name: 'Rok Wydania', value: 'ReleaseYear' }
      ];
  constructor(private fb: FormBuilder,private cdr: ChangeDetectorRef,private movieService: MovieService,private genreService: GenreService,private reviewService: ReviewService) {
  }
  ngOnInit(): void {
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
  loadMoviesByFilter(): void {
    const searchValues = this.filterForm.value;
    this.movieService.getMoviesByFilter(searchValues).subscribe({
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
onSubmit(): void {
  Promise.resolve().then(() => this.loadMoviesByFilter());
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
