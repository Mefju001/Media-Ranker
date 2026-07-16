import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule } from '@angular/forms';
import { MovieService } from '../../Services/MovieService';
import { TvSeriesService } from '../../Services/TvSeriesService';
import { AdminService } from '../../Services/AdminService';
import { GameService } from '../../Services/GameService';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { TvSeriesResponse } from '../../Data/Response/TvSeriesResponse';
import { GameResponse } from '../../Data/Response/GameResponse';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})

export class AdminDashboard implements OnInit {
  moviesCount: number = 0;
  tvSeriesCount: number = 0;
  gamesCount: number = 0;
  movies: MovieResponse[] = [];
  tvSeries: TvSeriesResponse[] = [];
  games: GameResponse[] = [];
  mediaForm!: FormGroup;
  currentType: 'film' | 'serial' | 'gra' = 'film';
  statusOptions: Record<'film' | 'serial' | 'gra', { value: string; label: string }[]> = {
      film: [
        { value: 'Announced', label: 'Zapowiedziany' },
        { value: 'InProduction', label: 'W produkcji' },
        { value: 'Released', label: 'Wydany' },
        { value: 'Cancelled', label: 'Anulowany' }
      ],
      gra: [
        { value: 'Announced', label: 'Zapowiedziana' },
        { value: 'EarlyAccess', label: 'Wczesny dostęp' },
        { value: 'Released', label: 'Wydana' },
        { value: 'Delayed', label: 'Opóźniona' },
        { value: 'Cancelled', label: 'Anulowana' }
      ],
      serial: [
        { value: 'Announced', label: 'Zapowiedziany' },
        { value: 'Ongoing', label: 'W trakcie emisji' },
        { value: 'Ended', label: 'Zakończony' },
        { value: 'Canceled', label: 'Anulowany' }
      ]
    };
  platformOptions = [
    { value: 'PC', label: 'PC' },
    { value: 'PlayStation5', label: 'PlayStation 5' },
    { value: 'PlayStation4', label: 'PlayStation 4' },
    { value: 'XboxSeries', label: 'Xbox Series X/S' },
    { value: 'XboxOne', label: 'Xbox One' },
    { value: 'NintendoSwitch', label: 'Nintendo Switch' },
    { value: 'Mobile', label: 'Urządzenia mobilne' },
    { value: 'SteamDeck', label: 'Steam Deck' },
    { value: 'VR', label: 'Virtual Reality' },
    { value: 'WebBrowser', label: 'Przeglądarka WWW' }
  ];
  distributionTypeOptions = [
    { value: 'Cinema', label: 'Kino' },
    { value: 'Streaming', label: 'Streaming / VOD' },
    { value: 'DirectToVideo', label: 'Wydanie bezpośrednie (DVD/Blu-ray)' }
  ];
  users: any[] = [];
  constructor(
    private fb: FormBuilder, 
    private movieService: MovieService, 
    private tvSeriesService: TvSeriesService,
    private gameService: GameService, 
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.getAllNumbers();
    this.loadMovies();
    this.loadGames();
    this.loadTvSeries();
    this.getAllUsers();
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
      distributionType: ['Cinema'],
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
          distributionType: raw.distributionType || "Cinema",
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
  loadMovies(): void {
    this.movieService.getMovies().subscribe((data) => {
      console.log('Pobrano filmy:', data);
      this.movies = data;
    });
  }
  loadTvSeries(): void {
    this.tvSeriesService.getTvSeries().subscribe((data) => {
      this.tvSeries = data;
    });
  }
  loadGames(): void {
    this.gameService.getGames().subscribe((data) => {
      this.games = data;
    });
  }
  getAllUsers(): void {
    this.adminService.getAllUsers().subscribe({
      next: (users) => {
        this.users = users;
      },
      error: (error) => console.error('Błąd podczas pobierania użytkowników:', error)
    });
  }
  getAllNumbers(): void {
    this.adminService.getAllNumbers().subscribe({
      next: (numbers) => {
        this.moviesCount = numbers.numberOfMovies;
        this.tvSeriesCount = numbers.numberOfTvSeries;
        this.gamesCount = numbers.numberOfGames;
      },
      error: (error) => console.error('Błąd podczas pobierania liczby elementów:', error)
    });
  }
  deleteMovie(id: string): void{
    this.movieService.deleteMovie(id).subscribe({})
  }
  deleteTvSeries(id: string): void{
    this.tvSeriesService.deleteSeries(id).subscribe({})
  }
  deleteGame(id: string): void{
    this.gameService.deleteGame(id).subscribe({})
  }
}