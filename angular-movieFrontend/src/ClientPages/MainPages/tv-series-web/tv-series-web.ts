import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { GenreResponse } from '../../../Data/Response/GenreResponse';
import { ReviewService } from '../../../Services/ReviewService';
import { RouterLink } from '@angular/router';
import { TvSeriesResponse } from '../../../Data/Response/TvSeriesResponse';
import { TvSeriesService } from '../../../Services/TvSeriesService';
import { CommonModule } from '@angular/common';
import { TvSeriesQuery } from '../tv-series-web/TvSeriesQuery';

@Component({
  selector: 'app-tv-series-web',
  imports: [RouterLink,ReactiveFormsModule,CommonModule],
  templateUrl: './tv-series-web.html',
  styleUrl: './tv-series-web.css',
})
export class TvSeriesWeb {
  private readonly fb = inject(FormBuilder);
  private readonly tvSeriesService = inject(TvSeriesService);
  private readonly reviewService = inject(ReviewService);
  readonly filterForm = this.fb.group({
      TitleSearch: [null],
      MinRating: [null],
      ReleaseYear: [null],
      genreName: [null],
      status: [null],
      SortByField: [null as { sortBy: string; isDescending: boolean } | null]
    });
  TvSeries: TvSeriesResponse[] = [];
  genres: GenreResponse[] = [];
  reviewsTitle: string[] = [];
  sortFields = [
    { name: 'Tytuł (A-Z)', sortBy: 'Title', isDescending: false }, 
    { name: 'Ocena (najniższa)', sortBy: 'average', isDescending: false }, 
    { name: 'Rok Wydania (najstarsze)', sortBy: 'releaseDate', isDescending: false },
    { name: 'Tytuł (Z-A)', sortBy: 'Title', isDescending: true },
    { name: 'Ocena (najwyższa)', sortBy: 'average', isDescending: true },
    { name: 'Rok Wydania (najnowsze)', sortBy: 'releaseDate', isDescending: true },
    ];
    statusOptions = {
      tvSeries: [
        { value: 'Announced', label: 'Zapowiedziany' },
        { value: 'Ongoing', label: 'W trakcie emisji' },
        { value: 'Ended', label: 'Zakończony' },
        { value: 'Canceled', label: 'Anulowany' }
      ]
    };
  ngOnInit(): void {
    this.loadTvSeries();
    this.loadGenres();
    this.GetTheLastestReviews();
  }
  onFilter(): void {
    const rawValue = this.filterForm.getRawValue();
    const selectedSortField = rawValue.SortByField;
    const tvSeriesQuery: TvSeriesQuery = {
      TitleSearch: rawValue.TitleSearch,
      MinRating: rawValue.MinRating,
      ReleaseYear: rawValue.ReleaseYear,
      genreName: rawValue.genreName,
      status: rawValue.status,

      SortByField: selectedSortField ? selectedSortField.sortBy : null,
      IsDescending: selectedSortField ? selectedSortField.isDescending : false,
    };
    this.loadTvSeriesByFilter(tvSeriesQuery);
  }
  onReset(): void {
    this.filterForm.reset();
    this.loadTvSeries();
  }
  loadTvSeries(): void {
    this.tvSeriesService.getTvSeries().subscribe((data) => {
      this.TvSeries = data;
    });
  }
  loadTvSeriesByFilter(query: TvSeriesQuery): void {
    this.tvSeriesService.getTvSeriesByFilter(query).subscribe({
        next: (data) => {
            this.TvSeries = data;
        },
        error: (err) => {
            console.error('Błąd ładowania seriali:', err);
            this.TvSeries = [];
        }
    });
  }
  loadGenres(): void {
    this.tvSeriesService.GetGenres().subscribe((data) => {
      this.genres = data;
    });
  }
  GetTheLastestReviews(): void {
    this.reviewService.getTheLastestReviews().subscribe((data) => {
      this.reviewsTitle = data;
    });
  }
}
