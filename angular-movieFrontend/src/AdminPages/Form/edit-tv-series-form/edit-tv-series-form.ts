import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { GenreService } from '../../../Services/GenreService';
import { TvSeriesService } from '../../../Services/TvSeriesService';

@Component({
  selector: 'app-edit-tv-series-form',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './edit-tv-series-form.html',
  styleUrl: './edit-tv-series-form.css',
})
export class EditTvSeriesForm {
  editForm!: FormGroup;
  seriesId!: string;
  genres: any[] = [];
  isLoading = true;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private seriesService: TvSeriesService,
    private genreService: GenreService
  ) {}

  ngOnInit(): void {
    this.seriesId = this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.loadData();
  }

  private initForm(): void {
    this.editForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      genreId: ['', Validators.required],
      releaseDate: ['', Validators.required],
      language: ['', Validators.required],
      seasons: [0, [Validators.required, Validators.min(1)]],
      episodes: [0, [Validators.required, Validators.min(1)]],
      network: ['', Validators.required],
      status: ['', Validators.required]
    });
  }

  private loadData(): void {
    this.genreService.getGenres().subscribe({
      next: (genres) => {
        this.genres = genres;
      },
      error: (err) => console.error('Błąd pobierania gatunków:', err)
    });

    this.seriesService.getTvSeriesById(this.seriesId).subscribe({
      next: (series) => {
        const formattedDate = series.releaseDate ? new Date(series.releaseDate).toISOString().substring(0, 10) : '';

        this.editForm.patchValue({
          title: series.title,
          description: series.description,
          genreId: series.GenreResponse?.id || series.genreResponse?.id || '',
          releaseDate: formattedDate,
          language: series.language,
          seasons: series.seasons,
          episodes: series.episodes,
          network: series.network,
          status: series.status
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Błąd ładowania danych:', err);
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    const updateCommand = {
      id: this.seriesId,
      ...this.editForm.value
    };

    this.seriesService.updateSeries(this.seriesId, updateCommand).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/adminDashboard']);
      },
      error: (err) => {
        console.error('Błąd podczas edycji:', err);
        this.isSubmitting = false;
      }
    });
  }
}
