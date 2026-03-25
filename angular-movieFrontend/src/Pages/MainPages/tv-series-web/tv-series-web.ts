import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { MovieQuery } from '../../../Data/Request/MovieQuery';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { MovieResponse } from '../../../Data/Response/MovieResponse';
import { GenreService } from '../../../Services/GenreService';
import { MovieService } from '../../../Services/MovieService';
import { ReviewService } from '../../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { TvSeriesResponse } from '../../../Data/Response/TvSeriesResponse';
import { TvSeriesService } from '../../../Services/TvSeriesService';

@Component({
  selector: 'app-tv-series-web',
  imports: [RouterLink,ReactiveFormsModule],
  templateUrl: './tv-series-web.html',
  styleUrl: './tv-series-web.css',
})
export class TvSeriesWeb {
filterForm: FormGroup;
  TvSeries: TvSeriesResponse[] = [];
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
  constructor(private fb: FormBuilder,private cdr: ChangeDetectorRef,private tvSeriesService: TvSeriesService,private genreService: GenreService,private reviewService: ReviewService) {
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
  }
  ngOnInit(): void {
    this.filterForm.valueChanges
      .pipe(
        debounceTime(300)
      )
      .subscribe((query: MovieQuery) => {
        this.loadTvSeriesByFilter(query);
      });
    this.loadTvSeries();
    this.loadGenres();
    this.GetTheLastestReviews();
  }

  loadTvSeries(): void {
    this.tvSeriesService.getTvSeries().subscribe((data) => {
      this.TvSeries = data;
      this.cdr.detectChanges();
    });
  }
  loadTvSeriesByFilter(query: MovieQuery): void {
    this.tvSeriesService.getMoviesByFilter(query).subscribe({
        next: (data) => {
          console.log('Załadowano filmy z filtrami:', data);
            this.TvSeries = data;
        },
        error: (err) => {
            console.error('Błąd ładowania filmów:', err);
            this.TvSeries = [];
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
