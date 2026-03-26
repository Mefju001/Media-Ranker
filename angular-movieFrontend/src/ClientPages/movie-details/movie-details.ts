import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MovieResponse } from '../../Data/Response/MovieResponse';
import { MovieService } from '../../Services/MovieService';
import { EMPTY } from 'rxjs';
import { switchMap } from 'rxjs/operators';

@Component({
  standalone: true,
  selector: 'app-movie-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.css',
})
export class MovieDetails implements OnInit {
  movie: any;
  movieId:number = 0;
  isLoading: boolean = true;
  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private cdr: ChangeDetectorRef
  ){}
  ngOnInit(): void{
     this.route.paramMap.subscribe(params => {
      const idString = params.get('id');
      const id = idString ? Number(idString) : NaN;

      if (isNaN(id)) {
        this.movie = null;
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      this.isLoading = true;
      this.movieService.getMovieById(id).subscribe({
        next: data => {
          this.movie = data;
          this.isLoading = false;
          this.cdr.detectChanges(); // wymusza odświeżenie UI
        },
        error: () => {
          this.movie = null;
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    });
  }
}
