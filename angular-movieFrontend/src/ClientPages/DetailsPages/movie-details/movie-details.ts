import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MovieService } from '../../../Services/MovieService';
import { ReviewService } from '../../../Services/ReviewService';
import { PLATFORM_ID, Inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
@Component({
  standalone: true,
  selector: 'app-movie-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.css',
})
export class MovieDetails implements OnInit {
  movie: any;
  movieId:string = '';
  isLoading: boolean = true;
  newReview = {
  Rating: 10,
  Comment: ''
};
showReviewForm = false;
isLoggedIn = false;
  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService,
    private reviewService: ReviewService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ){}
  ngOnInit(): void{
     if (isPlatformBrowser(this.platformId)) {
    const state = sessionStorage.getItem("isLoggedIn");
    this.isLoggedIn = state === "true";
    }
     this.route.paramMap.subscribe(params => {
      const idString = params.get('id');

      if (!idString) {
        this.movie = null;
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      this.isLoading = true;
      this.movieService.getMovieById(idString).subscribe({
        next: data => {
          this.movie = data;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.movie = null;
          this.isLoading = false;
          this.cdr.detectChanges();
        }
      });
    });
  }
  toggleReviewForm() {
  this.showReviewForm = !this.showReviewForm;
}

submitReview() {
  const reviewData = {
    ...this.newReview,
  };

  this.reviewService.addReview(this.movie.id, reviewData).subscribe({
    next: (savedReview) => {
      this.movie.reviews.unshift(savedReview);
      this.showReviewForm = false;
      this.newReview = { Rating: 10, Comment: '' };
    },
    error: (err) => alert('Błąd podczas dodawania recenzji.')
  });
}
}
