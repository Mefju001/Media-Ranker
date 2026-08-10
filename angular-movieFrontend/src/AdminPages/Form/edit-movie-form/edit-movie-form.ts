import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { GenreService } from '../../../Services/GenreService';
import { MovieService } from '../../../Services/MovieService';

@Component({
  selector: 'app-edit-movie-form',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './edit-movie-form.html',
  styleUrl: './edit-movie-form.css',
})
export class EditMovieForm {
editForm!: FormGroup;
  movieId!: string;
  genres: any[] = [];
  directors: any[] = [];
  isLoading = true;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private movieService: MovieService,
    private genreService: GenreService,
  ) {}

  ngOnInit(): void {
    this.movieId = this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.loadData();
  }

  private initForm(): void {
    this.editForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(2)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      genreId: ['', Validators.required],
      directorId: ['', Validators.required],
      releaseDate: ['', Validators.required],
      language: ['', Validators.required],
      duration: ['', [Validators.required, Validators.pattern(/^([0-1]?[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$/)]], // Walidacja formatu HH:mm:ss
      distributionType: ['', Validators.required],
      status: ['', Validators.required]
    });
  }

  private loadData(): void {
    this.genreService.getGenres().subscribe({
      next: (genres) => this.genres = genres,
      error: (err) => console.error('Błąd pobierania gatunków:', err)
    });

    this.movieService.getMovieById(this.movieId).subscribe({
      next: (movie) => {
        const formattedDate = movie.releaseDate ? new Date(movie.releaseDate).toISOString().substring(0, 10) : '';

        this.editForm.patchValue({
          title: movie.title,
          description: movie.description,
          genreId: movie.genre?.id || movie.genre.id || '',
          directorId: movie.director?.id || movie.director.id || '',
          releaseDate: formattedDate,
          language: movie.language,
          duration: movie.duration,
          distributionType: movie.distributionType,
          status: movie.status
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Błąd ładowania danych filmu:', err);
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
      id: this.movieId,
      ...this.editForm.value
    };

    this.movieService.updateMovie(this.movieId, updateCommand).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/movies', this.movieId]);
      },
      error: (err) => {
        console.error('Błąd podczas edycji filmu:', err);
        this.isSubmitting = false;
      }
    });
  }
}
