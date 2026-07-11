import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { MovieService } from '../../Services/MovieService';
import { TvSeriesService } from '../../Services/TvSeriesService';
import { GameService } from '../../Services/GameService';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit {
  mediaForm!: FormGroup;
  currentType: 'film' | 'serial' | 'gra' = 'film';

  constructor(
    private fb: FormBuilder, 
    private movieService: MovieService, 
    private tvSeriesService: TvSeriesService, 
    private gameService: GameService
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.mediaForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      language: [''],
      releaseDate: [new Date().toISOString().substring(0, 16)],
      genre: this.fb.group({
        name: ['', Validators.required]
      }),

      director: this.fb.group({
        name: [''],
        surname: ['']
      }),
      duration: [''],
      distributionType: [''],
      status: [''],

      seasons: [0],
      episodes: [0],
      network: [''],

      developer: [''],
      engine: [''],
      pegiRating: [0],
      platforms: this.fb.array([]),
      supportsCrossPlay: [false],
      gameStatus: ['']
    });
  }

  onMediaTypeChange(type: 'film' | 'serial' | 'gra'): void {
    this.currentType = type;
  }

  onPlatformChange(event: any): void {
    const platforms = this.mediaForm.get('platforms') as FormArray;
    if (event.target.checked) {
      platforms.push(this.fb.control(event.target.value));
    } else {
      const index = platforms.controls.findIndex(x => x.value === event.target.value);
      if (index !== -1) {
        platforms.removeAt(index);
      }
    }
  }

  onSubmit(): void {
    if (this.mediaForm.invalid) return;

    const raw = this.mediaForm.value;
    const formattedDate = new Date(raw.releaseDate).toISOString();

    if (this.currentType === 'film') {
      

      const minutesInput = Number(raw.duration);
      let formattedDuration: string;

      if (!raw.duration || isNaN(minutesInput) || minutesInput <= 0) {
        throw new Error('Nieprawidłowa wartość czasu trwania filmu. Wprowadź liczbę minut większą od zera.');
      } else {
        const hours = Math.floor(minutesInput / 60).toString().padStart(2, '0');
        const minutes = (minutesInput % 60).toString().padStart(2, '0');
        formattedDuration = `${hours}:${minutes}:00`;
      }

      const moviePayload = {
          title: raw.title,
          description: raw.description,
          language: raw.language,
          releaseDate: formattedDate,
          genre: raw.genre,
          director: raw.director,
          duration: formattedDuration,
          distributionType: raw.distributionType || "Kino",
          status: raw.status || "Wydany"
      };

      console.log('Wysyłam FILM do .NET API (po konwersji):', moviePayload);

      this.movieService.addMovie(moviePayload).subscribe({
        next: (response) => console.log('Film dodany pomyślnie:', response),
        error: (error) => console.error('Błąd podczas dodawania filmu:', error.error || error)
      });
    }
    
    else if (this.currentType === 'gra') {
      const gamePayload = {
        title: raw.title,
        description: raw.description,
        language: raw.language,
        releaseDate: formattedDate,
        genre: raw.genre,
        developer: raw.developer,
        engine: raw.engine,
        pegiRating: raw.pegiRating,
        platforms: raw.platforms,
        supportsCrossPlay: raw.supportsCrossPlay,
        gameStatus: raw.gameStatus || "Wydana"
      };

      console.log('Wysyłam GRĘ do .NET API:', gamePayload);

      this.gameService.addGame(gamePayload).subscribe({
        next: (response) => console.log('Gra dodana pomyślnie:', response),
        error: (error) => console.error('Błąd podczas dodawania gry:', error.error || error)
      });
    }
    
    else if (this.currentType === 'serial') {
      const tvSeriesPayload = {
        title: raw.title,
        description: raw.description,
        language: raw.language,
        releaseDate: formattedDate,
        genre: raw.genre,
        seasons: raw.seasons,
        episodes: raw.episodes,
        network: raw.network,
        status: raw.status || "Wydany"
      };

      console.log('Wysyłam SERIAL do .NET API:', tvSeriesPayload);

      this.tvSeriesService.addTvSeries(tvSeriesPayload).subscribe({
        next: (response) => console.log('Serial dodany pomyślnie:', response),
        error: (error) => console.error('Błąd podczas dodawania serialu:', error.error || error)
      });
    }
  }
}