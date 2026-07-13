import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MovieService } from '../../../Services/MovieService';
import { ReviewService } from '../../../Services/ReviewService';
import { FormsModule } from '@angular/forms';
import { GameService } from '../../../Services/GameService';

@Component({
  selector: 'app-games-details',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './games-details.html',
  styleUrl: './games-details.css',
})
export class GamesDetails implements OnInit {
  game: any;
  gameId:string = '';
  isLoading: boolean = true;
  newReview = {
  Rating: 10,
  Comment: ''
};
showReviewForm = false;
isLoggedIn = false;
  constructor(
    private route: ActivatedRoute,
    private gameService: GameService,
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
        this.game = null;
        this.isLoading = false;
        this.cdr.detectChanges();
        return;
      }

      this.isLoading = true;
      this.gameService.getGameById(idString).subscribe({
        next: data => {
          this.game = data;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.game = null;
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

  this.reviewService.addReview(this.game.id, reviewData).subscribe({
    next: (savedReview) => {
      this.game.reviews.unshift(savedReview);
      this.showReviewForm = false;
      this.newReview = { Rating: 10, Comment: '' };
    },
    error: (err) => alert('Błąd podczas dodawania recenzji.')
  });
}

}
