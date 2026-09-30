import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { GameService } from '../../../Services/GameService';
import { GenreService } from '../../../Services/GenreService';
import { GameResponse } from '../../../Data/Response/GameResponse';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-game-form',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './edit-game-form.html',
  styleUrl: './edit-game-form.css',
})
export class EditGameForm implements OnInit{
  editForm!: FormGroup;
  gameId!: string;
  genres: any[] = [];
  isLoading = true;
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private gameService: GameService,
    private genreService: GenreService
  ) {}

  ngOnInit(): void {
    this.gameId = this.route.snapshot.paramMap.get('id')!;
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
      developer: ['', Validators.required],
      engine: [''],
      pegiRating: [3, [Validators.required, Validators.min(3), Validators.max(18)]],
      supportsCrossPlay: [false],
      platforms: ['', Validators.required]
    });
  }

  private loadData(): void {
    this.genreService.getGenres().subscribe({
      next: (genres) => this.genres = genres,
      error: (err) => console.error('Błąd pobierania gatunków:', err)
    });

    this.gameService.getGameById(this.gameId).subscribe({
      next: (game: GameResponse) => {
        const formattedDate = game.releaseDate ? new Date(game.releaseDate).toISOString().substring(0, 10) : '';
        

        const platformsString = game.platforms ? game.platforms.join(', ') : '';

        this.editForm.patchValue({
          title: game.title,
          description: game.description,
          genreId: game.genreResponse?.id || '',
          releaseDate: formattedDate,
          language: game.language,
          developer: game.developer,
          engine: game.engine,
          pegiRating: game.pegiRating,
          supportsCrossPlay: game.supportsCrossPlay,
          platforms: platformsString
        });
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Błąd ładowania danych gry:', err);
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

    
    const platformsRaw = this.editForm.value.platforms as string;
    const platformsArray = platformsRaw.split(',').map(p => p.trim()).filter(p => p.length > 0);

    const updateCommand = {
      id: this.gameId,
      ...this.editForm.value,
      platforms: platformsArray
    };

    this.gameService.updateGame(this.gameId, updateCommand).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/games', this.gameId]);
      },
      error: (err) => {
        console.error('Błąd podczas edycji gry:', err);
        this.isSubmitting = false;
      }
    });
  }
}
